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
    public partial class Test_QueryAndControlNetlogonBehavior_NetrLogonControl : PtfTestClassBase {
        
        public Test_QueryAndControlNetlogonBehavior_NetrLogonControl() {
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
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp13 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetPlatformChecker2)));
            if ((temp13 == 0)) {
                this.Manager.Comment("reaching state \'S134\'");
                bool temp1;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp1);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp1);
                this.Manager.Comment("reaching state \'S335\'");
                int temp4 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetClientAccountTypeChecker1)));
                if ((temp4 == 0)) {
                    this.Manager.Comment("reaching state \'S536\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp2;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,3)\'");
                    temp2 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 3u);
                    this.Manager.Checkpoint("MS-NRPC_R104022");
                    this.Manager.Comment("reaching state \'S938\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_INVALID_LEVEL\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_INVALID_LEVEL, temp2, "return of NetrLogonControl, state S938");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label0;
                }
                if ((temp4 == 1)) {
                    this.Manager.Comment("reaching state \'S537\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp3;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,3)\'");
                    temp3 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 3u);
                    this.Manager.Comment("reaching state \'S939\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp3, "return of NetrLogonControl, state S939");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label0;
                }
                throw new InvalidOperationException("never reached");
            label0:
;
                goto label3;
            }
            if ((temp13 == 1)) {
                this.Manager.Comment("reaching state \'S135\'");
                bool temp5;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp5);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp5);
                this.Manager.Comment("reaching state \'S336\'");
                int temp8 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetClientAccountTypeChecker3)));
                if ((temp8 == 0)) {
                    this.Manager.Comment("reaching state \'S538\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp6;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,3)\'");
                    temp6 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 3u);
                    this.Manager.Checkpoint("MS-NRPC_R104022");
                    this.Manager.Comment("reaching state \'S940\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_INVALID_LEVEL\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_INVALID_LEVEL, temp6, "return of NetrLogonControl, state S940");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label1;
                }
                if ((temp8 == 1)) {
                    this.Manager.Comment("reaching state \'S539\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp7;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,3)\'");
                    temp7 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 3u);
                    this.Manager.Comment("reaching state \'S941\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp7, "return of NetrLogonControl, state S941");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label1;
                }
                throw new InvalidOperationException("never reached");
            label1:
;
                goto label3;
            }
            if ((temp13 == 2)) {
                this.Manager.Comment("reaching state \'S136\'");
                bool temp9;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp9);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp9);
                this.Manager.Comment("reaching state \'S337\'");
                int temp12 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetClientAccountTypeChecker5)));
                if ((temp12 == 0)) {
                    this.Manager.Comment("reaching state \'S540\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp10;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,3)\'");
                    temp10 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 3u);
                    this.Manager.Checkpoint("MS-NRPC_R104022");
                    this.Manager.Comment("reaching state \'S942\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_INVALID_LEVEL\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_INVALID_LEVEL, temp10, "return of NetrLogonControl, state S942");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label2;
                }
                if ((temp12 == 1)) {
                    this.Manager.Comment("reaching state \'S541\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp11;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,3)\'");
                    temp11 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 3u);
                    this.Manager.Comment("reaching state \'S943\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp11, "return of NetrLogonControl, state S943");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S335");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340() {
            this.Manager.Comment("reaching state \'S1340\'");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S335");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341() {
            this.Manager.Comment("reaching state \'S1341\'");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S336");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342() {
            this.Manager.Comment("reaching state \'S1342\'");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S336");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343() {
            this.Manager.Comment("reaching state \'S1343\'");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S337");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344() {
            this.Manager.Comment("reaching state \'S1344\'");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS0GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S337");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345() {
            this.Manager.Comment("reaching state \'S1345\'");
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp14;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp14);
            this.Manager.AddReturn(GetPlatformInfo, null, temp14);
            this.Manager.Comment("reaching state \'S11\'");
            int temp27 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetPlatformChecker2)));
            if ((temp27 == 0)) {
                this.Manager.Comment("reaching state \'S149\'");
                bool temp15;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp15);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp15);
                this.Manager.Comment("reaching state \'S350\'");
                int temp18 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetClientAccountTypeChecker1)));
                if ((temp18 == 0)) {
                    this.Manager.Comment("reaching state \'S566\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp16;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,5,1)\'");
                    temp16 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 5u, 1u);
                    this.Manager.Comment("reaching state \'S968\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp16, "return of NetrLogonControl, state S968");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label4;
                }
                if ((temp18 == 1)) {
                    this.Manager.Comment("reaching state \'S567\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp17;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,8,1)\'");
                    temp17 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 8u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S969\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_INVALID_PARAMETER, temp17, "return of NetrLogonControl, state S969");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label4;
                }
                throw new InvalidOperationException("never reached");
            label4:
;
                goto label7;
            }
            if ((temp27 == 1)) {
                this.Manager.Comment("reaching state \'S150\'");
                bool temp19;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp19);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp19);
                this.Manager.Comment("reaching state \'S351\'");
                int temp22 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetClientAccountTypeChecker3)));
                if ((temp22 == 0)) {
                    this.Manager.Comment("reaching state \'S568\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp20;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,5,1)\'");
                    temp20 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 5u, 1u);
                    this.Manager.Comment("reaching state \'S970\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp20, "return of NetrLogonControl, state S970");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label5;
                }
                if ((temp22 == 1)) {
                    this.Manager.Comment("reaching state \'S569\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp21;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,6,1)\'");
                    temp21 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 6u, 1u);
                    this.Manager.Comment("reaching state \'S971\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp21, "return of NetrLogonControl, state S971");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label5;
                }
                throw new InvalidOperationException("never reached");
            label5:
;
                goto label7;
            }
            if ((temp27 == 2)) {
                this.Manager.Comment("reaching state \'S151\'");
                bool temp23;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp23);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp23);
                this.Manager.Comment("reaching state \'S352\'");
                int temp26 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetClientAccountTypeChecker5)));
                if ((temp26 == 0)) {
                    this.Manager.Comment("reaching state \'S570\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp24;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp24 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S972\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp24, "return of NetrLogonControl, state S972");
                    this.Manager.Comment("reaching state \'S1354\'");
                    goto label6;
                }
                if ((temp26 == 1)) {
                    this.Manager.Comment("reaching state \'S571\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp25;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp25 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S973\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp25, "return of NetrLogonControl, state S973");
                    this.Manager.Comment("reaching state \'S1355\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S350");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S350");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S351");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S351");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S352");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS10GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S352");
        }
        #endregion
        
        #region Test Starting in S100
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100");
            this.Manager.Comment("reaching state \'S100\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp28;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp28);
            this.Manager.AddReturn(GetPlatformInfo, null, temp28);
            this.Manager.Comment("reaching state \'S101\'");
            int temp41 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetPlatformChecker2)));
            if ((temp41 == 0)) {
                this.Manager.Comment("reaching state \'S284\'");
                bool temp29;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp29);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp29);
                this.Manager.Comment("reaching state \'S485\'");
                int temp32 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetClientAccountTypeChecker1)));
                if ((temp32 == 0)) {
                    this.Manager.Comment("reaching state \'S836\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp30;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,1)\'");
                    temp30 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1238\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp30, "return of NetrLogonControl, state S1238");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label8;
                }
                if ((temp32 == 1)) {
                    this.Manager.Comment("reaching state \'S837\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp31;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65535,1)\'");
                    temp31 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1239\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp31, "return of NetrLogonControl, state S1239");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label8;
                }
                throw new InvalidOperationException("never reached");
            label8:
;
                goto label11;
            }
            if ((temp41 == 1)) {
                this.Manager.Comment("reaching state \'S285\'");
                bool temp33;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp33);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp33);
                this.Manager.Comment("reaching state \'S486\'");
                int temp36 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetClientAccountTypeChecker3)));
                if ((temp36 == 0)) {
                    this.Manager.Comment("reaching state \'S838\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp34;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp34 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1240\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp34, "return of NetrLogonControl, state S1240");
                    this.Manager.Comment("reaching state \'S1480\'");
                    goto label9;
                }
                if ((temp36 == 1)) {
                    this.Manager.Comment("reaching state \'S839\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp35;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp35 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1241\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp35, "return of NetrLogonControl, state S1241");
                    this.Manager.Comment("reaching state \'S1481\'");
                    goto label9;
                }
                throw new InvalidOperationException("never reached");
            label9:
;
                goto label11;
            }
            if ((temp41 == 2)) {
                this.Manager.Comment("reaching state \'S286\'");
                bool temp37;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp37);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp37);
                this.Manager.Comment("reaching state \'S487\'");
                int temp40 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetClientAccountTypeChecker5)));
                if ((temp40 == 0)) {
                    this.Manager.Comment("reaching state \'S840\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp38;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp38 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1242\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp38, "return of NetrLogonControl, state S1242");
                    this.Manager.Comment("reaching state \'S1482\'");
                    goto label10;
                }
                if ((temp40 == 1)) {
                    this.Manager.Comment("reaching state \'S841\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp39;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp39 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1243\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp39, "return of NetrLogonControl, state S1243");
                    this.Manager.Comment("reaching state \'S1483\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S485");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S485");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S486");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S486");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S487");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS100GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S487");
        }
        #endregion
        
        #region Test Starting in S102
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102");
            this.Manager.Comment("reaching state \'S102\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp42;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp42);
            this.Manager.AddReturn(GetPlatformInfo, null, temp42);
            this.Manager.Comment("reaching state \'S103\'");
            int temp55 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetPlatformChecker2)));
            if ((temp55 == 0)) {
                this.Manager.Comment("reaching state \'S287\'");
                bool temp43;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp43);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp43);
                this.Manager.Comment("reaching state \'S488\'");
                int temp46 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetClientAccountTypeChecker1)));
                if ((temp46 == 0)) {
                    this.Manager.Comment("reaching state \'S842\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp44;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,2,1)\'");
                    temp44 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 2u, 1u);
                    this.Manager.Comment("reaching state \'S1244\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp44, "return of NetrLogonControl, state S1244");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label12;
                }
                if ((temp46 == 1)) {
                    this.Manager.Comment("reaching state \'S843\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp45;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,4,1)\'");
                    temp45 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 4u, 1u);
                    this.Manager.Comment("reaching state \'S1245\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp45, "return of NetrLogonControl, state S1245");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label12;
                }
                throw new InvalidOperationException("never reached");
            label12:
;
                goto label15;
            }
            if ((temp55 == 1)) {
                this.Manager.Comment("reaching state \'S288\'");
                bool temp47;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp47);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp47);
                this.Manager.Comment("reaching state \'S489\'");
                int temp50 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetClientAccountTypeChecker3)));
                if ((temp50 == 0)) {
                    this.Manager.Comment("reaching state \'S844\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp48;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp48 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1246\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp48, "return of NetrLogonControl, state S1246");
                    this.Manager.Comment("reaching state \'S1484\'");
                    goto label13;
                }
                if ((temp50 == 1)) {
                    this.Manager.Comment("reaching state \'S845\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp49;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp49 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1247\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp49, "return of NetrLogonControl, state S1247");
                    this.Manager.Comment("reaching state \'S1485\'");
                    goto label13;
                }
                throw new InvalidOperationException("never reached");
            label13:
;
                goto label15;
            }
            if ((temp55 == 2)) {
                this.Manager.Comment("reaching state \'S289\'");
                bool temp51;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp51);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp51);
                this.Manager.Comment("reaching state \'S490\'");
                int temp54 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetClientAccountTypeChecker5)));
                if ((temp54 == 0)) {
                    this.Manager.Comment("reaching state \'S846\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp52;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp52 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1248\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp52, "return of NetrLogonControl, state S1248");
                    this.Manager.Comment("reaching state \'S1486\'");
                    goto label14;
                }
                if ((temp54 == 1)) {
                    this.Manager.Comment("reaching state \'S847\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp53;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp53 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1249\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp53, "return of NetrLogonControl, state S1249");
                    this.Manager.Comment("reaching state \'S1487\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S488");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S488");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S489");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S489");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S490");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS102GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S490");
        }
        #endregion
        
        #region Test Starting in S104
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104");
            this.Manager.Comment("reaching state \'S104\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp56;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp56);
            this.Manager.AddReturn(GetPlatformInfo, null, temp56);
            this.Manager.Comment("reaching state \'S105\'");
            int temp69 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetPlatformChecker2)));
            if ((temp69 == 0)) {
                this.Manager.Comment("reaching state \'S290\'");
                bool temp57;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp57);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp57);
                this.Manager.Comment("reaching state \'S491\'");
                int temp60 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetClientAccountTypeChecker1)));
                if ((temp60 == 0)) {
                    this.Manager.Comment("reaching state \'S848\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp58;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,3,1)\'");
                    temp58 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 3u, 1u);
                    this.Manager.Comment("reaching state \'S1250\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp58, "return of NetrLogonControl, state S1250");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label16;
                }
                if ((temp60 == 1)) {
                    this.Manager.Comment("reaching state \'S849\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp59;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,2,1)\'");
                    temp59 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 2u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104925");
                    this.Manager.Comment("reaching state \'S1251\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp59, "return of NetrLogonControl, state S1251");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label16;
                }
                throw new InvalidOperationException("never reached");
            label16:
;
                goto label19;
            }
            if ((temp69 == 1)) {
                this.Manager.Comment("reaching state \'S291\'");
                bool temp61;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp61);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp61);
                this.Manager.Comment("reaching state \'S492\'");
                int temp64 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetClientAccountTypeChecker3)));
                if ((temp64 == 0)) {
                    this.Manager.Comment("reaching state \'S850\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp62;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp62 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1252\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp62, "return of NetrLogonControl, state S1252");
                    this.Manager.Comment("reaching state \'S1488\'");
                    goto label17;
                }
                if ((temp64 == 1)) {
                    this.Manager.Comment("reaching state \'S851\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp63;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp63 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1253\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp63, "return of NetrLogonControl, state S1253");
                    this.Manager.Comment("reaching state \'S1489\'");
                    goto label17;
                }
                throw new InvalidOperationException("never reached");
            label17:
;
                goto label19;
            }
            if ((temp69 == 2)) {
                this.Manager.Comment("reaching state \'S292\'");
                bool temp65;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp65);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp65);
                this.Manager.Comment("reaching state \'S493\'");
                int temp68 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetClientAccountTypeChecker5)));
                if ((temp68 == 0)) {
                    this.Manager.Comment("reaching state \'S852\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp66;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp66 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1254\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp66, "return of NetrLogonControl, state S1254");
                    this.Manager.Comment("reaching state \'S1490\'");
                    goto label18;
                }
                if ((temp68 == 1)) {
                    this.Manager.Comment("reaching state \'S853\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp67;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp67 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1255\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp67, "return of NetrLogonControl, state S1255");
                    this.Manager.Comment("reaching state \'S1491\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S491");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S491");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S492");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S492");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S493");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS104GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S493");
        }
        #endregion
        
        #region Test Starting in S106
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106");
            this.Manager.Comment("reaching state \'S106\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp70;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp70);
            this.Manager.AddReturn(GetPlatformInfo, null, temp70);
            this.Manager.Comment("reaching state \'S107\'");
            int temp83 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetPlatformChecker2)));
            if ((temp83 == 0)) {
                this.Manager.Comment("reaching state \'S293\'");
                bool temp71;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp71);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp71);
                this.Manager.Comment("reaching state \'S494\'");
                int temp74 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetClientAccountTypeChecker1)));
                if ((temp74 == 0)) {
                    this.Manager.Comment("reaching state \'S854\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp72;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,4,1)\'");
                    temp72 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 4u, 1u);
                    this.Manager.Comment("reaching state \'S1256\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp72, "return of NetrLogonControl, state S1256");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label20;
                }
                if ((temp74 == 1)) {
                    this.Manager.Comment("reaching state \'S855\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp73;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,6,1)\'");
                    temp73 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 6u, 1u);
                    this.Manager.Comment("reaching state \'S1257\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp73, "return of NetrLogonControl, state S1257");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label20;
                }
                throw new InvalidOperationException("never reached");
            label20:
;
                goto label23;
            }
            if ((temp83 == 1)) {
                this.Manager.Comment("reaching state \'S294\'");
                bool temp75;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp75);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp75);
                this.Manager.Comment("reaching state \'S495\'");
                int temp78 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetClientAccountTypeChecker3)));
                if ((temp78 == 0)) {
                    this.Manager.Comment("reaching state \'S856\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp76;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp76 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1258\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp76, "return of NetrLogonControl, state S1258");
                    this.Manager.Comment("reaching state \'S1492\'");
                    goto label21;
                }
                if ((temp78 == 1)) {
                    this.Manager.Comment("reaching state \'S857\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp77;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp77 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1259\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp77, "return of NetrLogonControl, state S1259");
                    this.Manager.Comment("reaching state \'S1493\'");
                    goto label21;
                }
                throw new InvalidOperationException("never reached");
            label21:
;
                goto label23;
            }
            if ((temp83 == 2)) {
                this.Manager.Comment("reaching state \'S295\'");
                bool temp79;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp79);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp79);
                this.Manager.Comment("reaching state \'S496\'");
                int temp82 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetClientAccountTypeChecker5)));
                if ((temp82 == 0)) {
                    this.Manager.Comment("reaching state \'S858\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp80;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp80 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1260\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp80, "return of NetrLogonControl, state S1260");
                    this.Manager.Comment("reaching state \'S1494\'");
                    goto label22;
                }
                if ((temp82 == 1)) {
                    this.Manager.Comment("reaching state \'S859\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp81;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp81 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1261\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp81, "return of NetrLogonControl, state S1261");
                    this.Manager.Comment("reaching state \'S1495\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S494");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S494");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S495");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S495");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S496");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS106GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S496");
        }
        #endregion
        
        #region Test Starting in S108
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108");
            this.Manager.Comment("reaching state \'S108\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp84;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp84);
            this.Manager.AddReturn(GetPlatformInfo, null, temp84);
            this.Manager.Comment("reaching state \'S109\'");
            int temp97 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetPlatformChecker2)));
            if ((temp97 == 0)) {
                this.Manager.Comment("reaching state \'S296\'");
                bool temp85;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp85);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp85);
                this.Manager.Comment("reaching state \'S497\'");
                int temp88 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetClientAccountTypeChecker1)));
                if ((temp88 == 0)) {
                    this.Manager.Comment("reaching state \'S860\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp86;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,5,1)\'");
                    temp86 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 5u, 1u);
                    this.Manager.Comment("reaching state \'S1262\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp86, "return of NetrLogonControl, state S1262");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label24;
                }
                if ((temp88 == 1)) {
                    this.Manager.Comment("reaching state \'S861\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp87;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,1)\'");
                    temp87 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1263\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp87, "return of NetrLogonControl, state S1263");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label24;
                }
                throw new InvalidOperationException("never reached");
            label24:
;
                goto label27;
            }
            if ((temp97 == 1)) {
                this.Manager.Comment("reaching state \'S297\'");
                bool temp89;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp89);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp89);
                this.Manager.Comment("reaching state \'S498\'");
                int temp92 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetClientAccountTypeChecker3)));
                if ((temp92 == 0)) {
                    this.Manager.Comment("reaching state \'S862\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp90;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp90 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1264\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp90, "return of NetrLogonControl, state S1264");
                    this.Manager.Comment("reaching state \'S1496\'");
                    goto label25;
                }
                if ((temp92 == 1)) {
                    this.Manager.Comment("reaching state \'S863\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp91;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp91 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1265\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp91, "return of NetrLogonControl, state S1265");
                    this.Manager.Comment("reaching state \'S1497\'");
                    goto label25;
                }
                throw new InvalidOperationException("never reached");
            label25:
;
                goto label27;
            }
            if ((temp97 == 2)) {
                this.Manager.Comment("reaching state \'S298\'");
                bool temp93;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp93);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp93);
                this.Manager.Comment("reaching state \'S499\'");
                int temp96 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetClientAccountTypeChecker5)));
                if ((temp96 == 0)) {
                    this.Manager.Comment("reaching state \'S864\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp94;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp94 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1266\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp94, "return of NetrLogonControl, state S1266");
                    this.Manager.Comment("reaching state \'S1498\'");
                    goto label26;
                }
                if ((temp96 == 1)) {
                    this.Manager.Comment("reaching state \'S865\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp95;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp95 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1267\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp95, "return of NetrLogonControl, state S1267");
                    this.Manager.Comment("reaching state \'S1499\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S497");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S497");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S498");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S498");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S499");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS108GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S499");
        }
        #endregion
        
        #region Test Starting in S110
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110");
            this.Manager.Comment("reaching state \'S110\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp98;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp98);
            this.Manager.AddReturn(GetPlatformInfo, null, temp98);
            this.Manager.Comment("reaching state \'S111\'");
            int temp111 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetPlatformChecker2)));
            if ((temp111 == 0)) {
                this.Manager.Comment("reaching state \'S299\'");
                bool temp99;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp99);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp99);
                this.Manager.Comment("reaching state \'S500\'");
                int temp102 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetClientAccountTypeChecker1)));
                if ((temp102 == 0)) {
                    this.Manager.Comment("reaching state \'S866\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp100;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,6,1)\'");
                    temp100 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 6u, 1u);
                    this.Manager.Comment("reaching state \'S1268\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp100, "return of NetrLogonControl, state S1268");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label28;
                }
                if ((temp102 == 1)) {
                    this.Manager.Comment("reaching state \'S867\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp101;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,7,1)\'");
                    temp101 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 7u, 1u);
                    this.Manager.Comment("reaching state \'S1269\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp101, "return of NetrLogonControl, state S1269");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label28;
                }
                throw new InvalidOperationException("never reached");
            label28:
;
                goto label31;
            }
            if ((temp111 == 1)) {
                this.Manager.Comment("reaching state \'S300\'");
                bool temp103;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp103);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp103);
                this.Manager.Comment("reaching state \'S501\'");
                int temp106 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetClientAccountTypeChecker3)));
                if ((temp106 == 0)) {
                    this.Manager.Comment("reaching state \'S868\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp104;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp104 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1270\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp104, "return of NetrLogonControl, state S1270");
                    this.Manager.Comment("reaching state \'S1500\'");
                    goto label29;
                }
                if ((temp106 == 1)) {
                    this.Manager.Comment("reaching state \'S869\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp105;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp105 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1271\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp105, "return of NetrLogonControl, state S1271");
                    this.Manager.Comment("reaching state \'S1501\'");
                    goto label29;
                }
                throw new InvalidOperationException("never reached");
            label29:
;
                goto label31;
            }
            if ((temp111 == 2)) {
                this.Manager.Comment("reaching state \'S301\'");
                bool temp107;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp107);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp107);
                this.Manager.Comment("reaching state \'S502\'");
                int temp110 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetClientAccountTypeChecker5)));
                if ((temp110 == 0)) {
                    this.Manager.Comment("reaching state \'S870\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp108;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp108 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1272\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp108, "return of NetrLogonControl, state S1272");
                    this.Manager.Comment("reaching state \'S1502\'");
                    goto label30;
                }
                if ((temp110 == 1)) {
                    this.Manager.Comment("reaching state \'S871\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp109;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp109 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1273\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp109, "return of NetrLogonControl, state S1273");
                    this.Manager.Comment("reaching state \'S1503\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S500");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S500");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S501");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S501");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S502");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS110GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S502");
        }
        #endregion
        
        #region Test Starting in S112
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112");
            this.Manager.Comment("reaching state \'S112\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp112;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp112);
            this.Manager.AddReturn(GetPlatformInfo, null, temp112);
            this.Manager.Comment("reaching state \'S113\'");
            int temp125 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetPlatformChecker2)));
            if ((temp125 == 0)) {
                this.Manager.Comment("reaching state \'S302\'");
                bool temp113;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp113);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp113);
                this.Manager.Comment("reaching state \'S503\'");
                int temp116 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetClientAccountTypeChecker1)));
                if ((temp116 == 0)) {
                    this.Manager.Comment("reaching state \'S872\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp114;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,7,1)\'");
                    temp114 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 7u, 1u);
                    this.Manager.Comment("reaching state \'S1274\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp114, "return of NetrLogonControl, state S1274");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label32;
                }
                if ((temp116 == 1)) {
                    this.Manager.Comment("reaching state \'S873\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp115;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,3,1)\'");
                    temp115 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 3u, 1u);
                    this.Manager.Comment("reaching state \'S1275\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp115, "return of NetrLogonControl, state S1275");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label32;
                }
                throw new InvalidOperationException("never reached");
            label32:
;
                goto label35;
            }
            if ((temp125 == 1)) {
                this.Manager.Comment("reaching state \'S303\'");
                bool temp117;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp117);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp117);
                this.Manager.Comment("reaching state \'S504\'");
                int temp120 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetClientAccountTypeChecker3)));
                if ((temp120 == 0)) {
                    this.Manager.Comment("reaching state \'S874\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp118;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp118 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1276\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp118, "return of NetrLogonControl, state S1276");
                    this.Manager.Comment("reaching state \'S1504\'");
                    goto label33;
                }
                if ((temp120 == 1)) {
                    this.Manager.Comment("reaching state \'S875\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp119;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp119 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1277\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp119, "return of NetrLogonControl, state S1277");
                    this.Manager.Comment("reaching state \'S1505\'");
                    goto label33;
                }
                throw new InvalidOperationException("never reached");
            label33:
;
                goto label35;
            }
            if ((temp125 == 2)) {
                this.Manager.Comment("reaching state \'S304\'");
                bool temp121;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp121);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp121);
                this.Manager.Comment("reaching state \'S505\'");
                int temp124 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetClientAccountTypeChecker5)));
                if ((temp124 == 0)) {
                    this.Manager.Comment("reaching state \'S876\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp122;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp122 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1278\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp122, "return of NetrLogonControl, state S1278");
                    this.Manager.Comment("reaching state \'S1506\'");
                    goto label34;
                }
                if ((temp124 == 1)) {
                    this.Manager.Comment("reaching state \'S877\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp123;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp123 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1279\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp123, "return of NetrLogonControl, state S1279");
                    this.Manager.Comment("reaching state \'S1507\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S113");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S503");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S503");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S113");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S504");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S504");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S113");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S505");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS112GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S505");
        }
        #endregion
        
        #region Test Starting in S114
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114");
            this.Manager.Comment("reaching state \'S114\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp126;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp126);
            this.Manager.AddReturn(GetPlatformInfo, null, temp126);
            this.Manager.Comment("reaching state \'S115\'");
            int temp139 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetPlatformChecker2)));
            if ((temp139 == 0)) {
                this.Manager.Comment("reaching state \'S305\'");
                bool temp127;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp127);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp127);
                this.Manager.Comment("reaching state \'S506\'");
                int temp130 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetClientAccountTypeChecker1)));
                if ((temp130 == 0)) {
                    this.Manager.Comment("reaching state \'S878\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp128;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,8,1)\'");
                    temp128 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8u, 1u);
                    this.Manager.Comment("reaching state \'S1280\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp128, "return of NetrLogonControl, state S1280");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label36;
                }
                if ((temp130 == 1)) {
                    this.Manager.Comment("reaching state \'S879\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp129;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,5,1)\'");
                    temp129 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 5u, 1u);
                    this.Manager.Comment("reaching state \'S1281\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp129, "return of NetrLogonControl, state S1281");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label36;
                }
                throw new InvalidOperationException("never reached");
            label36:
;
                goto label39;
            }
            if ((temp139 == 1)) {
                this.Manager.Comment("reaching state \'S306\'");
                bool temp131;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp131);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp131);
                this.Manager.Comment("reaching state \'S507\'");
                int temp134 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetClientAccountTypeChecker3)));
                if ((temp134 == 0)) {
                    this.Manager.Comment("reaching state \'S880\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp132;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp132 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1282\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp132, "return of NetrLogonControl, state S1282");
                    this.Manager.Comment("reaching state \'S1508\'");
                    goto label37;
                }
                if ((temp134 == 1)) {
                    this.Manager.Comment("reaching state \'S881\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp133;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp133 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1283\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp133, "return of NetrLogonControl, state S1283");
                    this.Manager.Comment("reaching state \'S1509\'");
                    goto label37;
                }
                throw new InvalidOperationException("never reached");
            label37:
;
                goto label39;
            }
            if ((temp139 == 2)) {
                this.Manager.Comment("reaching state \'S307\'");
                bool temp135;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp135);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp135);
                this.Manager.Comment("reaching state \'S508\'");
                int temp138 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetClientAccountTypeChecker5)));
                if ((temp138 == 0)) {
                    this.Manager.Comment("reaching state \'S882\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp136;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp136 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1284\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp136, "return of NetrLogonControl, state S1284");
                    this.Manager.Comment("reaching state \'S1510\'");
                    goto label38;
                }
                if ((temp138 == 1)) {
                    this.Manager.Comment("reaching state \'S883\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp137;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp137 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1285\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp137, "return of NetrLogonControl, state S1285");
                    this.Manager.Comment("reaching state \'S1511\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S115");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S506");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S506");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S115");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S507");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S507");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S115");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S508");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS114GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S508");
        }
        #endregion
        
        #region Test Starting in S116
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116");
            this.Manager.Comment("reaching state \'S116\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp140;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp140);
            this.Manager.AddReturn(GetPlatformInfo, null, temp140);
            this.Manager.Comment("reaching state \'S117\'");
            int temp153 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetPlatformChecker2)));
            if ((temp153 == 0)) {
                this.Manager.Comment("reaching state \'S308\'");
                bool temp141;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp141);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp141);
                this.Manager.Comment("reaching state \'S509\'");
                int temp144 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetClientAccountTypeChecker1)));
                if ((temp144 == 0)) {
                    this.Manager.Comment("reaching state \'S884\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp142;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,9,1)\'");
                    temp142 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 9u, 1u);
                    this.Manager.Comment("reaching state \'S1286\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp142, "return of NetrLogonControl, state S1286");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label40;
                }
                if ((temp144 == 1)) {
                    this.Manager.Comment("reaching state \'S885\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp143;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65532,1)\'");
                    temp143 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65532u, 1u);
                    this.Manager.Comment("reaching state \'S1287\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp143, "return of NetrLogonControl, state S1287");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label40;
                }
                throw new InvalidOperationException("never reached");
            label40:
;
                goto label43;
            }
            if ((temp153 == 1)) {
                this.Manager.Comment("reaching state \'S309\'");
                bool temp145;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp145);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp145);
                this.Manager.Comment("reaching state \'S510\'");
                int temp148 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetClientAccountTypeChecker3)));
                if ((temp148 == 0)) {
                    this.Manager.Comment("reaching state \'S886\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp146;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp146 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1288\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp146, "return of NetrLogonControl, state S1288");
                    this.Manager.Comment("reaching state \'S1512\'");
                    goto label41;
                }
                if ((temp148 == 1)) {
                    this.Manager.Comment("reaching state \'S887\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp147;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp147 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1289\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp147, "return of NetrLogonControl, state S1289");
                    this.Manager.Comment("reaching state \'S1513\'");
                    goto label41;
                }
                throw new InvalidOperationException("never reached");
            label41:
;
                goto label43;
            }
            if ((temp153 == 2)) {
                this.Manager.Comment("reaching state \'S310\'");
                bool temp149;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp149);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp149);
                this.Manager.Comment("reaching state \'S511\'");
                int temp152 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetClientAccountTypeChecker5)));
                if ((temp152 == 0)) {
                    this.Manager.Comment("reaching state \'S888\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp150;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp150 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1290\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp150, "return of NetrLogonControl, state S1290");
                    this.Manager.Comment("reaching state \'S1514\'");
                    goto label42;
                }
                if ((temp152 == 1)) {
                    this.Manager.Comment("reaching state \'S889\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp151;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp151 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1291\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp151, "return of NetrLogonControl, state S1291");
                    this.Manager.Comment("reaching state \'S1515\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S117");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S509");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S509");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S117");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S510");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S510");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S117");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S511");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS116GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S511");
        }
        #endregion
        
        #region Test Starting in S118
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118");
            this.Manager.Comment("reaching state \'S118\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp154;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp154);
            this.Manager.AddReturn(GetPlatformInfo, null, temp154);
            this.Manager.Comment("reaching state \'S119\'");
            int temp167 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetPlatformChecker2)));
            if ((temp167 == 0)) {
                this.Manager.Comment("reaching state \'S311\'");
                bool temp155;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp155);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp155);
                this.Manager.Comment("reaching state \'S512\'");
                int temp158 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetClientAccountTypeChecker1)));
                if ((temp158 == 0)) {
                    this.Manager.Comment("reaching state \'S890\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp156;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,10,1)\'");
                    temp156 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 10u, 1u);
                    this.Manager.Comment("reaching state \'S1292\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp156, "return of NetrLogonControl, state S1292");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label44;
                }
                if ((temp158 == 1)) {
                    this.Manager.Comment("reaching state \'S891\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp157;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,10,1)\'");
                    temp157 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 10u, 1u);
                    this.Manager.Comment("reaching state \'S1293\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp157, "return of NetrLogonControl, state S1293");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label44;
                }
                throw new InvalidOperationException("never reached");
            label44:
;
                goto label47;
            }
            if ((temp167 == 1)) {
                this.Manager.Comment("reaching state \'S312\'");
                bool temp159;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp159);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp159);
                this.Manager.Comment("reaching state \'S513\'");
                int temp162 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetClientAccountTypeChecker3)));
                if ((temp162 == 0)) {
                    this.Manager.Comment("reaching state \'S892\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp160;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp160 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1294\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp160, "return of NetrLogonControl, state S1294");
                    this.Manager.Comment("reaching state \'S1516\'");
                    goto label45;
                }
                if ((temp162 == 1)) {
                    this.Manager.Comment("reaching state \'S893\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp161;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp161 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1295\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp161, "return of NetrLogonControl, state S1295");
                    this.Manager.Comment("reaching state \'S1517\'");
                    goto label45;
                }
                throw new InvalidOperationException("never reached");
            label45:
;
                goto label47;
            }
            if ((temp167 == 2)) {
                this.Manager.Comment("reaching state \'S313\'");
                bool temp163;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp163);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp163);
                this.Manager.Comment("reaching state \'S514\'");
                int temp166 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetClientAccountTypeChecker5)));
                if ((temp166 == 0)) {
                    this.Manager.Comment("reaching state \'S894\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp164;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp164 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1296\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp164, "return of NetrLogonControl, state S1296");
                    this.Manager.Comment("reaching state \'S1518\'");
                    goto label46;
                }
                if ((temp166 == 1)) {
                    this.Manager.Comment("reaching state \'S895\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp165;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp165 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1297\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp165, "return of NetrLogonControl, state S1297");
                    this.Manager.Comment("reaching state \'S1519\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S119");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S512");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S512");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S119");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S513");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S513");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S119");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S514");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS118GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S514");
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp168;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp168);
            this.Manager.AddReturn(GetPlatformInfo, null, temp168);
            this.Manager.Comment("reaching state \'S13\'");
            int temp181 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetPlatformChecker2)));
            if ((temp181 == 0)) {
                this.Manager.Comment("reaching state \'S152\'");
                bool temp169;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp169);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp169);
                this.Manager.Comment("reaching state \'S353\'");
                int temp172 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetClientAccountTypeChecker1)));
                if ((temp172 == 0)) {
                    this.Manager.Comment("reaching state \'S572\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp170;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,6,1)\'");
                    temp170 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 6u, 1u);
                    this.Manager.Comment("reaching state \'S974\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp170, "return of NetrLogonControl, state S974");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label48;
                }
                if ((temp172 == 1)) {
                    this.Manager.Comment("reaching state \'S573\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp171;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,4,1)\'");
                    temp171 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 4u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S975\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp171, "return of NetrLogonControl, state S975");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label48;
                }
                throw new InvalidOperationException("never reached");
            label48:
;
                goto label51;
            }
            if ((temp181 == 1)) {
                this.Manager.Comment("reaching state \'S153\'");
                bool temp173;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp173);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp173);
                this.Manager.Comment("reaching state \'S354\'");
                int temp176 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetClientAccountTypeChecker3)));
                if ((temp176 == 0)) {
                    this.Manager.Comment("reaching state \'S574\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp174;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,6,1)\'");
                    temp174 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 6u, 1u);
                    this.Manager.Comment("reaching state \'S976\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp174, "return of NetrLogonControl, state S976");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label49;
                }
                if ((temp176 == 1)) {
                    this.Manager.Comment("reaching state \'S575\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp175;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp175 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Comment("reaching state \'S977\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp175, "return of NetrLogonControl, state S977");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label49;
                }
                throw new InvalidOperationException("never reached");
            label49:
;
                goto label51;
            }
            if ((temp181 == 2)) {
                this.Manager.Comment("reaching state \'S154\'");
                bool temp177;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp177);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp177);
                this.Manager.Comment("reaching state \'S355\'");
                int temp180 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetClientAccountTypeChecker5)));
                if ((temp180 == 0)) {
                    this.Manager.Comment("reaching state \'S576\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp178;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp178 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S978\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp178, "return of NetrLogonControl, state S978");
                    this.Manager.Comment("reaching state \'S1356\'");
                    goto label50;
                }
                if ((temp180 == 1)) {
                    this.Manager.Comment("reaching state \'S577\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp179;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp179 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S979\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp179, "return of NetrLogonControl, state S979");
                    this.Manager.Comment("reaching state \'S1357\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S353");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S353");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S354");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S354");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S355");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS12GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S355");
        }
        #endregion
        
        #region Test Starting in S120
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120");
            this.Manager.Comment("reaching state \'S120\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp182;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp182);
            this.Manager.AddReturn(GetPlatformInfo, null, temp182);
            this.Manager.Comment("reaching state \'S121\'");
            int temp195 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetPlatformChecker2)));
            if ((temp195 == 0)) {
                this.Manager.Comment("reaching state \'S314\'");
                bool temp183;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp183);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp183);
                this.Manager.Comment("reaching state \'S515\'");
                int temp186 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetClientAccountTypeChecker1)));
                if ((temp186 == 0)) {
                    this.Manager.Comment("reaching state \'S896\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp184;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,11,1)\'");
                    temp184 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 11u, 1u);
                    this.Manager.Comment("reaching state \'S1298\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp184, "return of NetrLogonControl, state S1298");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label52;
                }
                if ((temp186 == 1)) {
                    this.Manager.Comment("reaching state \'S897\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp185;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,12,1)\'");
                    temp185 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 12u, 1u);
                    this.Manager.Comment("reaching state \'S1299\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp185, "return of NetrLogonControl, state S1299");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label52;
                }
                throw new InvalidOperationException("never reached");
            label52:
;
                goto label55;
            }
            if ((temp195 == 1)) {
                this.Manager.Comment("reaching state \'S315\'");
                bool temp187;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp187);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp187);
                this.Manager.Comment("reaching state \'S516\'");
                int temp190 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetClientAccountTypeChecker3)));
                if ((temp190 == 0)) {
                    this.Manager.Comment("reaching state \'S898\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp188;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp188 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1300\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp188, "return of NetrLogonControl, state S1300");
                    this.Manager.Comment("reaching state \'S1520\'");
                    goto label53;
                }
                if ((temp190 == 1)) {
                    this.Manager.Comment("reaching state \'S899\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp189;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp189 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1301\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp189, "return of NetrLogonControl, state S1301");
                    this.Manager.Comment("reaching state \'S1521\'");
                    goto label53;
                }
                throw new InvalidOperationException("never reached");
            label53:
;
                goto label55;
            }
            if ((temp195 == 2)) {
                this.Manager.Comment("reaching state \'S316\'");
                bool temp191;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp191);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp191);
                this.Manager.Comment("reaching state \'S517\'");
                int temp194 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetClientAccountTypeChecker5)));
                if ((temp194 == 0)) {
                    this.Manager.Comment("reaching state \'S900\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp192;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp192 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1302\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp192, "return of NetrLogonControl, state S1302");
                    this.Manager.Comment("reaching state \'S1522\'");
                    goto label54;
                }
                if ((temp194 == 1)) {
                    this.Manager.Comment("reaching state \'S901\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp193;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp193 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1303\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp193, "return of NetrLogonControl, state S1303");
                    this.Manager.Comment("reaching state \'S1523\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S121");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S515");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S515");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S121");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S516");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S516");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S121");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S517");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS120GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S517");
        }
        #endregion
        
        #region Test Starting in S122
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122");
            this.Manager.Comment("reaching state \'S122\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp196;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp196);
            this.Manager.AddReturn(GetPlatformInfo, null, temp196);
            this.Manager.Comment("reaching state \'S123\'");
            int temp209 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetPlatformChecker2)));
            if ((temp209 == 0)) {
                this.Manager.Comment("reaching state \'S317\'");
                bool temp197;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp197);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp197);
                this.Manager.Comment("reaching state \'S518\'");
                int temp200 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetClientAccountTypeChecker1)));
                if ((temp200 == 0)) {
                    this.Manager.Comment("reaching state \'S902\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp198;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,12,1)\'");
                    temp198 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 12u, 1u);
                    this.Manager.Comment("reaching state \'S1304\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp198, "return of NetrLogonControl, state S1304");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label56;
                }
                if ((temp200 == 1)) {
                    this.Manager.Comment("reaching state \'S903\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp199;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,8,1)\'");
                    temp199 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8u, 1u);
                    this.Manager.Comment("reaching state \'S1305\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp199, "return of NetrLogonControl, state S1305");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label56;
                }
                throw new InvalidOperationException("never reached");
            label56:
;
                goto label59;
            }
            if ((temp209 == 1)) {
                this.Manager.Comment("reaching state \'S318\'");
                bool temp201;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp201);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp201);
                this.Manager.Comment("reaching state \'S519\'");
                int temp204 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetClientAccountTypeChecker3)));
                if ((temp204 == 0)) {
                    this.Manager.Comment("reaching state \'S904\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp202;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp202 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1306\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp202, "return of NetrLogonControl, state S1306");
                    this.Manager.Comment("reaching state \'S1524\'");
                    goto label57;
                }
                if ((temp204 == 1)) {
                    this.Manager.Comment("reaching state \'S905\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp203;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp203 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1307\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp203, "return of NetrLogonControl, state S1307");
                    this.Manager.Comment("reaching state \'S1525\'");
                    goto label57;
                }
                throw new InvalidOperationException("never reached");
            label57:
;
                goto label59;
            }
            if ((temp209 == 2)) {
                this.Manager.Comment("reaching state \'S319\'");
                bool temp205;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp205);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp205);
                this.Manager.Comment("reaching state \'S520\'");
                int temp208 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetClientAccountTypeChecker5)));
                if ((temp208 == 0)) {
                    this.Manager.Comment("reaching state \'S906\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp206;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp206 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1308\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp206, "return of NetrLogonControl, state S1308");
                    this.Manager.Comment("reaching state \'S1526\'");
                    goto label58;
                }
                if ((temp208 == 1)) {
                    this.Manager.Comment("reaching state \'S907\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp207;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp207 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1309\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp207, "return of NetrLogonControl, state S1309");
                    this.Manager.Comment("reaching state \'S1527\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S123");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S518");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S518");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S123");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S519");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S519");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S123");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S520");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS122GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S520");
        }
        #endregion
        
        #region Test Starting in S124
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124");
            this.Manager.Comment("reaching state \'S124\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp210;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp210);
            this.Manager.AddReturn(GetPlatformInfo, null, temp210);
            this.Manager.Comment("reaching state \'S125\'");
            int temp223 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetPlatformChecker2)));
            if ((temp223 == 0)) {
                this.Manager.Comment("reaching state \'S320\'");
                bool temp211;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp211);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp211);
                this.Manager.Comment("reaching state \'S521\'");
                int temp214 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetClientAccountTypeChecker1)));
                if ((temp214 == 0)) {
                    this.Manager.Comment("reaching state \'S908\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp212;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65532,1)\'");
                    temp212 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65532u, 1u);
                    this.Manager.Comment("reaching state \'S1310\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp212, "return of NetrLogonControl, state S1310");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label60;
                }
                if ((temp214 == 1)) {
                    this.Manager.Comment("reaching state \'S909\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp213;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65534,1)\'");
                    temp213 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65534u, 1u);
                    this.Manager.Comment("reaching state \'S1311\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp213, "return of NetrLogonControl, state S1311");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label60;
                }
                throw new InvalidOperationException("never reached");
            label60:
;
                goto label63;
            }
            if ((temp223 == 1)) {
                this.Manager.Comment("reaching state \'S321\'");
                bool temp215;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp215);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp215);
                this.Manager.Comment("reaching state \'S522\'");
                int temp218 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetClientAccountTypeChecker3)));
                if ((temp218 == 0)) {
                    this.Manager.Comment("reaching state \'S910\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp216;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp216 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1312\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp216, "return of NetrLogonControl, state S1312");
                    this.Manager.Comment("reaching state \'S1528\'");
                    goto label61;
                }
                if ((temp218 == 1)) {
                    this.Manager.Comment("reaching state \'S911\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp217;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp217 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1313\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp217, "return of NetrLogonControl, state S1313");
                    this.Manager.Comment("reaching state \'S1529\'");
                    goto label61;
                }
                throw new InvalidOperationException("never reached");
            label61:
;
                goto label63;
            }
            if ((temp223 == 2)) {
                this.Manager.Comment("reaching state \'S322\'");
                bool temp219;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp219);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp219);
                this.Manager.Comment("reaching state \'S523\'");
                int temp222 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetClientAccountTypeChecker5)));
                if ((temp222 == 0)) {
                    this.Manager.Comment("reaching state \'S912\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp220;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp220 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1314\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp220, "return of NetrLogonControl, state S1314");
                    this.Manager.Comment("reaching state \'S1530\'");
                    goto label62;
                }
                if ((temp222 == 1)) {
                    this.Manager.Comment("reaching state \'S913\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp221;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp221 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1315\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp221, "return of NetrLogonControl, state S1315");
                    this.Manager.Comment("reaching state \'S1531\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S125");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S521");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S521");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S125");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S522");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S522");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S125");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S523");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS124GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S523");
        }
        #endregion
        
        #region Test Starting in S126
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126");
            this.Manager.Comment("reaching state \'S126\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp224;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp224);
            this.Manager.AddReturn(GetPlatformInfo, null, temp224);
            this.Manager.Comment("reaching state \'S127\'");
            int temp237 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetPlatformChecker2)));
            if ((temp237 == 0)) {
                this.Manager.Comment("reaching state \'S323\'");
                bool temp225;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp225);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp225);
                this.Manager.Comment("reaching state \'S524\'");
                int temp228 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetClientAccountTypeChecker1)));
                if ((temp228 == 0)) {
                    this.Manager.Comment("reaching state \'S914\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp226;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65533,1)\'");
                    temp226 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65533u, 1u);
                    this.Manager.Comment("reaching state \'S1316\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp226, "return of NetrLogonControl, state S1316");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label64;
                }
                if ((temp228 == 1)) {
                    this.Manager.Comment("reaching state \'S915\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp227;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65533,1)\'");
                    temp227 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1317\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp227, "return of NetrLogonControl, state S1317");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label64;
                }
                throw new InvalidOperationException("never reached");
            label64:
;
                goto label67;
            }
            if ((temp237 == 1)) {
                this.Manager.Comment("reaching state \'S324\'");
                bool temp229;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp229);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp229);
                this.Manager.Comment("reaching state \'S525\'");
                int temp232 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetClientAccountTypeChecker3)));
                if ((temp232 == 0)) {
                    this.Manager.Comment("reaching state \'S916\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp230;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp230 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1318\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp230, "return of NetrLogonControl, state S1318");
                    this.Manager.Comment("reaching state \'S1532\'");
                    goto label65;
                }
                if ((temp232 == 1)) {
                    this.Manager.Comment("reaching state \'S917\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp231;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp231 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1319\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp231, "return of NetrLogonControl, state S1319");
                    this.Manager.Comment("reaching state \'S1533\'");
                    goto label65;
                }
                throw new InvalidOperationException("never reached");
            label65:
;
                goto label67;
            }
            if ((temp237 == 2)) {
                this.Manager.Comment("reaching state \'S325\'");
                bool temp233;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp233);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp233);
                this.Manager.Comment("reaching state \'S526\'");
                int temp236 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetClientAccountTypeChecker5)));
                if ((temp236 == 0)) {
                    this.Manager.Comment("reaching state \'S918\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp234;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp234 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1320\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp234, "return of NetrLogonControl, state S1320");
                    this.Manager.Comment("reaching state \'S1534\'");
                    goto label66;
                }
                if ((temp236 == 1)) {
                    this.Manager.Comment("reaching state \'S919\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp235;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp235 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1321\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp235, "return of NetrLogonControl, state S1321");
                    this.Manager.Comment("reaching state \'S1535\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S127");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S524");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S524");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S127");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S525");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S525");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S127");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S526");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS126GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S526");
        }
        #endregion
        
        #region Test Starting in S128
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128");
            this.Manager.Comment("reaching state \'S128\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp238;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp238);
            this.Manager.AddReturn(GetPlatformInfo, null, temp238);
            this.Manager.Comment("reaching state \'S129\'");
            int temp251 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetPlatformChecker2)));
            if ((temp251 == 0)) {
                this.Manager.Comment("reaching state \'S326\'");
                bool temp239;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp239);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp239);
                this.Manager.Comment("reaching state \'S527\'");
                int temp242 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetClientAccountTypeChecker1)));
                if ((temp242 == 0)) {
                    this.Manager.Comment("reaching state \'S920\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp240;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65534,1)\'");
                    temp240 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65534u, 1u);
                    this.Manager.Comment("reaching state \'S1322\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp240, "return of NetrLogonControl, state S1322");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label68;
                }
                if ((temp242 == 1)) {
                    this.Manager.Comment("reaching state \'S921\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp241;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,11,1)\'");
                    temp241 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 11u, 1u);
                    this.Manager.Comment("reaching state \'S1323\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp241, "return of NetrLogonControl, state S1323");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label68;
                }
                throw new InvalidOperationException("never reached");
            label68:
;
                goto label71;
            }
            if ((temp251 == 1)) {
                this.Manager.Comment("reaching state \'S327\'");
                bool temp243;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp243);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp243);
                this.Manager.Comment("reaching state \'S528\'");
                int temp246 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetClientAccountTypeChecker3)));
                if ((temp246 == 0)) {
                    this.Manager.Comment("reaching state \'S922\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp244;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp244 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1324\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp244, "return of NetrLogonControl, state S1324");
                    this.Manager.Comment("reaching state \'S1536\'");
                    goto label69;
                }
                if ((temp246 == 1)) {
                    this.Manager.Comment("reaching state \'S923\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp245;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp245 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1325\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp245, "return of NetrLogonControl, state S1325");
                    this.Manager.Comment("reaching state \'S1537\'");
                    goto label69;
                }
                throw new InvalidOperationException("never reached");
            label69:
;
                goto label71;
            }
            if ((temp251 == 2)) {
                this.Manager.Comment("reaching state \'S328\'");
                bool temp247;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp247);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp247);
                this.Manager.Comment("reaching state \'S529\'");
                int temp250 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetClientAccountTypeChecker5)));
                if ((temp250 == 0)) {
                    this.Manager.Comment("reaching state \'S924\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp248;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp248 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1326\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp248, "return of NetrLogonControl, state S1326");
                    this.Manager.Comment("reaching state \'S1538\'");
                    goto label70;
                }
                if ((temp250 == 1)) {
                    this.Manager.Comment("reaching state \'S925\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp249;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp249 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1327\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp249, "return of NetrLogonControl, state S1327");
                    this.Manager.Comment("reaching state \'S1539\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S129");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S527");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S527");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S129");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S528");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S528");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S129");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S529");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS128GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S529");
        }
        #endregion
        
        #region Test Starting in S130
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130");
            this.Manager.Comment("reaching state \'S130\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp252;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp252);
            this.Manager.AddReturn(GetPlatformInfo, null, temp252);
            this.Manager.Comment("reaching state \'S131\'");
            int temp265 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetPlatformChecker2)));
            if ((temp265 == 0)) {
                this.Manager.Comment("reaching state \'S329\'");
                bool temp253;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp253);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp253);
                this.Manager.Comment("reaching state \'S530\'");
                int temp256 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetClientAccountTypeChecker1)));
                if ((temp256 == 0)) {
                    this.Manager.Comment("reaching state \'S926\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp254;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65535,1)\'");
                    temp254 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65535u, 1u);
                    this.Manager.Comment("reaching state \'S1328\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp254, "return of NetrLogonControl, state S1328");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label72;
                }
                if ((temp256 == 1)) {
                    this.Manager.Comment("reaching state \'S927\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp255;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,9,1)\'");
                    temp255 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 9u, 1u);
                    this.Manager.Comment("reaching state \'S1329\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp255, "return of NetrLogonControl, state S1329");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label72;
                }
                throw new InvalidOperationException("never reached");
            label72:
;
                goto label75;
            }
            if ((temp265 == 1)) {
                this.Manager.Comment("reaching state \'S330\'");
                bool temp257;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp257);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp257);
                this.Manager.Comment("reaching state \'S531\'");
                int temp260 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetClientAccountTypeChecker3)));
                if ((temp260 == 0)) {
                    this.Manager.Comment("reaching state \'S928\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp258;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp258 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1330\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp258, "return of NetrLogonControl, state S1330");
                    this.Manager.Comment("reaching state \'S1540\'");
                    goto label73;
                }
                if ((temp260 == 1)) {
                    this.Manager.Comment("reaching state \'S929\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp259;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp259 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1331\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp259, "return of NetrLogonControl, state S1331");
                    this.Manager.Comment("reaching state \'S1541\'");
                    goto label73;
                }
                throw new InvalidOperationException("never reached");
            label73:
;
                goto label75;
            }
            if ((temp265 == 2)) {
                this.Manager.Comment("reaching state \'S331\'");
                bool temp261;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp261);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp261);
                this.Manager.Comment("reaching state \'S532\'");
                int temp264 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetClientAccountTypeChecker5)));
                if ((temp264 == 0)) {
                    this.Manager.Comment("reaching state \'S930\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp262;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp262 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1332\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp262, "return of NetrLogonControl, state S1332");
                    this.Manager.Comment("reaching state \'S1542\'");
                    goto label74;
                }
                if ((temp264 == 1)) {
                    this.Manager.Comment("reaching state \'S931\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp263;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp263 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1333\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp263, "return of NetrLogonControl, state S1333");
                    this.Manager.Comment("reaching state \'S1543\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S131");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S530");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S530");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S131");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S531");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S531");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S131");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S532");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS130GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S532");
        }
        #endregion
        
        #region Test Starting in S132
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132");
            this.Manager.Comment("reaching state \'S132\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp266;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp266);
            this.Manager.AddReturn(GetPlatformInfo, null, temp266);
            this.Manager.Comment("reaching state \'S133\'");
            int temp279 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetPlatformChecker2)));
            if ((temp279 == 0)) {
                this.Manager.Comment("reaching state \'S332\'");
                bool temp267;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp267);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp267);
                this.Manager.Comment("reaching state \'S533\'");
                int temp270 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetClientAccountTypeChecker1)));
                if ((temp270 == 0)) {
                    this.Manager.Comment("reaching state \'S932\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp268;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,2)\'");
                    temp268 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 2u);
                    this.Manager.Comment("reaching state \'S1334\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp268, "return of NetrLogonControl, state S1334");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label76;
                }
                if ((temp270 == 1)) {
                    this.Manager.Comment("reaching state \'S933\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp269;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,2)\'");
                    temp269 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 2u);
                    this.Manager.Checkpoint("MS-NRPC_R104020");
                    this.Manager.Comment("reaching state \'S1335\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp269, "return of NetrLogonControl, state S1335");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label76;
                }
                throw new InvalidOperationException("never reached");
            label76:
;
                goto label79;
            }
            if ((temp279 == 1)) {
                this.Manager.Comment("reaching state \'S333\'");
                bool temp271;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp271);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp271);
                this.Manager.Comment("reaching state \'S534\'");
                int temp274 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetClientAccountTypeChecker3)));
                if ((temp274 == 0)) {
                    this.Manager.Comment("reaching state \'S934\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp272;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp272 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1336\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp272, "return of NetrLogonControl, state S1336");
                    this.Manager.Comment("reaching state \'S1544\'");
                    goto label77;
                }
                if ((temp274 == 1)) {
                    this.Manager.Comment("reaching state \'S935\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp273;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp273 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1337\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp273, "return of NetrLogonControl, state S1337");
                    this.Manager.Comment("reaching state \'S1545\'");
                    goto label77;
                }
                throw new InvalidOperationException("never reached");
            label77:
;
                goto label79;
            }
            if ((temp279 == 2)) {
                this.Manager.Comment("reaching state \'S334\'");
                bool temp275;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp275);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp275);
                this.Manager.Comment("reaching state \'S535\'");
                int temp278 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetClientAccountTypeChecker5)));
                if ((temp278 == 0)) {
                    this.Manager.Comment("reaching state \'S936\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp276;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp276 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1338\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp276, "return of NetrLogonControl, state S1338");
                    this.Manager.Comment("reaching state \'S1546\'");
                    goto label78;
                }
                if ((temp278 == 1)) {
                    this.Manager.Comment("reaching state \'S937\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp277;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp277 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1339\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp277, "return of NetrLogonControl, state S1339");
                    this.Manager.Comment("reaching state \'S1547\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S133");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S533");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S533");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S133");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S534");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S534");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S133");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S535");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS132GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S535");
        }
        #endregion
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp280;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp280);
            this.Manager.AddReturn(GetPlatformInfo, null, temp280);
            this.Manager.Comment("reaching state \'S15\'");
            int temp293 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetPlatformChecker2)));
            if ((temp293 == 0)) {
                this.Manager.Comment("reaching state \'S155\'");
                bool temp281;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp281);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp281);
                this.Manager.Comment("reaching state \'S356\'");
                int temp284 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetClientAccountTypeChecker1)));
                if ((temp284 == 0)) {
                    this.Manager.Comment("reaching state \'S578\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp282;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,7,1)\'");
                    temp282 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 7u, 1u);
                    this.Manager.Comment("reaching state \'S980\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp282, "return of NetrLogonControl, state S980");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label80;
                }
                if ((temp284 == 1)) {
                    this.Manager.Comment("reaching state \'S579\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp283;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp283 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S981\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp283, "return of NetrLogonControl, state S981");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label80;
                }
                throw new InvalidOperationException("never reached");
            label80:
;
                goto label83;
            }
            if ((temp293 == 1)) {
                this.Manager.Comment("reaching state \'S156\'");
                bool temp285;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp285);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp285);
                this.Manager.Comment("reaching state \'S357\'");
                int temp288 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetClientAccountTypeChecker3)));
                if ((temp288 == 0)) {
                    this.Manager.Comment("reaching state \'S580\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp286;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,7,1)\'");
                    temp286 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 7u, 1u);
                    this.Manager.Comment("reaching state \'S982\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp286, "return of NetrLogonControl, state S982");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label81;
                }
                if ((temp288 == 1)) {
                    this.Manager.Comment("reaching state \'S581\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp287;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,10,1)\'");
                    temp287 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 10u, 1u);
                    this.Manager.Comment("reaching state \'S983\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp287, "return of NetrLogonControl, state S983");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label81;
                }
                throw new InvalidOperationException("never reached");
            label81:
;
                goto label83;
            }
            if ((temp293 == 2)) {
                this.Manager.Comment("reaching state \'S157\'");
                bool temp289;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp289);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp289);
                this.Manager.Comment("reaching state \'S358\'");
                int temp292 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetClientAccountTypeChecker5)));
                if ((temp292 == 0)) {
                    this.Manager.Comment("reaching state \'S582\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp290;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp290 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S984\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp290, "return of NetrLogonControl, state S984");
                    this.Manager.Comment("reaching state \'S1358\'");
                    goto label82;
                }
                if ((temp292 == 1)) {
                    this.Manager.Comment("reaching state \'S583\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp291;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp291 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S985\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp291, "return of NetrLogonControl, state S985");
                    this.Manager.Comment("reaching state \'S1359\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S356");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S356");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S357");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S357");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S358");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS14GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S358");
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp294;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp294);
            this.Manager.AddReturn(GetPlatformInfo, null, temp294);
            this.Manager.Comment("reaching state \'S17\'");
            int temp307 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetPlatformChecker2)));
            if ((temp307 == 0)) {
                this.Manager.Comment("reaching state \'S158\'");
                bool temp295;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp295);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp295);
                this.Manager.Comment("reaching state \'S359\'");
                int temp298 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetClientAccountTypeChecker1)));
                if ((temp298 == 0)) {
                    this.Manager.Comment("reaching state \'S584\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp296;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,8,1)\'");
                    temp296 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 8u, 1u);
                    this.Manager.Comment("reaching state \'S986\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp296, "return of NetrLogonControl, state S986");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label84;
                }
                if ((temp298 == 1)) {
                    this.Manager.Comment("reaching state \'S585\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp297;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp297 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S987\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp297, "return of NetrLogonControl, state S987");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label84;
                }
                throw new InvalidOperationException("never reached");
            label84:
;
                goto label87;
            }
            if ((temp307 == 1)) {
                this.Manager.Comment("reaching state \'S159\'");
                bool temp299;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp299);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp299);
                this.Manager.Comment("reaching state \'S360\'");
                int temp302 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetClientAccountTypeChecker3)));
                if ((temp302 == 0)) {
                    this.Manager.Comment("reaching state \'S586\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp300;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,8,1)\'");
                    temp300 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 8u, 1u);
                    this.Manager.Comment("reaching state \'S988\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp300, "return of NetrLogonControl, state S988");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label85;
                }
                if ((temp302 == 1)) {
                    this.Manager.Comment("reaching state \'S587\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp301;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,8,1)\'");
                    temp301 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 8u, 1u);
                    this.Manager.Comment("reaching state \'S989\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp301, "return of NetrLogonControl, state S989");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label85;
                }
                throw new InvalidOperationException("never reached");
            label85:
;
                goto label87;
            }
            if ((temp307 == 2)) {
                this.Manager.Comment("reaching state \'S160\'");
                bool temp303;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp303);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp303);
                this.Manager.Comment("reaching state \'S361\'");
                int temp306 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetClientAccountTypeChecker5)));
                if ((temp306 == 0)) {
                    this.Manager.Comment("reaching state \'S588\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp304;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp304 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S990\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp304, "return of NetrLogonControl, state S990");
                    this.Manager.Comment("reaching state \'S1360\'");
                    goto label86;
                }
                if ((temp306 == 1)) {
                    this.Manager.Comment("reaching state \'S589\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp305;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp305 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S991\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp305, "return of NetrLogonControl, state S991");
                    this.Manager.Comment("reaching state \'S1361\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S359");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S359");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S360");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S360");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S361");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS16GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S361");
        }
        #endregion
        
        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp308;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp308);
            this.Manager.AddReturn(GetPlatformInfo, null, temp308);
            this.Manager.Comment("reaching state \'S19\'");
            int temp321 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetPlatformChecker2)));
            if ((temp321 == 0)) {
                this.Manager.Comment("reaching state \'S161\'");
                bool temp309;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp309);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp309);
                this.Manager.Comment("reaching state \'S362\'");
                int temp312 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetClientAccountTypeChecker1)));
                if ((temp312 == 0)) {
                    this.Manager.Comment("reaching state \'S590\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp310;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,9,1)\'");
                    temp310 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 9u, 1u);
                    this.Manager.Comment("reaching state \'S992\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp310, "return of NetrLogonControl, state S992");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label88;
                }
                if ((temp312 == 1)) {
                    this.Manager.Comment("reaching state \'S591\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp311;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp311 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S993\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp311, "return of NetrLogonControl, state S993");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label88;
                }
                throw new InvalidOperationException("never reached");
            label88:
;
                goto label91;
            }
            if ((temp321 == 1)) {
                this.Manager.Comment("reaching state \'S162\'");
                bool temp313;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp313);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp313);
                this.Manager.Comment("reaching state \'S363\'");
                int temp316 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetClientAccountTypeChecker3)));
                if ((temp316 == 0)) {
                    this.Manager.Comment("reaching state \'S592\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp314;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,9,1)\'");
                    temp314 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 9u, 1u);
                    this.Manager.Comment("reaching state \'S994\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp314, "return of NetrLogonControl, state S994");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label89;
                }
                if ((temp316 == 1)) {
                    this.Manager.Comment("reaching state \'S593\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp315;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,12,1)\'");
                    temp315 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 12u, 1u);
                    this.Manager.Comment("reaching state \'S995\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp315, "return of NetrLogonControl, state S995");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label89;
                }
                throw new InvalidOperationException("never reached");
            label89:
;
                goto label91;
            }
            if ((temp321 == 2)) {
                this.Manager.Comment("reaching state \'S163\'");
                bool temp317;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp317);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp317);
                this.Manager.Comment("reaching state \'S364\'");
                int temp320 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetClientAccountTypeChecker5)));
                if ((temp320 == 0)) {
                    this.Manager.Comment("reaching state \'S594\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp318;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp318 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S996\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp318, "return of NetrLogonControl, state S996");
                    this.Manager.Comment("reaching state \'S1362\'");
                    goto label90;
                }
                if ((temp320 == 1)) {
                    this.Manager.Comment("reaching state \'S595\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp319;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp319 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S997\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp319, "return of NetrLogonControl, state S997");
                    this.Manager.Comment("reaching state \'S1363\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S362");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S362");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S363");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S363");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S364");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS18GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S364");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp322;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp322);
            this.Manager.AddReturn(GetPlatformInfo, null, temp322);
            this.Manager.Comment("reaching state \'S3\'");
            int temp335 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetPlatformChecker2)));
            if ((temp335 == 0)) {
                this.Manager.Comment("reaching state \'S137\'");
                bool temp323;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp323);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp323);
                this.Manager.Comment("reaching state \'S338\'");
                int temp326 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetClientAccountTypeChecker1)));
                if ((temp326 == 0)) {
                    this.Manager.Comment("reaching state \'S542\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp324;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp324 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S944\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp324, "return of NetrLogonControl, state S944");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label92;
                }
                if ((temp326 == 1)) {
                    this.Manager.Comment("reaching state \'S543\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp325;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp325 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S945\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp325, "return of NetrLogonControl, state S945");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label92;
                }
                throw new InvalidOperationException("never reached");
            label92:
;
                goto label95;
            }
            if ((temp335 == 1)) {
                this.Manager.Comment("reaching state \'S138\'");
                bool temp327;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp327);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp327);
                this.Manager.Comment("reaching state \'S339\'");
                int temp330 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetClientAccountTypeChecker3)));
                if ((temp330 == 0)) {
                    this.Manager.Comment("reaching state \'S544\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp328;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp328 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S946\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp328, "return of NetrLogonControl, state S946");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label93;
                }
                if ((temp330 == 1)) {
                    this.Manager.Comment("reaching state \'S545\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp329;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp329 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S947\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp329, "return of NetrLogonControl, state S947");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label93;
                }
                throw new InvalidOperationException("never reached");
            label93:
;
                goto label95;
            }
            if ((temp335 == 2)) {
                this.Manager.Comment("reaching state \'S139\'");
                bool temp331;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp331);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp331);
                this.Manager.Comment("reaching state \'S340\'");
                int temp334 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetClientAccountTypeChecker5)));
                if ((temp334 == 0)) {
                    this.Manager.Comment("reaching state \'S546\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp332;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp332 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S948\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp332, "return of NetrLogonControl, state S948");
                    this.Manager.Comment("reaching state \'S1346\'");
                    goto label94;
                }
                if ((temp334 == 1)) {
                    this.Manager.Comment("reaching state \'S547\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp333;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp333 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S949\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp333, "return of NetrLogonControl, state S949");
                    this.Manager.Comment("reaching state \'S1347\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S338");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S338");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S339");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S339");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S340");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS2GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S340");
        }
        #endregion
        
        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp336;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp336);
            this.Manager.AddReturn(GetPlatformInfo, null, temp336);
            this.Manager.Comment("reaching state \'S21\'");
            int temp349 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetPlatformChecker2)));
            if ((temp349 == 0)) {
                this.Manager.Comment("reaching state \'S164\'");
                bool temp337;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp337);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp337);
                this.Manager.Comment("reaching state \'S365\'");
                int temp340 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetClientAccountTypeChecker1)));
                if ((temp340 == 0)) {
                    this.Manager.Comment("reaching state \'S596\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp338;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,10,1)\'");
                    temp338 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 10u, 1u);
                    this.Manager.Comment("reaching state \'S998\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp338, "return of NetrLogonControl, state S998");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label96;
                }
                if ((temp340 == 1)) {
                    this.Manager.Comment("reaching state \'S597\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp339;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,11,1)\'");
                    temp339 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 11u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S999\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp339, "return of NetrLogonControl, state S999");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label96;
                }
                throw new InvalidOperationException("never reached");
            label96:
;
                goto label99;
            }
            if ((temp349 == 1)) {
                this.Manager.Comment("reaching state \'S165\'");
                bool temp341;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp341);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp341);
                this.Manager.Comment("reaching state \'S366\'");
                int temp344 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetClientAccountTypeChecker3)));
                if ((temp344 == 0)) {
                    this.Manager.Comment("reaching state \'S598\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp342;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,10,1)\'");
                    temp342 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 10u, 1u);
                    this.Manager.Comment("reaching state \'S1000\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp342, "return of NetrLogonControl, state S1000");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label97;
                }
                if ((temp344 == 1)) {
                    this.Manager.Comment("reaching state \'S599\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp343;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,4,1)\'");
                    temp343 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 4u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1001\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp343, "return of NetrLogonControl, state S1001");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label97;
                }
                throw new InvalidOperationException("never reached");
            label97:
;
                goto label99;
            }
            if ((temp349 == 2)) {
                this.Manager.Comment("reaching state \'S166\'");
                bool temp345;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp345);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp345);
                this.Manager.Comment("reaching state \'S367\'");
                int temp348 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetClientAccountTypeChecker5)));
                if ((temp348 == 0)) {
                    this.Manager.Comment("reaching state \'S600\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp346;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp346 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1002\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp346, "return of NetrLogonControl, state S1002");
                    this.Manager.Comment("reaching state \'S1364\'");
                    goto label98;
                }
                if ((temp348 == 1)) {
                    this.Manager.Comment("reaching state \'S601\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp347;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp347 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1003\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp347, "return of NetrLogonControl, state S1003");
                    this.Manager.Comment("reaching state \'S1365\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S365");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S365");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S366");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S366");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S367");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS20GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S367");
        }
        #endregion
        
        #region Test Starting in S22
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp350;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp350);
            this.Manager.AddReturn(GetPlatformInfo, null, temp350);
            this.Manager.Comment("reaching state \'S23\'");
            int temp363 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetPlatformChecker2)));
            if ((temp363 == 0)) {
                this.Manager.Comment("reaching state \'S167\'");
                bool temp351;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp351);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp351);
                this.Manager.Comment("reaching state \'S368\'");
                int temp354 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetClientAccountTypeChecker1)));
                if ((temp354 == 0)) {
                    this.Manager.Comment("reaching state \'S602\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp352;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,11,1)\'");
                    temp352 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 11u, 1u);
                    this.Manager.Comment("reaching state \'S1004\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp352, "return of NetrLogonControl, state S1004");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label100;
                }
                if ((temp354 == 1)) {
                    this.Manager.Comment("reaching state \'S603\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp353;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,3,1)\'");
                    temp353 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 3u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1005\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp353, "return of NetrLogonControl, state S1005");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label100;
                }
                throw new InvalidOperationException("never reached");
            label100:
;
                goto label103;
            }
            if ((temp363 == 1)) {
                this.Manager.Comment("reaching state \'S168\'");
                bool temp355;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp355);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp355);
                this.Manager.Comment("reaching state \'S369\'");
                int temp358 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetClientAccountTypeChecker3)));
                if ((temp358 == 0)) {
                    this.Manager.Comment("reaching state \'S604\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp356;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,11,1)\'");
                    temp356 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 11u, 1u);
                    this.Manager.Comment("reaching state \'S1006\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp356, "return of NetrLogonControl, state S1006");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label101;
                }
                if ((temp358 == 1)) {
                    this.Manager.Comment("reaching state \'S605\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp357;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65532,1)\'");
                    temp357 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65532u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1007\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp357, "return of NetrLogonControl, state S1007");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label101;
                }
                throw new InvalidOperationException("never reached");
            label101:
;
                goto label103;
            }
            if ((temp363 == 2)) {
                this.Manager.Comment("reaching state \'S169\'");
                bool temp359;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp359);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp359);
                this.Manager.Comment("reaching state \'S370\'");
                int temp362 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetClientAccountTypeChecker5)));
                if ((temp362 == 0)) {
                    this.Manager.Comment("reaching state \'S606\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp360;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp360 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1008\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp360, "return of NetrLogonControl, state S1008");
                    this.Manager.Comment("reaching state \'S1366\'");
                    goto label102;
                }
                if ((temp362 == 1)) {
                    this.Manager.Comment("reaching state \'S607\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp361;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp361 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1009\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp361, "return of NetrLogonControl, state S1009");
                    this.Manager.Comment("reaching state \'S1367\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S368");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S368");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S369");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S369");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S370");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS22GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S370");
        }
        #endregion
        
        #region Test Starting in S24
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp364;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp364);
            this.Manager.AddReturn(GetPlatformInfo, null, temp364);
            this.Manager.Comment("reaching state \'S25\'");
            int temp377 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetPlatformChecker2)));
            if ((temp377 == 0)) {
                this.Manager.Comment("reaching state \'S170\'");
                bool temp365;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp365);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp365);
                this.Manager.Comment("reaching state \'S371\'");
                int temp368 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetClientAccountTypeChecker1)));
                if ((temp368 == 0)) {
                    this.Manager.Comment("reaching state \'S608\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp366;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,12,1)\'");
                    temp366 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 12u, 1u);
                    this.Manager.Comment("reaching state \'S1010\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp366, "return of NetrLogonControl, state S1010");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label104;
                }
                if ((temp368 == 1)) {
                    this.Manager.Comment("reaching state \'S609\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp367;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,7,1)\'");
                    temp367 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 7u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1011\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp367, "return of NetrLogonControl, state S1011");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label104;
                }
                throw new InvalidOperationException("never reached");
            label104:
;
                goto label107;
            }
            if ((temp377 == 1)) {
                this.Manager.Comment("reaching state \'S171\'");
                bool temp369;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp369);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp369);
                this.Manager.Comment("reaching state \'S372\'");
                int temp372 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetClientAccountTypeChecker3)));
                if ((temp372 == 0)) {
                    this.Manager.Comment("reaching state \'S610\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp370;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,12,1)\'");
                    temp370 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 12u, 1u);
                    this.Manager.Comment("reaching state \'S1012\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp370, "return of NetrLogonControl, state S1012");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label105;
                }
                if ((temp372 == 1)) {
                    this.Manager.Comment("reaching state \'S611\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp371;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,9,1)\'");
                    temp371 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 9u, 1u);
                    this.Manager.Comment("reaching state \'S1013\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp371, "return of NetrLogonControl, state S1013");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label105;
                }
                throw new InvalidOperationException("never reached");
            label105:
;
                goto label107;
            }
            if ((temp377 == 2)) {
                this.Manager.Comment("reaching state \'S172\'");
                bool temp373;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp373);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp373);
                this.Manager.Comment("reaching state \'S373\'");
                int temp376 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetClientAccountTypeChecker5)));
                if ((temp376 == 0)) {
                    this.Manager.Comment("reaching state \'S612\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp374;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp374 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1014\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp374, "return of NetrLogonControl, state S1014");
                    this.Manager.Comment("reaching state \'S1368\'");
                    goto label106;
                }
                if ((temp376 == 1)) {
                    this.Manager.Comment("reaching state \'S613\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp375;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp375 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1015\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp375, "return of NetrLogonControl, state S1015");
                    this.Manager.Comment("reaching state \'S1369\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S371");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S371");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S372");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S372");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S373");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS24GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S373");
        }
        #endregion
        
        #region Test Starting in S26
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp378;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp378);
            this.Manager.AddReturn(GetPlatformInfo, null, temp378);
            this.Manager.Comment("reaching state \'S27\'");
            int temp391 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetPlatformChecker2)));
            if ((temp391 == 0)) {
                this.Manager.Comment("reaching state \'S173\'");
                bool temp379;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp379);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp379);
                this.Manager.Comment("reaching state \'S374\'");
                int temp382 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetClientAccountTypeChecker1)));
                if ((temp382 == 0)) {
                    this.Manager.Comment("reaching state \'S614\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp380;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65532,1)\'");
                    temp380 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65532u, 1u);
                    this.Manager.Comment("reaching state \'S1016\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp380, "return of NetrLogonControl, state S1016");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label108;
                }
                if ((temp382 == 1)) {
                    this.Manager.Comment("reaching state \'S615\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp381;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65534,1)\'");
                    temp381 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1017\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp381, "return of NetrLogonControl, state S1017");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label108;
                }
                throw new InvalidOperationException("never reached");
            label108:
;
                goto label111;
            }
            if ((temp391 == 1)) {
                this.Manager.Comment("reaching state \'S174\'");
                bool temp383;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp383);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp383);
                this.Manager.Comment("reaching state \'S375\'");
                int temp386 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetClientAccountTypeChecker3)));
                if ((temp386 == 0)) {
                    this.Manager.Comment("reaching state \'S616\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp384;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65532,1)\'");
                    temp384 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65532u, 1u);
                    this.Manager.Comment("reaching state \'S1018\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp384, "return of NetrLogonControl, state S1018");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label109;
                }
                if ((temp386 == 1)) {
                    this.Manager.Comment("reaching state \'S617\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp385;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,5,1)\'");
                    temp385 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 5u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104917");
                    this.Manager.Comment("reaching state \'S1019\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp385, "return of NetrLogonControl, state S1019");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label109;
                }
                throw new InvalidOperationException("never reached");
            label109:
;
                goto label111;
            }
            if ((temp391 == 2)) {
                this.Manager.Comment("reaching state \'S175\'");
                bool temp387;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp387);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp387);
                this.Manager.Comment("reaching state \'S376\'");
                int temp390 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetClientAccountTypeChecker5)));
                if ((temp390 == 0)) {
                    this.Manager.Comment("reaching state \'S618\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp388;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp388 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1020\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp388, "return of NetrLogonControl, state S1020");
                    this.Manager.Comment("reaching state \'S1370\'");
                    goto label110;
                }
                if ((temp390 == 1)) {
                    this.Manager.Comment("reaching state \'S619\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp389;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp389 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1021\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp389, "return of NetrLogonControl, state S1021");
                    this.Manager.Comment("reaching state \'S1371\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S374");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S374");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S375");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S375");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S376");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS26GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S376");
        }
        #endregion
        
        #region Test Starting in S28
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp392;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp392);
            this.Manager.AddReturn(GetPlatformInfo, null, temp392);
            this.Manager.Comment("reaching state \'S29\'");
            int temp405 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetPlatformChecker2)));
            if ((temp405 == 0)) {
                this.Manager.Comment("reaching state \'S176\'");
                bool temp393;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp393);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp393);
                this.Manager.Comment("reaching state \'S377\'");
                int temp396 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetClientAccountTypeChecker1)));
                if ((temp396 == 0)) {
                    this.Manager.Comment("reaching state \'S620\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp394;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp394 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Comment("reaching state \'S1022\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp394, "return of NetrLogonControl, state S1022");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label112;
                }
                if ((temp396 == 1)) {
                    this.Manager.Comment("reaching state \'S621\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp395;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,1)\'");
                    temp395 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1023\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp395, "return of NetrLogonControl, state S1023");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label112;
                }
                throw new InvalidOperationException("never reached");
            label112:
;
                goto label115;
            }
            if ((temp405 == 1)) {
                this.Manager.Comment("reaching state \'S177\'");
                bool temp397;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp397);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp397);
                this.Manager.Comment("reaching state \'S378\'");
                int temp400 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetClientAccountTypeChecker3)));
                if ((temp400 == 0)) {
                    this.Manager.Comment("reaching state \'S622\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp398;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp398 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Comment("reaching state \'S1024\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp398, "return of NetrLogonControl, state S1024");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label113;
                }
                if ((temp400 == 1)) {
                    this.Manager.Comment("reaching state \'S623\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp399;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,11,1)\'");
                    temp399 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 11u, 1u);
                    this.Manager.Comment("reaching state \'S1025\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp399, "return of NetrLogonControl, state S1025");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label113;
                }
                throw new InvalidOperationException("never reached");
            label113:
;
                goto label115;
            }
            if ((temp405 == 2)) {
                this.Manager.Comment("reaching state \'S178\'");
                bool temp401;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp401);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp401);
                this.Manager.Comment("reaching state \'S379\'");
                int temp404 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetClientAccountTypeChecker5)));
                if ((temp404 == 0)) {
                    this.Manager.Comment("reaching state \'S624\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp402;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp402 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1026\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp402, "return of NetrLogonControl, state S1026");
                    this.Manager.Comment("reaching state \'S1372\'");
                    goto label114;
                }
                if ((temp404 == 1)) {
                    this.Manager.Comment("reaching state \'S625\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp403;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp403 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1027\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp403, "return of NetrLogonControl, state S1027");
                    this.Manager.Comment("reaching state \'S1373\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S377");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S377");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S378");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S378");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S379");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS28GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S379");
        }
        #endregion
        
        #region Test Starting in S30
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp406;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp406);
            this.Manager.AddReturn(GetPlatformInfo, null, temp406);
            this.Manager.Comment("reaching state \'S31\'");
            int temp419 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetPlatformChecker2)));
            if ((temp419 == 0)) {
                this.Manager.Comment("reaching state \'S179\'");
                bool temp407;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp407);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp407);
                this.Manager.Comment("reaching state \'S380\'");
                int temp410 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetClientAccountTypeChecker1)));
                if ((temp410 == 0)) {
                    this.Manager.Comment("reaching state \'S626\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp408;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp408 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Comment("reaching state \'S1028\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp408, "return of NetrLogonControl, state S1028");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label116;
                }
                if ((temp410 == 1)) {
                    this.Manager.Comment("reaching state \'S627\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp409;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,2,1)\'");
                    temp409 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 2u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1029\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp409, "return of NetrLogonControl, state S1029");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label116;
                }
                throw new InvalidOperationException("never reached");
            label116:
;
                goto label119;
            }
            if ((temp419 == 1)) {
                this.Manager.Comment("reaching state \'S180\'");
                bool temp411;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp411);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp411);
                this.Manager.Comment("reaching state \'S381\'");
                int temp414 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetClientAccountTypeChecker3)));
                if ((temp414 == 0)) {
                    this.Manager.Comment("reaching state \'S628\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp412;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp412 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Comment("reaching state \'S1030\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp412, "return of NetrLogonControl, state S1030");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label117;
                }
                if ((temp414 == 1)) {
                    this.Manager.Comment("reaching state \'S629\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp413;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp413 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1031\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp413, "return of NetrLogonControl, state S1031");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label117;
                }
                throw new InvalidOperationException("never reached");
            label117:
;
                goto label119;
            }
            if ((temp419 == 2)) {
                this.Manager.Comment("reaching state \'S181\'");
                bool temp415;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp415);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp415);
                this.Manager.Comment("reaching state \'S382\'");
                int temp418 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetClientAccountTypeChecker5)));
                if ((temp418 == 0)) {
                    this.Manager.Comment("reaching state \'S630\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp416;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp416 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1032\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp416, "return of NetrLogonControl, state S1032");
                    this.Manager.Comment("reaching state \'S1374\'");
                    goto label118;
                }
                if ((temp418 == 1)) {
                    this.Manager.Comment("reaching state \'S631\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp417;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp417 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1033\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp417, "return of NetrLogonControl, state S1033");
                    this.Manager.Comment("reaching state \'S1375\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S380");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S380");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S381");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S381");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S382");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS30GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S382");
        }
        #endregion
        
        #region Test Starting in S32
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp420;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp420);
            this.Manager.AddReturn(GetPlatformInfo, null, temp420);
            this.Manager.Comment("reaching state \'S33\'");
            int temp433 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetPlatformChecker2)));
            if ((temp433 == 0)) {
                this.Manager.Comment("reaching state \'S182\'");
                bool temp421;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp421);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp421);
                this.Manager.Comment("reaching state \'S383\'");
                int temp424 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetClientAccountTypeChecker1)));
                if ((temp424 == 0)) {
                    this.Manager.Comment("reaching state \'S632\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp422;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp422 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Comment("reaching state \'S1034\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp422, "return of NetrLogonControl, state S1034");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label120;
                }
                if ((temp424 == 1)) {
                    this.Manager.Comment("reaching state \'S633\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp423;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,3,1)\'");
                    temp423 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 3u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1035\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp423, "return of NetrLogonControl, state S1035");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label120;
                }
                throw new InvalidOperationException("never reached");
            label120:
;
                goto label123;
            }
            if ((temp433 == 1)) {
                this.Manager.Comment("reaching state \'S183\'");
                bool temp425;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp425);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp425);
                this.Manager.Comment("reaching state \'S384\'");
                int temp428 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetClientAccountTypeChecker3)));
                if ((temp428 == 0)) {
                    this.Manager.Comment("reaching state \'S634\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp426;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp426 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Comment("reaching state \'S1036\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp426, "return of NetrLogonControl, state S1036");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label121;
                }
                if ((temp428 == 1)) {
                    this.Manager.Comment("reaching state \'S635\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp427;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,7,1)\'");
                    temp427 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 7u, 1u);
                    this.Manager.Comment("reaching state \'S1037\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp427, "return of NetrLogonControl, state S1037");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label121;
                }
                throw new InvalidOperationException("never reached");
            label121:
;
                goto label123;
            }
            if ((temp433 == 2)) {
                this.Manager.Comment("reaching state \'S184\'");
                bool temp429;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp429);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp429);
                this.Manager.Comment("reaching state \'S385\'");
                int temp432 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetClientAccountTypeChecker5)));
                if ((temp432 == 0)) {
                    this.Manager.Comment("reaching state \'S636\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp430;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp430 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1038\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp430, "return of NetrLogonControl, state S1038");
                    this.Manager.Comment("reaching state \'S1376\'");
                    goto label122;
                }
                if ((temp432 == 1)) {
                    this.Manager.Comment("reaching state \'S637\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp431;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp431 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1039\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp431, "return of NetrLogonControl, state S1039");
                    this.Manager.Comment("reaching state \'S1377\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S383");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S383");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S384");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S384");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S385");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS32GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S385");
        }
        #endregion
        
        #region Test Starting in S34
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp434;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp434);
            this.Manager.AddReturn(GetPlatformInfo, null, temp434);
            this.Manager.Comment("reaching state \'S35\'");
            int temp447 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetPlatformChecker2)));
            if ((temp447 == 0)) {
                this.Manager.Comment("reaching state \'S185\'");
                bool temp435;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp435);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp435);
                this.Manager.Comment("reaching state \'S386\'");
                int temp438 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetClientAccountTypeChecker1)));
                if ((temp438 == 0)) {
                    this.Manager.Comment("reaching state \'S638\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp436;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,1)\'");
                    temp436 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1040\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp436, "return of NetrLogonControl, state S1040");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label124;
                }
                if ((temp438 == 1)) {
                    this.Manager.Comment("reaching state \'S639\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp437;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,5,1)\'");
                    temp437 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 5u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1041\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_INVALID_PARAMETER, temp437, "return of NetrLogonControl, state S1041");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label124;
                }
                throw new InvalidOperationException("never reached");
            label124:
;
                goto label127;
            }
            if ((temp447 == 1)) {
                this.Manager.Comment("reaching state \'S186\'");
                bool temp439;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp439);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp439);
                this.Manager.Comment("reaching state \'S387\'");
                int temp442 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetClientAccountTypeChecker3)));
                if ((temp442 == 0)) {
                    this.Manager.Comment("reaching state \'S640\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp440;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,1)\'");
                    temp440 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1042\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp440, "return of NetrLogonControl, state S1042");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label125;
                }
                if ((temp442 == 1)) {
                    this.Manager.Comment("reaching state \'S641\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp441;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65533,1)\'");
                    temp441 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1043\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp441, "return of NetrLogonControl, state S1043");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label125;
                }
                throw new InvalidOperationException("never reached");
            label125:
;
                goto label127;
            }
            if ((temp447 == 2)) {
                this.Manager.Comment("reaching state \'S187\'");
                bool temp443;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp443);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp443);
                this.Manager.Comment("reaching state \'S388\'");
                int temp446 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetClientAccountTypeChecker5)));
                if ((temp446 == 0)) {
                    this.Manager.Comment("reaching state \'S642\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp444;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp444 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1044\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp444, "return of NetrLogonControl, state S1044");
                    this.Manager.Comment("reaching state \'S1378\'");
                    goto label126;
                }
                if ((temp446 == 1)) {
                    this.Manager.Comment("reaching state \'S643\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp445;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp445 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1045\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp445, "return of NetrLogonControl, state S1045");
                    this.Manager.Comment("reaching state \'S1379\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S386");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S386");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S387");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S387");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S388");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS34GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S388");
        }
        #endregion
        
        #region Test Starting in S36
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp448;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp448);
            this.Manager.AddReturn(GetPlatformInfo, null, temp448);
            this.Manager.Comment("reaching state \'S37\'");
            int temp461 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetPlatformChecker2)));
            if ((temp461 == 0)) {
                this.Manager.Comment("reaching state \'S188\'");
                bool temp449;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp449);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp449);
                this.Manager.Comment("reaching state \'S389\'");
                int temp452 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetClientAccountTypeChecker1)));
                if ((temp452 == 0)) {
                    this.Manager.Comment("reaching state \'S644\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp450;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,2,1)\'");
                    temp450 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 2u, 1u);
                    this.Manager.Comment("reaching state \'S1046\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp450, "return of NetrLogonControl, state S1046");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label128;
                }
                if ((temp452 == 1)) {
                    this.Manager.Comment("reaching state \'S645\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp451;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,6,1)\'");
                    temp451 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 6u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1047\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_INVALID_PARAMETER, temp451, "return of NetrLogonControl, state S1047");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label128;
                }
                throw new InvalidOperationException("never reached");
            label128:
;
                goto label131;
            }
            if ((temp461 == 1)) {
                this.Manager.Comment("reaching state \'S189\'");
                bool temp453;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp453);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp453);
                this.Manager.Comment("reaching state \'S390\'");
                int temp456 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetClientAccountTypeChecker3)));
                if ((temp456 == 0)) {
                    this.Manager.Comment("reaching state \'S646\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp454;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,2,1)\'");
                    temp454 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 2u, 1u);
                    this.Manager.Comment("reaching state \'S1048\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp454, "return of NetrLogonControl, state S1048");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label129;
                }
                if ((temp456 == 1)) {
                    this.Manager.Comment("reaching state \'S647\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp455;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,1)\'");
                    temp455 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1049\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp455, "return of NetrLogonControl, state S1049");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label129;
                }
                throw new InvalidOperationException("never reached");
            label129:
;
                goto label131;
            }
            if ((temp461 == 2)) {
                this.Manager.Comment("reaching state \'S190\'");
                bool temp457;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp457);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp457);
                this.Manager.Comment("reaching state \'S391\'");
                int temp460 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetClientAccountTypeChecker5)));
                if ((temp460 == 0)) {
                    this.Manager.Comment("reaching state \'S648\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp458;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp458 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1050\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp458, "return of NetrLogonControl, state S1050");
                    this.Manager.Comment("reaching state \'S1380\'");
                    goto label130;
                }
                if ((temp460 == 1)) {
                    this.Manager.Comment("reaching state \'S649\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp459;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp459 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1051\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp459, "return of NetrLogonControl, state S1051");
                    this.Manager.Comment("reaching state \'S1381\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S389");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S389");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S390");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S390");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S391");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS36GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S391");
        }
        #endregion
        
        #region Test Starting in S38
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp462;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp462);
            this.Manager.AddReturn(GetPlatformInfo, null, temp462);
            this.Manager.Comment("reaching state \'S39\'");
            int temp475 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetPlatformChecker2)));
            if ((temp475 == 0)) {
                this.Manager.Comment("reaching state \'S191\'");
                bool temp463;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp463);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp463);
                this.Manager.Comment("reaching state \'S392\'");
                int temp466 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetClientAccountTypeChecker1)));
                if ((temp466 == 0)) {
                    this.Manager.Comment("reaching state \'S650\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp464;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,3,1)\'");
                    temp464 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 3u, 1u);
                    this.Manager.Comment("reaching state \'S1052\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp464, "return of NetrLogonControl, state S1052");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label132;
                }
                if ((temp466 == 1)) {
                    this.Manager.Comment("reaching state \'S651\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp465;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,7,1)\'");
                    temp465 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 7u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1053\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp465, "return of NetrLogonControl, state S1053");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label132;
                }
                throw new InvalidOperationException("never reached");
            label132:
;
                goto label135;
            }
            if ((temp475 == 1)) {
                this.Manager.Comment("reaching state \'S192\'");
                bool temp467;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp467);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp467);
                this.Manager.Comment("reaching state \'S393\'");
                int temp470 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetClientAccountTypeChecker3)));
                if ((temp470 == 0)) {
                    this.Manager.Comment("reaching state \'S652\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp468;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,3,1)\'");
                    temp468 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 3u, 1u);
                    this.Manager.Comment("reaching state \'S1054\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp468, "return of NetrLogonControl, state S1054");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label133;
                }
                if ((temp470 == 1)) {
                    this.Manager.Comment("reaching state \'S653\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp469;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,2,1)\'");
                    temp469 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 2u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1055\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp469, "return of NetrLogonControl, state S1055");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label133;
                }
                throw new InvalidOperationException("never reached");
            label133:
;
                goto label135;
            }
            if ((temp475 == 2)) {
                this.Manager.Comment("reaching state \'S193\'");
                bool temp471;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp471);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp471);
                this.Manager.Comment("reaching state \'S394\'");
                int temp474 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetClientAccountTypeChecker5)));
                if ((temp474 == 0)) {
                    this.Manager.Comment("reaching state \'S654\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp472;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp472 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1056\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp472, "return of NetrLogonControl, state S1056");
                    this.Manager.Comment("reaching state \'S1382\'");
                    goto label134;
                }
                if ((temp474 == 1)) {
                    this.Manager.Comment("reaching state \'S655\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp473;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp473 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1057\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp473, "return of NetrLogonControl, state S1057");
                    this.Manager.Comment("reaching state \'S1383\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S392");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S392");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S393");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S393");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S394");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS38GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S394");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp476;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp476);
            this.Manager.AddReturn(GetPlatformInfo, null, temp476);
            this.Manager.Comment("reaching state \'S5\'");
            int temp489 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetPlatformChecker2)));
            if ((temp489 == 0)) {
                this.Manager.Comment("reaching state \'S140\'");
                bool temp477;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp477);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp477);
                this.Manager.Comment("reaching state \'S341\'");
                int temp480 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetClientAccountTypeChecker1)));
                if ((temp480 == 0)) {
                    this.Manager.Comment("reaching state \'S548\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp478;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,2,1)\'");
                    temp478 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 2u, 1u);
                    this.Manager.Comment("reaching state \'S950\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp478, "return of NetrLogonControl, state S950");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label136;
                }
                if ((temp480 == 1)) {
                    this.Manager.Comment("reaching state \'S549\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp479;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,2,1)\'");
                    temp479 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 2u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S951\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp479, "return of NetrLogonControl, state S951");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label136;
                }
                throw new InvalidOperationException("never reached");
            label136:
;
                goto label139;
            }
            if ((temp489 == 1)) {
                this.Manager.Comment("reaching state \'S141\'");
                bool temp481;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp481);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp481);
                this.Manager.Comment("reaching state \'S342\'");
                int temp484 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetClientAccountTypeChecker3)));
                if ((temp484 == 0)) {
                    this.Manager.Comment("reaching state \'S550\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp482;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,2,1)\'");
                    temp482 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 2u, 1u);
                    this.Manager.Comment("reaching state \'S952\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp482, "return of NetrLogonControl, state S952");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label137;
                }
                if ((temp484 == 1)) {
                    this.Manager.Comment("reaching state \'S551\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp483;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp483 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S953\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp483, "return of NetrLogonControl, state S953");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label137;
                }
                throw new InvalidOperationException("never reached");
            label137:
;
                goto label139;
            }
            if ((temp489 == 2)) {
                this.Manager.Comment("reaching state \'S142\'");
                bool temp485;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp485);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp485);
                this.Manager.Comment("reaching state \'S343\'");
                int temp488 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetClientAccountTypeChecker5)));
                if ((temp488 == 0)) {
                    this.Manager.Comment("reaching state \'S552\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp486;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp486 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S954\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp486, "return of NetrLogonControl, state S954");
                    this.Manager.Comment("reaching state \'S1348\'");
                    goto label138;
                }
                if ((temp488 == 1)) {
                    this.Manager.Comment("reaching state \'S553\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp487;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp487 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S955\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp487, "return of NetrLogonControl, state S955");
                    this.Manager.Comment("reaching state \'S1349\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S341");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S341");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S342");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S342");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S343");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS4GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S343");
        }
        #endregion
        
        #region Test Starting in S40
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp490;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp490);
            this.Manager.AddReturn(GetPlatformInfo, null, temp490);
            this.Manager.Comment("reaching state \'S41\'");
            int temp503 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetPlatformChecker2)));
            if ((temp503 == 0)) {
                this.Manager.Comment("reaching state \'S194\'");
                bool temp491;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp491);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp491);
                this.Manager.Comment("reaching state \'S395\'");
                int temp494 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetClientAccountTypeChecker1)));
                if ((temp494 == 0)) {
                    this.Manager.Comment("reaching state \'S656\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp492;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,4,1)\'");
                    temp492 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 4u, 1u);
                    this.Manager.Comment("reaching state \'S1058\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp492, "return of NetrLogonControl, state S1058");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label140;
                }
                if ((temp494 == 1)) {
                    this.Manager.Comment("reaching state \'S657\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp493;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,8,1)\'");
                    temp493 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1059\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_INVALID_PARAMETER, temp493, "return of NetrLogonControl, state S1059");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label140;
                }
                throw new InvalidOperationException("never reached");
            label140:
;
                goto label143;
            }
            if ((temp503 == 1)) {
                this.Manager.Comment("reaching state \'S195\'");
                bool temp495;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp495);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp495);
                this.Manager.Comment("reaching state \'S396\'");
                int temp498 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetClientAccountTypeChecker3)));
                if ((temp498 == 0)) {
                    this.Manager.Comment("reaching state \'S658\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp496;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,4,1)\'");
                    temp496 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 4u, 1u);
                    this.Manager.Comment("reaching state \'S1060\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp496, "return of NetrLogonControl, state S1060");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label141;
                }
                if ((temp498 == 1)) {
                    this.Manager.Comment("reaching state \'S659\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp497;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,3,1)\'");
                    temp497 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 3u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1061\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp497, "return of NetrLogonControl, state S1061");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label141;
                }
                throw new InvalidOperationException("never reached");
            label141:
;
                goto label143;
            }
            if ((temp503 == 2)) {
                this.Manager.Comment("reaching state \'S196\'");
                bool temp499;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp499);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp499);
                this.Manager.Comment("reaching state \'S397\'");
                int temp502 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetClientAccountTypeChecker5)));
                if ((temp502 == 0)) {
                    this.Manager.Comment("reaching state \'S660\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp500;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp500 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1062\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp500, "return of NetrLogonControl, state S1062");
                    this.Manager.Comment("reaching state \'S1384\'");
                    goto label142;
                }
                if ((temp502 == 1)) {
                    this.Manager.Comment("reaching state \'S661\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp501;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp501 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1063\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp501, "return of NetrLogonControl, state S1063");
                    this.Manager.Comment("reaching state \'S1385\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S395");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S395");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S396");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S396");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S397");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS40GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S397");
        }
        #endregion
        
        #region Test Starting in S42
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp504;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp504);
            this.Manager.AddReturn(GetPlatformInfo, null, temp504);
            this.Manager.Comment("reaching state \'S43\'");
            int temp517 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetPlatformChecker2)));
            if ((temp517 == 0)) {
                this.Manager.Comment("reaching state \'S197\'");
                bool temp505;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp505);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp505);
                this.Manager.Comment("reaching state \'S398\'");
                int temp508 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetClientAccountTypeChecker1)));
                if ((temp508 == 0)) {
                    this.Manager.Comment("reaching state \'S662\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp506;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,5,1)\'");
                    temp506 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 5u, 1u);
                    this.Manager.Comment("reaching state \'S1064\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp506, "return of NetrLogonControl, state S1064");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label144;
                }
                if ((temp508 == 1)) {
                    this.Manager.Comment("reaching state \'S663\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp507;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,9,1)\'");
                    temp507 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 9u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1065\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_INVALID_PARAMETER, temp507, "return of NetrLogonControl, state S1065");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label144;
                }
                throw new InvalidOperationException("never reached");
            label144:
;
                goto label147;
            }
            if ((temp517 == 1)) {
                this.Manager.Comment("reaching state \'S198\'");
                bool temp509;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp509);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp509);
                this.Manager.Comment("reaching state \'S399\'");
                int temp512 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetClientAccountTypeChecker3)));
                if ((temp512 == 0)) {
                    this.Manager.Comment("reaching state \'S664\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp510;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,5,1)\'");
                    temp510 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 5u, 1u);
                    this.Manager.Comment("reaching state \'S1066\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp510, "return of NetrLogonControl, state S1066");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label145;
                }
                if ((temp512 == 1)) {
                    this.Manager.Comment("reaching state \'S665\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp511;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,6,1)\'");
                    temp511 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 6u, 1u);
                    this.Manager.Comment("reaching state \'S1067\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp511, "return of NetrLogonControl, state S1067");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label145;
                }
                throw new InvalidOperationException("never reached");
            label145:
;
                goto label147;
            }
            if ((temp517 == 2)) {
                this.Manager.Comment("reaching state \'S199\'");
                bool temp513;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp513);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp513);
                this.Manager.Comment("reaching state \'S400\'");
                int temp516 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetClientAccountTypeChecker5)));
                if ((temp516 == 0)) {
                    this.Manager.Comment("reaching state \'S666\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp514;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp514 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1068\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp514, "return of NetrLogonControl, state S1068");
                    this.Manager.Comment("reaching state \'S1386\'");
                    goto label146;
                }
                if ((temp516 == 1)) {
                    this.Manager.Comment("reaching state \'S667\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp515;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp515 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1069\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp515, "return of NetrLogonControl, state S1069");
                    this.Manager.Comment("reaching state \'S1387\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S398");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S398");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S399");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S399");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S400");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS42GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S400");
        }
        #endregion
        
        #region Test Starting in S44
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp518;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp518);
            this.Manager.AddReturn(GetPlatformInfo, null, temp518);
            this.Manager.Comment("reaching state \'S45\'");
            int temp531 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetPlatformChecker2)));
            if ((temp531 == 0)) {
                this.Manager.Comment("reaching state \'S200\'");
                bool temp519;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp519);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp519);
                this.Manager.Comment("reaching state \'S401\'");
                int temp522 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetClientAccountTypeChecker1)));
                if ((temp522 == 0)) {
                    this.Manager.Comment("reaching state \'S668\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp520;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,6,1)\'");
                    temp520 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 6u, 1u);
                    this.Manager.Comment("reaching state \'S1070\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp520, "return of NetrLogonControl, state S1070");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label148;
                }
                if ((temp522 == 1)) {
                    this.Manager.Comment("reaching state \'S669\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp521;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,10,1)\'");
                    temp521 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 10u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1071\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_INVALID_LEVEL\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_INVALID_LEVEL, temp521, "return of NetrLogonControl, state S1071");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label148;
                }
                throw new InvalidOperationException("never reached");
            label148:
;
                goto label151;
            }
            if ((temp531 == 1)) {
                this.Manager.Comment("reaching state \'S201\'");
                bool temp523;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp523);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp523);
                this.Manager.Comment("reaching state \'S402\'");
                int temp526 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetClientAccountTypeChecker3)));
                if ((temp526 == 0)) {
                    this.Manager.Comment("reaching state \'S670\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp524;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,6,1)\'");
                    temp524 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 6u, 1u);
                    this.Manager.Comment("reaching state \'S1072\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp524, "return of NetrLogonControl, state S1072");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label149;
                }
                if ((temp526 == 1)) {
                    this.Manager.Comment("reaching state \'S671\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp525;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65534,1)\'");
                    temp525 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65534u, 1u);
                    this.Manager.Comment("reaching state \'S1073\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp525, "return of NetrLogonControl, state S1073");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label149;
                }
                throw new InvalidOperationException("never reached");
            label149:
;
                goto label151;
            }
            if ((temp531 == 2)) {
                this.Manager.Comment("reaching state \'S202\'");
                bool temp527;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp527);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp527);
                this.Manager.Comment("reaching state \'S403\'");
                int temp530 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetClientAccountTypeChecker5)));
                if ((temp530 == 0)) {
                    this.Manager.Comment("reaching state \'S672\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp528;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp528 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1074\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp528, "return of NetrLogonControl, state S1074");
                    this.Manager.Comment("reaching state \'S1388\'");
                    goto label150;
                }
                if ((temp530 == 1)) {
                    this.Manager.Comment("reaching state \'S673\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp529;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp529 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1075\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp529, "return of NetrLogonControl, state S1075");
                    this.Manager.Comment("reaching state \'S1389\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S401");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S401");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S402");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S402");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S403");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS44GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S403");
        }
        #endregion
        
        #region Test Starting in S46
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp532;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp532);
            this.Manager.AddReturn(GetPlatformInfo, null, temp532);
            this.Manager.Comment("reaching state \'S47\'");
            int temp545 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetPlatformChecker2)));
            if ((temp545 == 0)) {
                this.Manager.Comment("reaching state \'S203\'");
                bool temp533;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp533);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp533);
                this.Manager.Comment("reaching state \'S404\'");
                int temp536 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetClientAccountTypeChecker1)));
                if ((temp536 == 0)) {
                    this.Manager.Comment("reaching state \'S674\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp534;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,7,1)\'");
                    temp534 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 7u, 1u);
                    this.Manager.Comment("reaching state \'S1076\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp534, "return of NetrLogonControl, state S1076");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label152;
                }
                if ((temp536 == 1)) {
                    this.Manager.Comment("reaching state \'S675\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp535;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,11,1)\'");
                    temp535 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 11u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1077\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp535, "return of NetrLogonControl, state S1077");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label152;
                }
                throw new InvalidOperationException("never reached");
            label152:
;
                goto label155;
            }
            if ((temp545 == 1)) {
                this.Manager.Comment("reaching state \'S204\'");
                bool temp537;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp537);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp537);
                this.Manager.Comment("reaching state \'S405\'");
                int temp540 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetClientAccountTypeChecker3)));
                if ((temp540 == 0)) {
                    this.Manager.Comment("reaching state \'S676\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp538;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,7,1)\'");
                    temp538 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 7u, 1u);
                    this.Manager.Comment("reaching state \'S1078\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp538, "return of NetrLogonControl, state S1078");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label153;
                }
                if ((temp540 == 1)) {
                    this.Manager.Comment("reaching state \'S677\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp539;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,10,1)\'");
                    temp539 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 10u, 1u);
                    this.Manager.Comment("reaching state \'S1079\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp539, "return of NetrLogonControl, state S1079");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label153;
                }
                throw new InvalidOperationException("never reached");
            label153:
;
                goto label155;
            }
            if ((temp545 == 2)) {
                this.Manager.Comment("reaching state \'S205\'");
                bool temp541;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp541);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp541);
                this.Manager.Comment("reaching state \'S406\'");
                int temp544 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetClientAccountTypeChecker5)));
                if ((temp544 == 0)) {
                    this.Manager.Comment("reaching state \'S678\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp542;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp542 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1080\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp542, "return of NetrLogonControl, state S1080");
                    this.Manager.Comment("reaching state \'S1390\'");
                    goto label154;
                }
                if ((temp544 == 1)) {
                    this.Manager.Comment("reaching state \'S679\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp543;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp543 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1081\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp543, "return of NetrLogonControl, state S1081");
                    this.Manager.Comment("reaching state \'S1391\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S404");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S404");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S405");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S405");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S406");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS46GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S406");
        }
        #endregion
        
        #region Test Starting in S48
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp546;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp546);
            this.Manager.AddReturn(GetPlatformInfo, null, temp546);
            this.Manager.Comment("reaching state \'S49\'");
            int temp559 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetPlatformChecker2)));
            if ((temp559 == 0)) {
                this.Manager.Comment("reaching state \'S206\'");
                bool temp547;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp547);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp547);
                this.Manager.Comment("reaching state \'S407\'");
                int temp550 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetClientAccountTypeChecker1)));
                if ((temp550 == 0)) {
                    this.Manager.Comment("reaching state \'S680\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp548;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,8,1)\'");
                    temp548 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8u, 1u);
                    this.Manager.Comment("reaching state \'S1082\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp548, "return of NetrLogonControl, state S1082");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label156;
                }
                if ((temp550 == 1)) {
                    this.Manager.Comment("reaching state \'S681\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp549;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,12,1)\'");
                    temp549 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 12u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1083\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp549, "return of NetrLogonControl, state S1083");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label156;
                }
                throw new InvalidOperationException("never reached");
            label156:
;
                goto label159;
            }
            if ((temp559 == 1)) {
                this.Manager.Comment("reaching state \'S207\'");
                bool temp551;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp551);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp551);
                this.Manager.Comment("reaching state \'S408\'");
                int temp554 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetClientAccountTypeChecker3)));
                if ((temp554 == 0)) {
                    this.Manager.Comment("reaching state \'S682\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp552;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,8,1)\'");
                    temp552 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8u, 1u);
                    this.Manager.Comment("reaching state \'S1084\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp552, "return of NetrLogonControl, state S1084");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label157;
                }
                if ((temp554 == 1)) {
                    this.Manager.Comment("reaching state \'S683\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp553;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,8,1)\'");
                    temp553 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8u, 1u);
                    this.Manager.Comment("reaching state \'S1085\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp553, "return of NetrLogonControl, state S1085");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label157;
                }
                throw new InvalidOperationException("never reached");
            label157:
;
                goto label159;
            }
            if ((temp559 == 2)) {
                this.Manager.Comment("reaching state \'S208\'");
                bool temp555;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp555);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp555);
                this.Manager.Comment("reaching state \'S409\'");
                int temp558 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetClientAccountTypeChecker5)));
                if ((temp558 == 0)) {
                    this.Manager.Comment("reaching state \'S684\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp556;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp556 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1086\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp556, "return of NetrLogonControl, state S1086");
                    this.Manager.Comment("reaching state \'S1392\'");
                    goto label158;
                }
                if ((temp558 == 1)) {
                    this.Manager.Comment("reaching state \'S685\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp557;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp557 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1087\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp557, "return of NetrLogonControl, state S1087");
                    this.Manager.Comment("reaching state \'S1393\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S407");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S407");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S408");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S408");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S409");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS48GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S409");
        }
        #endregion
        
        #region Test Starting in S50
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp560;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp560);
            this.Manager.AddReturn(GetPlatformInfo, null, temp560);
            this.Manager.Comment("reaching state \'S51\'");
            int temp573 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetPlatformChecker2)));
            if ((temp573 == 0)) {
                this.Manager.Comment("reaching state \'S209\'");
                bool temp561;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp561);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp561);
                this.Manager.Comment("reaching state \'S410\'");
                int temp564 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetClientAccountTypeChecker1)));
                if ((temp564 == 0)) {
                    this.Manager.Comment("reaching state \'S686\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp562;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,9,1)\'");
                    temp562 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 9u, 1u);
                    this.Manager.Comment("reaching state \'S1088\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp562, "return of NetrLogonControl, state S1088");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label160;
                }
                if ((temp564 == 1)) {
                    this.Manager.Comment("reaching state \'S687\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp563;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65532,1)\'");
                    temp563 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65532u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1089\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp563, "return of NetrLogonControl, state S1089");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label160;
                }
                throw new InvalidOperationException("never reached");
            label160:
;
                goto label163;
            }
            if ((temp573 == 1)) {
                this.Manager.Comment("reaching state \'S210\'");
                bool temp565;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp565);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp565);
                this.Manager.Comment("reaching state \'S411\'");
                int temp568 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetClientAccountTypeChecker3)));
                if ((temp568 == 0)) {
                    this.Manager.Comment("reaching state \'S688\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp566;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,9,1)\'");
                    temp566 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 9u, 1u);
                    this.Manager.Comment("reaching state \'S1090\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp566, "return of NetrLogonControl, state S1090");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label161;
                }
                if ((temp568 == 1)) {
                    this.Manager.Comment("reaching state \'S689\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp567;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,12,1)\'");
                    temp567 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 12u, 1u);
                    this.Manager.Comment("reaching state \'S1091\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp567, "return of NetrLogonControl, state S1091");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label161;
                }
                throw new InvalidOperationException("never reached");
            label161:
;
                goto label163;
            }
            if ((temp573 == 2)) {
                this.Manager.Comment("reaching state \'S211\'");
                bool temp569;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp569);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp569);
                this.Manager.Comment("reaching state \'S412\'");
                int temp572 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetClientAccountTypeChecker5)));
                if ((temp572 == 0)) {
                    this.Manager.Comment("reaching state \'S690\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp570;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp570 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1092\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp570, "return of NetrLogonControl, state S1092");
                    this.Manager.Comment("reaching state \'S1394\'");
                    goto label162;
                }
                if ((temp572 == 1)) {
                    this.Manager.Comment("reaching state \'S691\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp571;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp571 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1093\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp571, "return of NetrLogonControl, state S1093");
                    this.Manager.Comment("reaching state \'S1395\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S410");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S410");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S411");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S411");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S412");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS50GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S412");
        }
        #endregion
        
        #region Test Starting in S52
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp574;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp574);
            this.Manager.AddReturn(GetPlatformInfo, null, temp574);
            this.Manager.Comment("reaching state \'S53\'");
            int temp587 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetPlatformChecker2)));
            if ((temp587 == 0)) {
                this.Manager.Comment("reaching state \'S212\'");
                bool temp575;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp575);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp575);
                this.Manager.Comment("reaching state \'S413\'");
                int temp578 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetClientAccountTypeChecker1)));
                if ((temp578 == 0)) {
                    this.Manager.Comment("reaching state \'S692\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp576;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,10,1)\'");
                    temp576 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 10u, 1u);
                    this.Manager.Comment("reaching state \'S1094\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp576, "return of NetrLogonControl, state S1094");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label164;
                }
                if ((temp578 == 1)) {
                    this.Manager.Comment("reaching state \'S693\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp577;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65533,1)\'");
                    temp577 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1095\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp577, "return of NetrLogonControl, state S1095");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label164;
                }
                throw new InvalidOperationException("never reached");
            label164:
;
                goto label167;
            }
            if ((temp587 == 1)) {
                this.Manager.Comment("reaching state \'S213\'");
                bool temp579;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp579);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp579);
                this.Manager.Comment("reaching state \'S414\'");
                int temp582 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetClientAccountTypeChecker3)));
                if ((temp582 == 0)) {
                    this.Manager.Comment("reaching state \'S694\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp580;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,10,1)\'");
                    temp580 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 10u, 1u);
                    this.Manager.Comment("reaching state \'S1096\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp580, "return of NetrLogonControl, state S1096");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label165;
                }
                if ((temp582 == 1)) {
                    this.Manager.Comment("reaching state \'S695\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp581;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,4,1)\'");
                    temp581 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 4u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104233");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1097\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp581, "return of NetrLogonControl, state S1097");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label165;
                }
                throw new InvalidOperationException("never reached");
            label165:
;
                goto label167;
            }
            if ((temp587 == 2)) {
                this.Manager.Comment("reaching state \'S214\'");
                bool temp583;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp583);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp583);
                this.Manager.Comment("reaching state \'S415\'");
                int temp586 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetClientAccountTypeChecker5)));
                if ((temp586 == 0)) {
                    this.Manager.Comment("reaching state \'S696\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp584;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp584 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1098\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp584, "return of NetrLogonControl, state S1098");
                    this.Manager.Comment("reaching state \'S1396\'");
                    goto label166;
                }
                if ((temp586 == 1)) {
                    this.Manager.Comment("reaching state \'S697\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp585;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp585 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1099\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp585, "return of NetrLogonControl, state S1099");
                    this.Manager.Comment("reaching state \'S1397\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S413");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S413");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S414");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S414");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S415");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS52GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S415");
        }
        #endregion
        
        #region Test Starting in S54
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp588;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp588);
            this.Manager.AddReturn(GetPlatformInfo, null, temp588);
            this.Manager.Comment("reaching state \'S55\'");
            int temp601 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetPlatformChecker2)));
            if ((temp601 == 0)) {
                this.Manager.Comment("reaching state \'S215\'");
                bool temp589;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp589);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp589);
                this.Manager.Comment("reaching state \'S416\'");
                int temp592 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetClientAccountTypeChecker1)));
                if ((temp592 == 0)) {
                    this.Manager.Comment("reaching state \'S698\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp590;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,11,1)\'");
                    temp590 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 11u, 1u);
                    this.Manager.Comment("reaching state \'S1100\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp590, "return of NetrLogonControl, state S1100");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label168;
                }
                if ((temp592 == 1)) {
                    this.Manager.Comment("reaching state \'S699\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp591;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65535,1)\'");
                    temp591 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1101\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp591, "return of NetrLogonControl, state S1101");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label168;
                }
                throw new InvalidOperationException("never reached");
            label168:
;
                goto label171;
            }
            if ((temp601 == 1)) {
                this.Manager.Comment("reaching state \'S216\'");
                bool temp593;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp593);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp593);
                this.Manager.Comment("reaching state \'S417\'");
                int temp596 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetClientAccountTypeChecker3)));
                if ((temp596 == 0)) {
                    this.Manager.Comment("reaching state \'S700\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp594;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,11,1)\'");
                    temp594 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 11u, 1u);
                    this.Manager.Comment("reaching state \'S1102\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp594, "return of NetrLogonControl, state S1102");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label169;
                }
                if ((temp596 == 1)) {
                    this.Manager.Comment("reaching state \'S701\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp595;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65532,1)\'");
                    temp595 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65532u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1103\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp595, "return of NetrLogonControl, state S1103");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label169;
                }
                throw new InvalidOperationException("never reached");
            label169:
;
                goto label171;
            }
            if ((temp601 == 2)) {
                this.Manager.Comment("reaching state \'S217\'");
                bool temp597;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp597);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp597);
                this.Manager.Comment("reaching state \'S418\'");
                int temp600 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetClientAccountTypeChecker5)));
                if ((temp600 == 0)) {
                    this.Manager.Comment("reaching state \'S702\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp598;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp598 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1104\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp598, "return of NetrLogonControl, state S1104");
                    this.Manager.Comment("reaching state \'S1398\'");
                    goto label170;
                }
                if ((temp600 == 1)) {
                    this.Manager.Comment("reaching state \'S703\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp599;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp599 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1105\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp599, "return of NetrLogonControl, state S1105");
                    this.Manager.Comment("reaching state \'S1399\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S416");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S416");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S417");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S417");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S418");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS54GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S418");
        }
        #endregion
        
        #region Test Starting in S56
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp602;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp602);
            this.Manager.AddReturn(GetPlatformInfo, null, temp602);
            this.Manager.Comment("reaching state \'S57\'");
            int temp615 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetPlatformChecker2)));
            if ((temp615 == 0)) {
                this.Manager.Comment("reaching state \'S218\'");
                bool temp603;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp603);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp603);
                this.Manager.Comment("reaching state \'S419\'");
                int temp606 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetClientAccountTypeChecker1)));
                if ((temp606 == 0)) {
                    this.Manager.Comment("reaching state \'S704\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp604;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,12,1)\'");
                    temp604 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 12u, 1u);
                    this.Manager.Comment("reaching state \'S1106\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp604, "return of NetrLogonControl, state S1106");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label172;
                }
                if ((temp606 == 1)) {
                    this.Manager.Comment("reaching state \'S705\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp605;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,4,1)\'");
                    temp605 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 4u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104233");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1107\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp605, "return of NetrLogonControl, state S1107");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label172;
                }
                throw new InvalidOperationException("never reached");
            label172:
;
                goto label175;
            }
            if ((temp615 == 1)) {
                this.Manager.Comment("reaching state \'S219\'");
                bool temp607;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp607);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp607);
                this.Manager.Comment("reaching state \'S420\'");
                int temp610 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetClientAccountTypeChecker3)));
                if ((temp610 == 0)) {
                    this.Manager.Comment("reaching state \'S706\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp608;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,12,1)\'");
                    temp608 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 12u, 1u);
                    this.Manager.Comment("reaching state \'S1108\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp608, "return of NetrLogonControl, state S1108");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label173;
                }
                if ((temp610 == 1)) {
                    this.Manager.Comment("reaching state \'S707\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp609;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,9,1)\'");
                    temp609 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 9u, 1u);
                    this.Manager.Comment("reaching state \'S1109\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp609, "return of NetrLogonControl, state S1109");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label173;
                }
                throw new InvalidOperationException("never reached");
            label173:
;
                goto label175;
            }
            if ((temp615 == 2)) {
                this.Manager.Comment("reaching state \'S220\'");
                bool temp611;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp611);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp611);
                this.Manager.Comment("reaching state \'S421\'");
                int temp614 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetClientAccountTypeChecker5)));
                if ((temp614 == 0)) {
                    this.Manager.Comment("reaching state \'S708\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp612;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp612 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1110\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp612, "return of NetrLogonControl, state S1110");
                    this.Manager.Comment("reaching state \'S1400\'");
                    goto label174;
                }
                if ((temp614 == 1)) {
                    this.Manager.Comment("reaching state \'S709\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp613;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp613 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1111\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp613, "return of NetrLogonControl, state S1111");
                    this.Manager.Comment("reaching state \'S1401\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S419");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S419");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S420");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S420");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S421");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS56GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S421");
        }
        #endregion
        
        #region Test Starting in S58
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp616;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp616);
            this.Manager.AddReturn(GetPlatformInfo, null, temp616);
            this.Manager.Comment("reaching state \'S59\'");
            int temp629 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetPlatformChecker2)));
            if ((temp629 == 0)) {
                this.Manager.Comment("reaching state \'S221\'");
                bool temp617;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp617);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp617);
                this.Manager.Comment("reaching state \'S422\'");
                int temp620 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetClientAccountTypeChecker1)));
                if ((temp620 == 0)) {
                    this.Manager.Comment("reaching state \'S710\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp618;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65532,1)\'");
                    temp618 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65532u, 1u);
                    this.Manager.Comment("reaching state \'S1112\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp618, "return of NetrLogonControl, state S1112");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label176;
                }
                if ((temp620 == 1)) {
                    this.Manager.Comment("reaching state \'S711\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp619;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,2)\'");
                    temp619 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 2u);
                    this.Manager.Checkpoint("MS-NRPC_R104020");
                    this.Manager.Comment("reaching state \'S1113\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp619, "return of NetrLogonControl, state S1113");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label176;
                }
                throw new InvalidOperationException("never reached");
            label176:
;
                goto label179;
            }
            if ((temp629 == 1)) {
                this.Manager.Comment("reaching state \'S222\'");
                bool temp621;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp621);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp621);
                this.Manager.Comment("reaching state \'S423\'");
                int temp624 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetClientAccountTypeChecker3)));
                if ((temp624 == 0)) {
                    this.Manager.Comment("reaching state \'S712\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp622;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65532,1)\'");
                    temp622 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65532u, 1u);
                    this.Manager.Comment("reaching state \'S1114\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp622, "return of NetrLogonControl, state S1114");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label177;
                }
                if ((temp624 == 1)) {
                    this.Manager.Comment("reaching state \'S713\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp623;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,5,1)\'");
                    temp623 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 5u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104917");
                    this.Manager.Comment("reaching state \'S1115\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp623, "return of NetrLogonControl, state S1115");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label177;
                }
                throw new InvalidOperationException("never reached");
            label177:
;
                goto label179;
            }
            if ((temp629 == 2)) {
                this.Manager.Comment("reaching state \'S223\'");
                bool temp625;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp625);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp625);
                this.Manager.Comment("reaching state \'S424\'");
                int temp628 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetClientAccountTypeChecker5)));
                if ((temp628 == 0)) {
                    this.Manager.Comment("reaching state \'S714\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp626;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp626 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1116\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp626, "return of NetrLogonControl, state S1116");
                    this.Manager.Comment("reaching state \'S1402\'");
                    goto label178;
                }
                if ((temp628 == 1)) {
                    this.Manager.Comment("reaching state \'S715\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp627;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp627 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1117\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp627, "return of NetrLogonControl, state S1117");
                    this.Manager.Comment("reaching state \'S1403\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S422");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S422");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S423");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S423");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S424");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS58GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S424");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp630;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp630);
            this.Manager.AddReturn(GetPlatformInfo, null, temp630);
            this.Manager.Comment("reaching state \'S7\'");
            int temp643 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetPlatformChecker2)));
            if ((temp643 == 0)) {
                this.Manager.Comment("reaching state \'S143\'");
                bool temp631;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp631);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp631);
                this.Manager.Comment("reaching state \'S344\'");
                int temp634 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetClientAccountTypeChecker1)));
                if ((temp634 == 0)) {
                    this.Manager.Comment("reaching state \'S554\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp632;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,3,1)\'");
                    temp632 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 3u, 1u);
                    this.Manager.Comment("reaching state \'S956\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp632, "return of NetrLogonControl, state S956");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label180;
                }
                if ((temp634 == 1)) {
                    this.Manager.Comment("reaching state \'S555\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp633;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65532,1)\'");
                    temp633 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65532u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S957\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp633, "return of NetrLogonControl, state S957");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label180;
                }
                throw new InvalidOperationException("never reached");
            label180:
;
                goto label183;
            }
            if ((temp643 == 1)) {
                this.Manager.Comment("reaching state \'S144\'");
                bool temp635;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp635);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp635);
                this.Manager.Comment("reaching state \'S345\'");
                int temp638 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetClientAccountTypeChecker3)));
                if ((temp638 == 0)) {
                    this.Manager.Comment("reaching state \'S556\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp636;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,3,1)\'");
                    temp636 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 3u, 1u);
                    this.Manager.Comment("reaching state \'S958\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp636, "return of NetrLogonControl, state S958");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label181;
                }
                if ((temp638 == 1)) {
                    this.Manager.Comment("reaching state \'S557\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp637;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,2,1)\'");
                    temp637 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 2u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S959\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp637, "return of NetrLogonControl, state S959");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label181;
                }
                throw new InvalidOperationException("never reached");
            label181:
;
                goto label183;
            }
            if ((temp643 == 2)) {
                this.Manager.Comment("reaching state \'S145\'");
                bool temp639;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp639);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp639);
                this.Manager.Comment("reaching state \'S346\'");
                int temp642 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetClientAccountTypeChecker5)));
                if ((temp642 == 0)) {
                    this.Manager.Comment("reaching state \'S558\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp640;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp640 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S960\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp640, "return of NetrLogonControl, state S960");
                    this.Manager.Comment("reaching state \'S1350\'");
                    goto label182;
                }
                if ((temp642 == 1)) {
                    this.Manager.Comment("reaching state \'S559\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp641;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp641 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S961\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp641, "return of NetrLogonControl, state S961");
                    this.Manager.Comment("reaching state \'S1351\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S344");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S344");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S345");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S345");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S346");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS6GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S346");
        }
        #endregion
        
        #region Test Starting in S60
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60");
            this.Manager.Comment("reaching state \'S60\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp644;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp644);
            this.Manager.AddReturn(GetPlatformInfo, null, temp644);
            this.Manager.Comment("reaching state \'S61\'");
            int temp657 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetPlatformChecker2)));
            if ((temp657 == 0)) {
                this.Manager.Comment("reaching state \'S224\'");
                bool temp645;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp645);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp645);
                this.Manager.Comment("reaching state \'S425\'");
                int temp648 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetClientAccountTypeChecker1)));
                if ((temp648 == 0)) {
                    this.Manager.Comment("reaching state \'S716\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp646;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65533,1)\'");
                    temp646 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65533u, 1u);
                    this.Manager.Comment("reaching state \'S1118\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp646, "return of NetrLogonControl, state S1118");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label184;
                }
                if ((temp648 == 1)) {
                    this.Manager.Comment("reaching state \'S717\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp647;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp647 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1119\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp647, "return of NetrLogonControl, state S1119");
                    this.Manager.Comment("reaching state \'S1404\'");
                    goto label184;
                }
                throw new InvalidOperationException("never reached");
            label184:
;
                goto label187;
            }
            if ((temp657 == 1)) {
                this.Manager.Comment("reaching state \'S225\'");
                bool temp649;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp649);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp649);
                this.Manager.Comment("reaching state \'S426\'");
                int temp652 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetClientAccountTypeChecker3)));
                if ((temp652 == 0)) {
                    this.Manager.Comment("reaching state \'S718\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp650;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65533,1)\'");
                    temp650 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65533u, 1u);
                    this.Manager.Comment("reaching state \'S1120\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp650, "return of NetrLogonControl, state S1120");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label185;
                }
                if ((temp652 == 1)) {
                    this.Manager.Comment("reaching state \'S719\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp651;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,7,1)\'");
                    temp651 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 7u, 1u);
                    this.Manager.Comment("reaching state \'S1121\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp651, "return of NetrLogonControl, state S1121");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label185;
                }
                throw new InvalidOperationException("never reached");
            label185:
;
                goto label187;
            }
            if ((temp657 == 2)) {
                this.Manager.Comment("reaching state \'S226\'");
                bool temp653;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp653);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp653);
                this.Manager.Comment("reaching state \'S427\'");
                int temp656 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetClientAccountTypeChecker5)));
                if ((temp656 == 0)) {
                    this.Manager.Comment("reaching state \'S720\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp654;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp654 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1122\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp654, "return of NetrLogonControl, state S1122");
                    this.Manager.Comment("reaching state \'S1405\'");
                    goto label186;
                }
                if ((temp656 == 1)) {
                    this.Manager.Comment("reaching state \'S721\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp655;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp655 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1123\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp655, "return of NetrLogonControl, state S1123");
                    this.Manager.Comment("reaching state \'S1406\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S425");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S425");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S426");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S426");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S427");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS60GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S427");
        }
        #endregion
        
        #region Test Starting in S62
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62");
            this.Manager.Comment("reaching state \'S62\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp658;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp658);
            this.Manager.AddReturn(GetPlatformInfo, null, temp658);
            this.Manager.Comment("reaching state \'S63\'");
            int temp671 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetPlatformChecker2)));
            if ((temp671 == 0)) {
                this.Manager.Comment("reaching state \'S227\'");
                bool temp659;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp659);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp659);
                this.Manager.Comment("reaching state \'S428\'");
                int temp662 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetClientAccountTypeChecker1)));
                if ((temp662 == 0)) {
                    this.Manager.Comment("reaching state \'S722\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp660;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65534,1)\'");
                    temp660 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65534u, 1u);
                    this.Manager.Comment("reaching state \'S1124\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp660, "return of NetrLogonControl, state S1124");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label188;
                }
                if ((temp662 == 1)) {
                    this.Manager.Comment("reaching state \'S723\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp661;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp661 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1125\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp661, "return of NetrLogonControl, state S1125");
                    this.Manager.Comment("reaching state \'S1407\'");
                    goto label188;
                }
                throw new InvalidOperationException("never reached");
            label188:
;
                goto label191;
            }
            if ((temp671 == 1)) {
                this.Manager.Comment("reaching state \'S228\'");
                bool temp663;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp663);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp663);
                this.Manager.Comment("reaching state \'S429\'");
                int temp666 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetClientAccountTypeChecker3)));
                if ((temp666 == 0)) {
                    this.Manager.Comment("reaching state \'S724\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp664;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65534,1)\'");
                    temp664 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65534u, 1u);
                    this.Manager.Comment("reaching state \'S1126\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp664, "return of NetrLogonControl, state S1126");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label189;
                }
                if ((temp666 == 1)) {
                    this.Manager.Comment("reaching state \'S725\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp665;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65535,1)\'");
                    temp665 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1127\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp665, "return of NetrLogonControl, state S1127");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label189;
                }
                throw new InvalidOperationException("never reached");
            label189:
;
                goto label191;
            }
            if ((temp671 == 2)) {
                this.Manager.Comment("reaching state \'S229\'");
                bool temp667;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp667);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp667);
                this.Manager.Comment("reaching state \'S430\'");
                int temp670 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetClientAccountTypeChecker5)));
                if ((temp670 == 0)) {
                    this.Manager.Comment("reaching state \'S726\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp668;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp668 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1128\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp668, "return of NetrLogonControl, state S1128");
                    this.Manager.Comment("reaching state \'S1408\'");
                    goto label190;
                }
                if ((temp670 == 1)) {
                    this.Manager.Comment("reaching state \'S727\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp669;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp669 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1129\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp669, "return of NetrLogonControl, state S1129");
                    this.Manager.Comment("reaching state \'S1409\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S428");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S428");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S429");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S429");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S430");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS62GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S430");
        }
        #endregion
        
        #region Test Starting in S64
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64");
            this.Manager.Comment("reaching state \'S64\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp672;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp672);
            this.Manager.AddReturn(GetPlatformInfo, null, temp672);
            this.Manager.Comment("reaching state \'S65\'");
            int temp685 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetPlatformChecker2)));
            if ((temp685 == 0)) {
                this.Manager.Comment("reaching state \'S230\'");
                bool temp673;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp673);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp673);
                this.Manager.Comment("reaching state \'S431\'");
                int temp676 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetClientAccountTypeChecker1)));
                if ((temp676 == 0)) {
                    this.Manager.Comment("reaching state \'S728\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp674;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65535,1)\'");
                    temp674 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65535u, 1u);
                    this.Manager.Comment("reaching state \'S1130\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp674, "return of NetrLogonControl, state S1130");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label192;
                }
                if ((temp676 == 1)) {
                    this.Manager.Comment("reaching state \'S729\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp675;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp675 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1131\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp675, "return of NetrLogonControl, state S1131");
                    this.Manager.Comment("reaching state \'S1410\'");
                    goto label192;
                }
                throw new InvalidOperationException("never reached");
            label192:
;
                goto label195;
            }
            if ((temp685 == 1)) {
                this.Manager.Comment("reaching state \'S231\'");
                bool temp677;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp677);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp677);
                this.Manager.Comment("reaching state \'S432\'");
                int temp680 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetClientAccountTypeChecker3)));
                if ((temp680 == 0)) {
                    this.Manager.Comment("reaching state \'S730\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp678;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,65535,1)\'");
                    temp678 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 65535u, 1u);
                    this.Manager.Comment("reaching state \'S1132\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp678, "return of NetrLogonControl, state S1132");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label193;
                }
                if ((temp680 == 1)) {
                    this.Manager.Comment("reaching state \'S731\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp679;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,11,1)\'");
                    temp679 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 11u, 1u);
                    this.Manager.Comment("reaching state \'S1133\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp679, "return of NetrLogonControl, state S1133");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label193;
                }
                throw new InvalidOperationException("never reached");
            label193:
;
                goto label195;
            }
            if ((temp685 == 2)) {
                this.Manager.Comment("reaching state \'S232\'");
                bool temp681;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp681);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp681);
                this.Manager.Comment("reaching state \'S433\'");
                int temp684 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetClientAccountTypeChecker5)));
                if ((temp684 == 0)) {
                    this.Manager.Comment("reaching state \'S732\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp682;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp682 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1134\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp682, "return of NetrLogonControl, state S1134");
                    this.Manager.Comment("reaching state \'S1411\'");
                    goto label194;
                }
                if ((temp684 == 1)) {
                    this.Manager.Comment("reaching state \'S733\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp683;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp683 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1135\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp683, "return of NetrLogonControl, state S1135");
                    this.Manager.Comment("reaching state \'S1412\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S431");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S431");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S432");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S432");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S433");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS64GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S433");
        }
        #endregion
        
        #region Test Starting in S66
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66");
            this.Manager.Comment("reaching state \'S66\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp686;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp686);
            this.Manager.AddReturn(GetPlatformInfo, null, temp686);
            this.Manager.Comment("reaching state \'S67\'");
            int temp699 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetPlatformChecker2)));
            if ((temp699 == 0)) {
                this.Manager.Comment("reaching state \'S233\'");
                bool temp687;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp687);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp687);
                this.Manager.Comment("reaching state \'S434\'");
                int temp690 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetClientAccountTypeChecker1)));
                if ((temp690 == 0)) {
                    this.Manager.Comment("reaching state \'S734\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp688;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,2)\'");
                    temp688 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 2u);
                    this.Manager.Comment("reaching state \'S1136\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp688, "return of NetrLogonControl, state S1136");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label196;
                }
                if ((temp690 == 1)) {
                    this.Manager.Comment("reaching state \'S735\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp689;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp689 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1137\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp689, "return of NetrLogonControl, state S1137");
                    this.Manager.Comment("reaching state \'S1413\'");
                    goto label196;
                }
                throw new InvalidOperationException("never reached");
            label196:
;
                goto label199;
            }
            if ((temp699 == 1)) {
                this.Manager.Comment("reaching state \'S234\'");
                bool temp691;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp691);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp691);
                this.Manager.Comment("reaching state \'S435\'");
                int temp694 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetClientAccountTypeChecker3)));
                if ((temp694 == 0)) {
                    this.Manager.Comment("reaching state \'S736\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp692;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,2)\'");
                    temp692 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 2u);
                    this.Manager.Comment("reaching state \'S1138\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp692, "return of NetrLogonControl, state S1138");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label197;
                }
                if ((temp694 == 1)) {
                    this.Manager.Comment("reaching state \'S737\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp693;
                    this.Manager.Comment("executing step \'call NetrLogonControl(PrimaryDc,1,2)\'");
                    temp693 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 2u);
                    this.Manager.Checkpoint("MS-NRPC_R104020");
                    this.Manager.Comment("reaching state \'S1139\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp693, "return of NetrLogonControl, state S1139");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label197;
                }
                throw new InvalidOperationException("never reached");
            label197:
;
                goto label199;
            }
            if ((temp699 == 2)) {
                this.Manager.Comment("reaching state \'S235\'");
                bool temp695;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp695);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp695);
                this.Manager.Comment("reaching state \'S436\'");
                int temp698 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetClientAccountTypeChecker5)));
                if ((temp698 == 0)) {
                    this.Manager.Comment("reaching state \'S738\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp696;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp696 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1140\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp696, "return of NetrLogonControl, state S1140");
                    this.Manager.Comment("reaching state \'S1414\'");
                    goto label198;
                }
                if ((temp698 == 1)) {
                    this.Manager.Comment("reaching state \'S739\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp697;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp697 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1141\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp697, "return of NetrLogonControl, state S1141");
                    this.Manager.Comment("reaching state \'S1415\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S434");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S434");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S435");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S435");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S436");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS66GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S436");
        }
        #endregion
        
        #region Test Starting in S68
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68");
            this.Manager.Comment("reaching state \'S68\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp700;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp700);
            this.Manager.AddReturn(GetPlatformInfo, null, temp700);
            this.Manager.Comment("reaching state \'S69\'");
            int temp713 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetPlatformChecker2)));
            if ((temp713 == 0)) {
                this.Manager.Comment("reaching state \'S236\'");
                bool temp701;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp701);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp701);
                this.Manager.Comment("reaching state \'S437\'");
                int temp704 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetClientAccountTypeChecker1)));
                if ((temp704 == 0)) {
                    this.Manager.Comment("reaching state \'S740\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp702;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp702 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1142\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp702, "return of NetrLogonControl, state S1142");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label200;
                }
                if ((temp704 == 1)) {
                    this.Manager.Comment("reaching state \'S741\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp703;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp703 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S1143\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp703, "return of NetrLogonControl, state S1143");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label200;
                }
                throw new InvalidOperationException("never reached");
            label200:
;
                goto label203;
            }
            if ((temp713 == 1)) {
                this.Manager.Comment("reaching state \'S237\'");
                bool temp705;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp705);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp705);
                this.Manager.Comment("reaching state \'S438\'");
                int temp708 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetClientAccountTypeChecker3)));
                if ((temp708 == 0)) {
                    this.Manager.Comment("reaching state \'S742\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp706;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp706 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1144\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp706, "return of NetrLogonControl, state S1144");
                    this.Manager.Comment("reaching state \'S1416\'");
                    goto label201;
                }
                if ((temp708 == 1)) {
                    this.Manager.Comment("reaching state \'S743\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp707;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp707 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1145\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp707, "return of NetrLogonControl, state S1145");
                    this.Manager.Comment("reaching state \'S1417\'");
                    goto label201;
                }
                throw new InvalidOperationException("never reached");
            label201:
;
                goto label203;
            }
            if ((temp713 == 2)) {
                this.Manager.Comment("reaching state \'S238\'");
                bool temp709;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp709);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp709);
                this.Manager.Comment("reaching state \'S439\'");
                int temp712 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetClientAccountTypeChecker5)));
                if ((temp712 == 0)) {
                    this.Manager.Comment("reaching state \'S744\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp710;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp710 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1146\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp710, "return of NetrLogonControl, state S1146");
                    this.Manager.Comment("reaching state \'S1418\'");
                    goto label202;
                }
                if ((temp712 == 1)) {
                    this.Manager.Comment("reaching state \'S745\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp711;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp711 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1147\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp711, "return of NetrLogonControl, state S1147");
                    this.Manager.Comment("reaching state \'S1419\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S437");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S437");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S438");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S438");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S439");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS68GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S439");
        }
        #endregion
        
        #region Test Starting in S70
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70");
            this.Manager.Comment("reaching state \'S70\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp714;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp714);
            this.Manager.AddReturn(GetPlatformInfo, null, temp714);
            this.Manager.Comment("reaching state \'S71\'");
            int temp727 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetPlatformChecker2)));
            if ((temp727 == 0)) {
                this.Manager.Comment("reaching state \'S239\'");
                bool temp715;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp715);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp715);
                this.Manager.Comment("reaching state \'S440\'");
                int temp718 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetClientAccountTypeChecker1)));
                if ((temp718 == 0)) {
                    this.Manager.Comment("reaching state \'S746\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp716;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,2,1)\'");
                    temp716 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 2u, 1u);
                    this.Manager.Comment("reaching state \'S1148\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp716, "return of NetrLogonControl, state S1148");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label204;
                }
                if ((temp718 == 1)) {
                    this.Manager.Comment("reaching state \'S747\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp717;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,4,1)\'");
                    temp717 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 4u, 1u);
                    this.Manager.Comment("reaching state \'S1149\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp717, "return of NetrLogonControl, state S1149");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label204;
                }
                throw new InvalidOperationException("never reached");
            label204:
;
                goto label207;
            }
            if ((temp727 == 1)) {
                this.Manager.Comment("reaching state \'S240\'");
                bool temp719;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp719);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp719);
                this.Manager.Comment("reaching state \'S441\'");
                int temp722 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetClientAccountTypeChecker3)));
                if ((temp722 == 0)) {
                    this.Manager.Comment("reaching state \'S748\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp720;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp720 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1150\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp720, "return of NetrLogonControl, state S1150");
                    this.Manager.Comment("reaching state \'S1420\'");
                    goto label205;
                }
                if ((temp722 == 1)) {
                    this.Manager.Comment("reaching state \'S749\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp721;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp721 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1151\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp721, "return of NetrLogonControl, state S1151");
                    this.Manager.Comment("reaching state \'S1421\'");
                    goto label205;
                }
                throw new InvalidOperationException("never reached");
            label205:
;
                goto label207;
            }
            if ((temp727 == 2)) {
                this.Manager.Comment("reaching state \'S241\'");
                bool temp723;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp723);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp723);
                this.Manager.Comment("reaching state \'S442\'");
                int temp726 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetClientAccountTypeChecker5)));
                if ((temp726 == 0)) {
                    this.Manager.Comment("reaching state \'S750\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp724;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp724 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1152\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp724, "return of NetrLogonControl, state S1152");
                    this.Manager.Comment("reaching state \'S1422\'");
                    goto label206;
                }
                if ((temp726 == 1)) {
                    this.Manager.Comment("reaching state \'S751\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp725;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp725 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1153\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp725, "return of NetrLogonControl, state S1153");
                    this.Manager.Comment("reaching state \'S1423\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S440");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S440");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S441");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S441");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S442");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS70GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S442");
        }
        #endregion
        
        #region Test Starting in S72
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72");
            this.Manager.Comment("reaching state \'S72\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp728;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp728);
            this.Manager.AddReturn(GetPlatformInfo, null, temp728);
            this.Manager.Comment("reaching state \'S73\'");
            int temp741 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetPlatformChecker2)));
            if ((temp741 == 0)) {
                this.Manager.Comment("reaching state \'S242\'");
                bool temp729;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp729);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp729);
                this.Manager.Comment("reaching state \'S443\'");
                int temp732 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetClientAccountTypeChecker1)));
                if ((temp732 == 0)) {
                    this.Manager.Comment("reaching state \'S752\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp730;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,3,1)\'");
                    temp730 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 3u, 1u);
                    this.Manager.Comment("reaching state \'S1154\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp730, "return of NetrLogonControl, state S1154");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label208;
                }
                if ((temp732 == 1)) {
                    this.Manager.Comment("reaching state \'S753\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp731;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,2,1)\'");
                    temp731 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 2u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104925");
                    this.Manager.Comment("reaching state \'S1155\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp731, "return of NetrLogonControl, state S1155");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label208;
                }
                throw new InvalidOperationException("never reached");
            label208:
;
                goto label211;
            }
            if ((temp741 == 1)) {
                this.Manager.Comment("reaching state \'S243\'");
                bool temp733;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp733);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp733);
                this.Manager.Comment("reaching state \'S444\'");
                int temp736 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetClientAccountTypeChecker3)));
                if ((temp736 == 0)) {
                    this.Manager.Comment("reaching state \'S754\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp734;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp734 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1156\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp734, "return of NetrLogonControl, state S1156");
                    this.Manager.Comment("reaching state \'S1424\'");
                    goto label209;
                }
                if ((temp736 == 1)) {
                    this.Manager.Comment("reaching state \'S755\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp735;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp735 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1157\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp735, "return of NetrLogonControl, state S1157");
                    this.Manager.Comment("reaching state \'S1425\'");
                    goto label209;
                }
                throw new InvalidOperationException("never reached");
            label209:
;
                goto label211;
            }
            if ((temp741 == 2)) {
                this.Manager.Comment("reaching state \'S244\'");
                bool temp737;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp737);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp737);
                this.Manager.Comment("reaching state \'S445\'");
                int temp740 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetClientAccountTypeChecker5)));
                if ((temp740 == 0)) {
                    this.Manager.Comment("reaching state \'S756\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp738;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp738 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1158\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp738, "return of NetrLogonControl, state S1158");
                    this.Manager.Comment("reaching state \'S1426\'");
                    goto label210;
                }
                if ((temp740 == 1)) {
                    this.Manager.Comment("reaching state \'S757\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp739;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp739 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1159\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp739, "return of NetrLogonControl, state S1159");
                    this.Manager.Comment("reaching state \'S1427\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S443");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S443");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S444");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S444");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S445");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS72GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S445");
        }
        #endregion
        
        #region Test Starting in S74
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74");
            this.Manager.Comment("reaching state \'S74\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp742;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp742);
            this.Manager.AddReturn(GetPlatformInfo, null, temp742);
            this.Manager.Comment("reaching state \'S75\'");
            int temp755 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetPlatformChecker2)));
            if ((temp755 == 0)) {
                this.Manager.Comment("reaching state \'S245\'");
                bool temp743;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp743);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp743);
                this.Manager.Comment("reaching state \'S446\'");
                int temp746 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetClientAccountTypeChecker1)));
                if ((temp746 == 0)) {
                    this.Manager.Comment("reaching state \'S758\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp744;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,4,1)\'");
                    temp744 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 4u, 1u);
                    this.Manager.Comment("reaching state \'S1160\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp744, "return of NetrLogonControl, state S1160");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label212;
                }
                if ((temp746 == 1)) {
                    this.Manager.Comment("reaching state \'S759\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp745;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,6,1)\'");
                    temp745 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 6u, 1u);
                    this.Manager.Comment("reaching state \'S1161\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp745, "return of NetrLogonControl, state S1161");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label212;
                }
                throw new InvalidOperationException("never reached");
            label212:
;
                goto label215;
            }
            if ((temp755 == 1)) {
                this.Manager.Comment("reaching state \'S246\'");
                bool temp747;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp747);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp747);
                this.Manager.Comment("reaching state \'S447\'");
                int temp750 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetClientAccountTypeChecker3)));
                if ((temp750 == 0)) {
                    this.Manager.Comment("reaching state \'S760\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp748;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp748 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1162\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp748, "return of NetrLogonControl, state S1162");
                    this.Manager.Comment("reaching state \'S1428\'");
                    goto label213;
                }
                if ((temp750 == 1)) {
                    this.Manager.Comment("reaching state \'S761\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp749;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp749 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1163\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp749, "return of NetrLogonControl, state S1163");
                    this.Manager.Comment("reaching state \'S1429\'");
                    goto label213;
                }
                throw new InvalidOperationException("never reached");
            label213:
;
                goto label215;
            }
            if ((temp755 == 2)) {
                this.Manager.Comment("reaching state \'S247\'");
                bool temp751;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp751);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp751);
                this.Manager.Comment("reaching state \'S448\'");
                int temp754 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetClientAccountTypeChecker5)));
                if ((temp754 == 0)) {
                    this.Manager.Comment("reaching state \'S762\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp752;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp752 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1164\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp752, "return of NetrLogonControl, state S1164");
                    this.Manager.Comment("reaching state \'S1430\'");
                    goto label214;
                }
                if ((temp754 == 1)) {
                    this.Manager.Comment("reaching state \'S763\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp753;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp753 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1165\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp753, "return of NetrLogonControl, state S1165");
                    this.Manager.Comment("reaching state \'S1431\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S446");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S446");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S447");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S447");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S448");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS74GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S448");
        }
        #endregion
        
        #region Test Starting in S76
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76");
            this.Manager.Comment("reaching state \'S76\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp756;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp756);
            this.Manager.AddReturn(GetPlatformInfo, null, temp756);
            this.Manager.Comment("reaching state \'S77\'");
            int temp769 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetPlatformChecker2)));
            if ((temp769 == 0)) {
                this.Manager.Comment("reaching state \'S248\'");
                bool temp757;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp757);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp757);
                this.Manager.Comment("reaching state \'S449\'");
                int temp760 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetClientAccountTypeChecker1)));
                if ((temp760 == 0)) {
                    this.Manager.Comment("reaching state \'S764\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp758;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,5,1)\'");
                    temp758 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 5u, 1u);
                    this.Manager.Comment("reaching state \'S1166\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp758, "return of NetrLogonControl, state S1166");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label216;
                }
                if ((temp760 == 1)) {
                    this.Manager.Comment("reaching state \'S765\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp759;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,7,1)\'");
                    temp759 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 7u, 1u);
                    this.Manager.Comment("reaching state \'S1167\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp759, "return of NetrLogonControl, state S1167");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label216;
                }
                throw new InvalidOperationException("never reached");
            label216:
;
                goto label219;
            }
            if ((temp769 == 1)) {
                this.Manager.Comment("reaching state \'S249\'");
                bool temp761;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp761);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp761);
                this.Manager.Comment("reaching state \'S450\'");
                int temp764 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetClientAccountTypeChecker3)));
                if ((temp764 == 0)) {
                    this.Manager.Comment("reaching state \'S766\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp762;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp762 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1168\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp762, "return of NetrLogonControl, state S1168");
                    this.Manager.Comment("reaching state \'S1432\'");
                    goto label217;
                }
                if ((temp764 == 1)) {
                    this.Manager.Comment("reaching state \'S767\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp763;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp763 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1169\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp763, "return of NetrLogonControl, state S1169");
                    this.Manager.Comment("reaching state \'S1433\'");
                    goto label217;
                }
                throw new InvalidOperationException("never reached");
            label217:
;
                goto label219;
            }
            if ((temp769 == 2)) {
                this.Manager.Comment("reaching state \'S250\'");
                bool temp765;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp765);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp765);
                this.Manager.Comment("reaching state \'S451\'");
                int temp768 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetClientAccountTypeChecker5)));
                if ((temp768 == 0)) {
                    this.Manager.Comment("reaching state \'S768\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp766;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp766 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1170\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp766, "return of NetrLogonControl, state S1170");
                    this.Manager.Comment("reaching state \'S1434\'");
                    goto label218;
                }
                if ((temp768 == 1)) {
                    this.Manager.Comment("reaching state \'S769\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp767;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp767 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1171\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp767, "return of NetrLogonControl, state S1171");
                    this.Manager.Comment("reaching state \'S1435\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S449");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S449");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S450");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S450");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S451");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS76GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S451");
        }
        #endregion
        
        #region Test Starting in S78
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78");
            this.Manager.Comment("reaching state \'S78\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp770;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp770);
            this.Manager.AddReturn(GetPlatformInfo, null, temp770);
            this.Manager.Comment("reaching state \'S79\'");
            int temp783 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetPlatformChecker2)));
            if ((temp783 == 0)) {
                this.Manager.Comment("reaching state \'S251\'");
                bool temp771;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp771);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp771);
                this.Manager.Comment("reaching state \'S452\'");
                int temp774 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetClientAccountTypeChecker1)));
                if ((temp774 == 0)) {
                    this.Manager.Comment("reaching state \'S770\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp772;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,6,1)\'");
                    temp772 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 6u, 1u);
                    this.Manager.Comment("reaching state \'S1172\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp772, "return of NetrLogonControl, state S1172");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label220;
                }
                if ((temp774 == 1)) {
                    this.Manager.Comment("reaching state \'S771\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp773;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp773 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1173\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp773, "return of NetrLogonControl, state S1173");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label220;
                }
                throw new InvalidOperationException("never reached");
            label220:
;
                goto label223;
            }
            if ((temp783 == 1)) {
                this.Manager.Comment("reaching state \'S252\'");
                bool temp775;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp775);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp775);
                this.Manager.Comment("reaching state \'S453\'");
                int temp778 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetClientAccountTypeChecker3)));
                if ((temp778 == 0)) {
                    this.Manager.Comment("reaching state \'S772\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp776;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp776 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1174\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp776, "return of NetrLogonControl, state S1174");
                    this.Manager.Comment("reaching state \'S1436\'");
                    goto label221;
                }
                if ((temp778 == 1)) {
                    this.Manager.Comment("reaching state \'S773\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp777;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp777 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1175\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp777, "return of NetrLogonControl, state S1175");
                    this.Manager.Comment("reaching state \'S1437\'");
                    goto label221;
                }
                throw new InvalidOperationException("never reached");
            label221:
;
                goto label223;
            }
            if ((temp783 == 2)) {
                this.Manager.Comment("reaching state \'S253\'");
                bool temp779;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp779);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp779);
                this.Manager.Comment("reaching state \'S454\'");
                int temp782 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetClientAccountTypeChecker5)));
                if ((temp782 == 0)) {
                    this.Manager.Comment("reaching state \'S774\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp780;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp780 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1176\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp780, "return of NetrLogonControl, state S1176");
                    this.Manager.Comment("reaching state \'S1438\'");
                    goto label222;
                }
                if ((temp782 == 1)) {
                    this.Manager.Comment("reaching state \'S775\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp781;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp781 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1177\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp781, "return of NetrLogonControl, state S1177");
                    this.Manager.Comment("reaching state \'S1439\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S452");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S452");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S453");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S453");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S454");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS78GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S454");
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp784;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp784);
            this.Manager.AddReturn(GetPlatformInfo, null, temp784);
            this.Manager.Comment("reaching state \'S9\'");
            int temp797 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetPlatformChecker2)));
            if ((temp797 == 0)) {
                this.Manager.Comment("reaching state \'S146\'");
                bool temp785;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp785);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp785);
                this.Manager.Comment("reaching state \'S347\'");
                int temp788 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetClientAccountTypeChecker1)));
                if ((temp788 == 0)) {
                    this.Manager.Comment("reaching state \'S560\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp786;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,4,1)\'");
                    temp786 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 4u, 1u);
                    this.Manager.Comment("reaching state \'S962\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp786, "return of NetrLogonControl, state S962");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1345();
                    goto label224;
                }
                if ((temp788 == 1)) {
                    this.Manager.Comment("reaching state \'S561\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp787;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,12,1)\'");
                    temp787 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 12u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S963\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp787, "return of NetrLogonControl, state S963");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1344();
                    goto label224;
                }
                throw new InvalidOperationException("never reached");
            label224:
;
                goto label227;
            }
            if ((temp797 == 1)) {
                this.Manager.Comment("reaching state \'S147\'");
                bool temp789;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp789);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp789);
                this.Manager.Comment("reaching state \'S348\'");
                int temp792 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetClientAccountTypeChecker3)));
                if ((temp792 == 0)) {
                    this.Manager.Comment("reaching state \'S562\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp790;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,4,1)\'");
                    temp790 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 4u, 1u);
                    this.Manager.Comment("reaching state \'S964\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp790, "return of NetrLogonControl, state S964");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1343();
                    goto label225;
                }
                if ((temp792 == 1)) {
                    this.Manager.Comment("reaching state \'S563\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp791;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,3,1)\'");
                    temp791 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 3u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S965\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp791, "return of NetrLogonControl, state S965");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1342();
                    goto label225;
                }
                throw new InvalidOperationException("never reached");
            label225:
;
                goto label227;
            }
            if ((temp797 == 2)) {
                this.Manager.Comment("reaching state \'S148\'");
                bool temp793;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp793);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp793);
                this.Manager.Comment("reaching state \'S349\'");
                int temp796 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetClientAccountTypeChecker5)));
                if ((temp796 == 0)) {
                    this.Manager.Comment("reaching state \'S564\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp794;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp794 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R103964");
                    this.Manager.Checkpoint("MS-NRPC_R104216");
                    this.Manager.Comment("reaching state \'S966\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp794, "return of NetrLogonControl, state S966");
                    this.Manager.Comment("reaching state \'S1352\'");
                    goto label226;
                }
                if ((temp796 == 1)) {
                    this.Manager.Comment("reaching state \'S565\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp795;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp795 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S967\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp795, "return of NetrLogonControl, state S967");
                    this.Manager.Comment("reaching state \'S1353\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S347");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S347");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S348");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S348");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S349");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS8GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S349");
        }
        #endregion
        
        #region Test Starting in S80
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80");
            this.Manager.Comment("reaching state \'S80\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp798;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp798);
            this.Manager.AddReturn(GetPlatformInfo, null, temp798);
            this.Manager.Comment("reaching state \'S81\'");
            int temp811 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetPlatformChecker2)));
            if ((temp811 == 0)) {
                this.Manager.Comment("reaching state \'S254\'");
                bool temp799;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp799);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp799);
                this.Manager.Comment("reaching state \'S455\'");
                int temp802 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetClientAccountTypeChecker1)));
                if ((temp802 == 0)) {
                    this.Manager.Comment("reaching state \'S776\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp800;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,7,1)\'");
                    temp800 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 7u, 1u);
                    this.Manager.Comment("reaching state \'S1178\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp800, "return of NetrLogonControl, state S1178");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label228;
                }
                if ((temp802 == 1)) {
                    this.Manager.Comment("reaching state \'S777\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp801;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,3,1)\'");
                    temp801 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 3u, 1u);
                    this.Manager.Comment("reaching state \'S1179\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp801, "return of NetrLogonControl, state S1179");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label228;
                }
                throw new InvalidOperationException("never reached");
            label228:
;
                goto label231;
            }
            if ((temp811 == 1)) {
                this.Manager.Comment("reaching state \'S255\'");
                bool temp803;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp803);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp803);
                this.Manager.Comment("reaching state \'S456\'");
                int temp806 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetClientAccountTypeChecker3)));
                if ((temp806 == 0)) {
                    this.Manager.Comment("reaching state \'S778\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp804;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp804 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1180\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp804, "return of NetrLogonControl, state S1180");
                    this.Manager.Comment("reaching state \'S1440\'");
                    goto label229;
                }
                if ((temp806 == 1)) {
                    this.Manager.Comment("reaching state \'S779\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp805;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp805 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1181\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp805, "return of NetrLogonControl, state S1181");
                    this.Manager.Comment("reaching state \'S1441\'");
                    goto label229;
                }
                throw new InvalidOperationException("never reached");
            label229:
;
                goto label231;
            }
            if ((temp811 == 2)) {
                this.Manager.Comment("reaching state \'S256\'");
                bool temp807;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp807);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp807);
                this.Manager.Comment("reaching state \'S457\'");
                int temp810 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetClientAccountTypeChecker5)));
                if ((temp810 == 0)) {
                    this.Manager.Comment("reaching state \'S780\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp808;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp808 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1182\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp808, "return of NetrLogonControl, state S1182");
                    this.Manager.Comment("reaching state \'S1442\'");
                    goto label230;
                }
                if ((temp810 == 1)) {
                    this.Manager.Comment("reaching state \'S781\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp809;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp809 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1183\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp809, "return of NetrLogonControl, state S1183");
                    this.Manager.Comment("reaching state \'S1443\'");
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
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S455");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S455");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S456");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S456");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S457");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS80GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S457");
        }
        #endregion
        
        #region Test Starting in S82
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82");
            this.Manager.Comment("reaching state \'S82\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp812;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp812);
            this.Manager.AddReturn(GetPlatformInfo, null, temp812);
            this.Manager.Comment("reaching state \'S83\'");
            int temp825 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetPlatformChecker2)));
            if ((temp825 == 0)) {
                this.Manager.Comment("reaching state \'S257\'");
                bool temp813;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp813);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp813);
                this.Manager.Comment("reaching state \'S458\'");
                int temp816 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetClientAccountTypeChecker1)));
                if ((temp816 == 0)) {
                    this.Manager.Comment("reaching state \'S782\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp814;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,8,1)\'");
                    temp814 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 8u, 1u);
                    this.Manager.Comment("reaching state \'S1184\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp814, "return of NetrLogonControl, state S1184");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label232;
                }
                if ((temp816 == 1)) {
                    this.Manager.Comment("reaching state \'S783\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp815;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,5,1)\'");
                    temp815 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 5u, 1u);
                    this.Manager.Comment("reaching state \'S1185\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp815, "return of NetrLogonControl, state S1185");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label232;
                }
                throw new InvalidOperationException("never reached");
            label232:
;
                goto label235;
            }
            if ((temp825 == 1)) {
                this.Manager.Comment("reaching state \'S258\'");
                bool temp817;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp817);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp817);
                this.Manager.Comment("reaching state \'S459\'");
                int temp820 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetClientAccountTypeChecker3)));
                if ((temp820 == 0)) {
                    this.Manager.Comment("reaching state \'S784\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp818;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp818 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1186\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp818, "return of NetrLogonControl, state S1186");
                    this.Manager.Comment("reaching state \'S1444\'");
                    goto label233;
                }
                if ((temp820 == 1)) {
                    this.Manager.Comment("reaching state \'S785\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp819;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp819 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1187\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp819, "return of NetrLogonControl, state S1187");
                    this.Manager.Comment("reaching state \'S1445\'");
                    goto label233;
                }
                throw new InvalidOperationException("never reached");
            label233:
;
                goto label235;
            }
            if ((temp825 == 2)) {
                this.Manager.Comment("reaching state \'S259\'");
                bool temp821;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp821);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp821);
                this.Manager.Comment("reaching state \'S460\'");
                int temp824 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetClientAccountTypeChecker5)));
                if ((temp824 == 0)) {
                    this.Manager.Comment("reaching state \'S786\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp822;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp822 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1188\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp822, "return of NetrLogonControl, state S1188");
                    this.Manager.Comment("reaching state \'S1446\'");
                    goto label234;
                }
                if ((temp824 == 1)) {
                    this.Manager.Comment("reaching state \'S787\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp823;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp823 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1189\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp823, "return of NetrLogonControl, state S1189");
                    this.Manager.Comment("reaching state \'S1447\'");
                    goto label234;
                }
                throw new InvalidOperationException("never reached");
            label234:
;
                goto label235;
            }
            throw new InvalidOperationException("never reached");
        label235:
;
            this.Manager.EndTest();
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S458");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S458");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S459");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S459");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S460");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS82GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S460");
        }
        #endregion
        
        #region Test Starting in S84
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84");
            this.Manager.Comment("reaching state \'S84\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp826;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp826);
            this.Manager.AddReturn(GetPlatformInfo, null, temp826);
            this.Manager.Comment("reaching state \'S85\'");
            int temp839 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetPlatformChecker2)));
            if ((temp839 == 0)) {
                this.Manager.Comment("reaching state \'S260\'");
                bool temp827;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp827);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp827);
                this.Manager.Comment("reaching state \'S461\'");
                int temp830 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetClientAccountTypeChecker1)));
                if ((temp830 == 0)) {
                    this.Manager.Comment("reaching state \'S788\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp828;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,9,1)\'");
                    temp828 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 9u, 1u);
                    this.Manager.Comment("reaching state \'S1190\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp828, "return of NetrLogonControl, state S1190");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label236;
                }
                if ((temp830 == 1)) {
                    this.Manager.Comment("reaching state \'S789\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp829;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,9,1)\'");
                    temp829 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 9u, 1u);
                    this.Manager.Comment("reaching state \'S1191\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp829, "return of NetrLogonControl, state S1191");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label236;
                }
                throw new InvalidOperationException("never reached");
            label236:
;
                goto label239;
            }
            if ((temp839 == 1)) {
                this.Manager.Comment("reaching state \'S261\'");
                bool temp831;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp831);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp831);
                this.Manager.Comment("reaching state \'S462\'");
                int temp834 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetClientAccountTypeChecker3)));
                if ((temp834 == 0)) {
                    this.Manager.Comment("reaching state \'S790\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp832;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp832 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1192\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp832, "return of NetrLogonControl, state S1192");
                    this.Manager.Comment("reaching state \'S1448\'");
                    goto label237;
                }
                if ((temp834 == 1)) {
                    this.Manager.Comment("reaching state \'S791\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp833;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp833 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1193\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp833, "return of NetrLogonControl, state S1193");
                    this.Manager.Comment("reaching state \'S1449\'");
                    goto label237;
                }
                throw new InvalidOperationException("never reached");
            label237:
;
                goto label239;
            }
            if ((temp839 == 2)) {
                this.Manager.Comment("reaching state \'S262\'");
                bool temp835;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp835);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp835);
                this.Manager.Comment("reaching state \'S463\'");
                int temp838 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetClientAccountTypeChecker5)));
                if ((temp838 == 0)) {
                    this.Manager.Comment("reaching state \'S792\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp836;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp836 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1194\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp836, "return of NetrLogonControl, state S1194");
                    this.Manager.Comment("reaching state \'S1450\'");
                    goto label238;
                }
                if ((temp838 == 1)) {
                    this.Manager.Comment("reaching state \'S793\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp837;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp837 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1195\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp837, "return of NetrLogonControl, state S1195");
                    this.Manager.Comment("reaching state \'S1451\'");
                    goto label238;
                }
                throw new InvalidOperationException("never reached");
            label238:
;
                goto label239;
            }
            throw new InvalidOperationException("never reached");
        label239:
;
            this.Manager.EndTest();
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S461");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S461");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S462");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S462");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S463");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS84GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S463");
        }
        #endregion
        
        #region Test Starting in S86
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86");
            this.Manager.Comment("reaching state \'S86\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp840;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp840);
            this.Manager.AddReturn(GetPlatformInfo, null, temp840);
            this.Manager.Comment("reaching state \'S87\'");
            int temp853 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetPlatformChecker2)));
            if ((temp853 == 0)) {
                this.Manager.Comment("reaching state \'S263\'");
                bool temp841;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp841);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp841);
                this.Manager.Comment("reaching state \'S464\'");
                int temp844 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetClientAccountTypeChecker1)));
                if ((temp844 == 0)) {
                    this.Manager.Comment("reaching state \'S794\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp842;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,10,1)\'");
                    temp842 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 10u, 1u);
                    this.Manager.Comment("reaching state \'S1196\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp842, "return of NetrLogonControl, state S1196");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label240;
                }
                if ((temp844 == 1)) {
                    this.Manager.Comment("reaching state \'S795\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp843;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp843 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1197\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp843, "return of NetrLogonControl, state S1197");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label240;
                }
                throw new InvalidOperationException("never reached");
            label240:
;
                goto label243;
            }
            if ((temp853 == 1)) {
                this.Manager.Comment("reaching state \'S264\'");
                bool temp845;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp845);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp845);
                this.Manager.Comment("reaching state \'S465\'");
                int temp848 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetClientAccountTypeChecker3)));
                if ((temp848 == 0)) {
                    this.Manager.Comment("reaching state \'S796\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp846;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp846 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1198\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp846, "return of NetrLogonControl, state S1198");
                    this.Manager.Comment("reaching state \'S1452\'");
                    goto label241;
                }
                if ((temp848 == 1)) {
                    this.Manager.Comment("reaching state \'S797\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp847;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp847 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1199\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp847, "return of NetrLogonControl, state S1199");
                    this.Manager.Comment("reaching state \'S1453\'");
                    goto label241;
                }
                throw new InvalidOperationException("never reached");
            label241:
;
                goto label243;
            }
            if ((temp853 == 2)) {
                this.Manager.Comment("reaching state \'S265\'");
                bool temp849;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp849);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp849);
                this.Manager.Comment("reaching state \'S466\'");
                int temp852 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetClientAccountTypeChecker5)));
                if ((temp852 == 0)) {
                    this.Manager.Comment("reaching state \'S798\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp850;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp850 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1200\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp850, "return of NetrLogonControl, state S1200");
                    this.Manager.Comment("reaching state \'S1454\'");
                    goto label242;
                }
                if ((temp852 == 1)) {
                    this.Manager.Comment("reaching state \'S799\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp851;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp851 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1201\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp851, "return of NetrLogonControl, state S1201");
                    this.Manager.Comment("reaching state \'S1455\'");
                    goto label242;
                }
                throw new InvalidOperationException("never reached");
            label242:
;
                goto label243;
            }
            throw new InvalidOperationException("never reached");
        label243:
;
            this.Manager.EndTest();
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S464");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S464");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S465");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S465");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S466");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS86GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S466");
        }
        #endregion
        
        #region Test Starting in S88
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88");
            this.Manager.Comment("reaching state \'S88\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp854;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp854);
            this.Manager.AddReturn(GetPlatformInfo, null, temp854);
            this.Manager.Comment("reaching state \'S89\'");
            int temp867 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetPlatformChecker2)));
            if ((temp867 == 0)) {
                this.Manager.Comment("reaching state \'S266\'");
                bool temp855;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp855);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp855);
                this.Manager.Comment("reaching state \'S467\'");
                int temp858 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetClientAccountTypeChecker1)));
                if ((temp858 == 0)) {
                    this.Manager.Comment("reaching state \'S800\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp856;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,11,1)\'");
                    temp856 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 11u, 1u);
                    this.Manager.Comment("reaching state \'S1202\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp856, "return of NetrLogonControl, state S1202");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label244;
                }
                if ((temp858 == 1)) {
                    this.Manager.Comment("reaching state \'S801\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp857;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,11,1)\'");
                    temp857 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 11u, 1u);
                    this.Manager.Comment("reaching state \'S1203\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp857, "return of NetrLogonControl, state S1203");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label244;
                }
                throw new InvalidOperationException("never reached");
            label244:
;
                goto label247;
            }
            if ((temp867 == 1)) {
                this.Manager.Comment("reaching state \'S267\'");
                bool temp859;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp859);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp859);
                this.Manager.Comment("reaching state \'S468\'");
                int temp862 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetClientAccountTypeChecker3)));
                if ((temp862 == 0)) {
                    this.Manager.Comment("reaching state \'S802\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp860;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp860 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1204\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp860, "return of NetrLogonControl, state S1204");
                    this.Manager.Comment("reaching state \'S1456\'");
                    goto label245;
                }
                if ((temp862 == 1)) {
                    this.Manager.Comment("reaching state \'S803\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp861;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp861 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1205\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp861, "return of NetrLogonControl, state S1205");
                    this.Manager.Comment("reaching state \'S1457\'");
                    goto label245;
                }
                throw new InvalidOperationException("never reached");
            label245:
;
                goto label247;
            }
            if ((temp867 == 2)) {
                this.Manager.Comment("reaching state \'S268\'");
                bool temp863;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp863);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp863);
                this.Manager.Comment("reaching state \'S469\'");
                int temp866 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetClientAccountTypeChecker5)));
                if ((temp866 == 0)) {
                    this.Manager.Comment("reaching state \'S804\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp864;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp864 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1206\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp864, "return of NetrLogonControl, state S1206");
                    this.Manager.Comment("reaching state \'S1458\'");
                    goto label246;
                }
                if ((temp866 == 1)) {
                    this.Manager.Comment("reaching state \'S805\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp865;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp865 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1207\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp865, "return of NetrLogonControl, state S1207");
                    this.Manager.Comment("reaching state \'S1459\'");
                    goto label246;
                }
                throw new InvalidOperationException("never reached");
            label246:
;
                goto label247;
            }
            throw new InvalidOperationException("never reached");
        label247:
;
            this.Manager.EndTest();
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S467");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S467");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S468");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S468");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S469");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS88GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S469");
        }
        #endregion
        
        #region Test Starting in S90
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90");
            this.Manager.Comment("reaching state \'S90\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp868;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp868);
            this.Manager.AddReturn(GetPlatformInfo, null, temp868);
            this.Manager.Comment("reaching state \'S91\'");
            int temp881 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetPlatformChecker2)));
            if ((temp881 == 0)) {
                this.Manager.Comment("reaching state \'S269\'");
                bool temp869;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp869);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp869);
                this.Manager.Comment("reaching state \'S470\'");
                int temp872 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetClientAccountTypeChecker1)));
                if ((temp872 == 0)) {
                    this.Manager.Comment("reaching state \'S806\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp870;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,12,1)\'");
                    temp870 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 12u, 1u);
                    this.Manager.Comment("reaching state \'S1208\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp870, "return of NetrLogonControl, state S1208");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label248;
                }
                if ((temp872 == 1)) {
                    this.Manager.Comment("reaching state \'S807\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp871;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,12,1)\'");
                    temp871 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 12u, 1u);
                    this.Manager.Comment("reaching state \'S1209\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp871, "return of NetrLogonControl, state S1209");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label248;
                }
                throw new InvalidOperationException("never reached");
            label248:
;
                goto label251;
            }
            if ((temp881 == 1)) {
                this.Manager.Comment("reaching state \'S270\'");
                bool temp873;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp873);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp873);
                this.Manager.Comment("reaching state \'S471\'");
                int temp876 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetClientAccountTypeChecker3)));
                if ((temp876 == 0)) {
                    this.Manager.Comment("reaching state \'S808\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp874;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp874 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1210\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp874, "return of NetrLogonControl, state S1210");
                    this.Manager.Comment("reaching state \'S1460\'");
                    goto label249;
                }
                if ((temp876 == 1)) {
                    this.Manager.Comment("reaching state \'S809\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp875;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp875 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1211\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp875, "return of NetrLogonControl, state S1211");
                    this.Manager.Comment("reaching state \'S1461\'");
                    goto label249;
                }
                throw new InvalidOperationException("never reached");
            label249:
;
                goto label251;
            }
            if ((temp881 == 2)) {
                this.Manager.Comment("reaching state \'S271\'");
                bool temp877;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp877);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp877);
                this.Manager.Comment("reaching state \'S472\'");
                int temp880 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetClientAccountTypeChecker5)));
                if ((temp880 == 0)) {
                    this.Manager.Comment("reaching state \'S810\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp878;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp878 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1212\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp878, "return of NetrLogonControl, state S1212");
                    this.Manager.Comment("reaching state \'S1462\'");
                    goto label250;
                }
                if ((temp880 == 1)) {
                    this.Manager.Comment("reaching state \'S811\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp879;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp879 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1213\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp879, "return of NetrLogonControl, state S1213");
                    this.Manager.Comment("reaching state \'S1463\'");
                    goto label250;
                }
                throw new InvalidOperationException("never reached");
            label250:
;
                goto label251;
            }
            throw new InvalidOperationException("never reached");
        label251:
;
            this.Manager.EndTest();
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S470");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S470");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S471");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S471");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S472");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS90GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S472");
        }
        #endregion
        
        #region Test Starting in S92
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92");
            this.Manager.Comment("reaching state \'S92\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp882;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp882);
            this.Manager.AddReturn(GetPlatformInfo, null, temp882);
            this.Manager.Comment("reaching state \'S93\'");
            int temp895 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetPlatformChecker2)));
            if ((temp895 == 0)) {
                this.Manager.Comment("reaching state \'S272\'");
                bool temp883;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp883);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp883);
                this.Manager.Comment("reaching state \'S473\'");
                int temp886 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetClientAccountTypeChecker1)));
                if ((temp886 == 0)) {
                    this.Manager.Comment("reaching state \'S812\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp884;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65532,1)\'");
                    temp884 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65532u, 1u);
                    this.Manager.Comment("reaching state \'S1214\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp884, "return of NetrLogonControl, state S1214");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label252;
                }
                if ((temp886 == 1)) {
                    this.Manager.Comment("reaching state \'S813\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp885;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,8,1)\'");
                    temp885 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 8u, 1u);
                    this.Manager.Comment("reaching state \'S1215\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp885, "return of NetrLogonControl, state S1215");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label252;
                }
                throw new InvalidOperationException("never reached");
            label252:
;
                goto label255;
            }
            if ((temp895 == 1)) {
                this.Manager.Comment("reaching state \'S273\'");
                bool temp887;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp887);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp887);
                this.Manager.Comment("reaching state \'S474\'");
                int temp890 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetClientAccountTypeChecker3)));
                if ((temp890 == 0)) {
                    this.Manager.Comment("reaching state \'S814\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp888;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp888 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1216\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp888, "return of NetrLogonControl, state S1216");
                    this.Manager.Comment("reaching state \'S1464\'");
                    goto label253;
                }
                if ((temp890 == 1)) {
                    this.Manager.Comment("reaching state \'S815\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp889;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp889 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1217\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp889, "return of NetrLogonControl, state S1217");
                    this.Manager.Comment("reaching state \'S1465\'");
                    goto label253;
                }
                throw new InvalidOperationException("never reached");
            label253:
;
                goto label255;
            }
            if ((temp895 == 2)) {
                this.Manager.Comment("reaching state \'S274\'");
                bool temp891;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp891);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp891);
                this.Manager.Comment("reaching state \'S475\'");
                int temp894 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetClientAccountTypeChecker5)));
                if ((temp894 == 0)) {
                    this.Manager.Comment("reaching state \'S816\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp892;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp892 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1218\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp892, "return of NetrLogonControl, state S1218");
                    this.Manager.Comment("reaching state \'S1466\'");
                    goto label254;
                }
                if ((temp894 == 1)) {
                    this.Manager.Comment("reaching state \'S817\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp893;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp893 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1219\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp893, "return of NetrLogonControl, state S1219");
                    this.Manager.Comment("reaching state \'S1467\'");
                    goto label254;
                }
                throw new InvalidOperationException("never reached");
            label254:
;
                goto label255;
            }
            throw new InvalidOperationException("never reached");
        label255:
;
            this.Manager.EndTest();
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S473");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S473");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S474");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S474");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S475");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS92GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S475");
        }
        #endregion
        
        #region Test Starting in S94
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94");
            this.Manager.Comment("reaching state \'S94\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp896;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp896);
            this.Manager.AddReturn(GetPlatformInfo, null, temp896);
            this.Manager.Comment("reaching state \'S95\'");
            int temp909 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetPlatformChecker2)));
            if ((temp909 == 0)) {
                this.Manager.Comment("reaching state \'S275\'");
                bool temp897;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp897);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp897);
                this.Manager.Comment("reaching state \'S476\'");
                int temp900 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetClientAccountTypeChecker1)));
                if ((temp900 == 0)) {
                    this.Manager.Comment("reaching state \'S818\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp898;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp898 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Comment("reaching state \'S1220\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp898, "return of NetrLogonControl, state S1220");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label256;
                }
                if ((temp900 == 1)) {
                    this.Manager.Comment("reaching state \'S819\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp899;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,10,1)\'");
                    temp899 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 10u, 1u);
                    this.Manager.Comment("reaching state \'S1221\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp899, "return of NetrLogonControl, state S1221");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label256;
                }
                throw new InvalidOperationException("never reached");
            label256:
;
                goto label259;
            }
            if ((temp909 == 1)) {
                this.Manager.Comment("reaching state \'S276\'");
                bool temp901;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp901);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp901);
                this.Manager.Comment("reaching state \'S477\'");
                int temp904 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetClientAccountTypeChecker3)));
                if ((temp904 == 0)) {
                    this.Manager.Comment("reaching state \'S820\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp902;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp902 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1222\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp902, "return of NetrLogonControl, state S1222");
                    this.Manager.Comment("reaching state \'S1468\'");
                    goto label257;
                }
                if ((temp904 == 1)) {
                    this.Manager.Comment("reaching state \'S821\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp903;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp903 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1223\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp903, "return of NetrLogonControl, state S1223");
                    this.Manager.Comment("reaching state \'S1469\'");
                    goto label257;
                }
                throw new InvalidOperationException("never reached");
            label257:
;
                goto label259;
            }
            if ((temp909 == 2)) {
                this.Manager.Comment("reaching state \'S277\'");
                bool temp905;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp905);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp905);
                this.Manager.Comment("reaching state \'S478\'");
                int temp908 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetClientAccountTypeChecker5)));
                if ((temp908 == 0)) {
                    this.Manager.Comment("reaching state \'S822\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp906;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp906 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1224\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp906, "return of NetrLogonControl, state S1224");
                    this.Manager.Comment("reaching state \'S1470\'");
                    goto label258;
                }
                if ((temp908 == 1)) {
                    this.Manager.Comment("reaching state \'S823\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp907;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp907 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1225\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp907, "return of NetrLogonControl, state S1225");
                    this.Manager.Comment("reaching state \'S1471\'");
                    goto label258;
                }
                throw new InvalidOperationException("never reached");
            label258:
;
                goto label259;
            }
            throw new InvalidOperationException("never reached");
        label259:
;
            this.Manager.EndTest();
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S476");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S476");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S477");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S477");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S478");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS94GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S478");
        }
        #endregion
        
        #region Test Starting in S96
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96");
            this.Manager.Comment("reaching state \'S96\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp910;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp910);
            this.Manager.AddReturn(GetPlatformInfo, null, temp910);
            this.Manager.Comment("reaching state \'S97\'");
            int temp923 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetPlatformChecker2)));
            if ((temp923 == 0)) {
                this.Manager.Comment("reaching state \'S278\'");
                bool temp911;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp911);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp911);
                this.Manager.Comment("reaching state \'S479\'");
                int temp914 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetClientAccountTypeChecker1)));
                if ((temp914 == 0)) {
                    this.Manager.Comment("reaching state \'S824\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp912;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp912 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Comment("reaching state \'S1226\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp912, "return of NetrLogonControl, state S1226");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label260;
                }
                if ((temp914 == 1)) {
                    this.Manager.Comment("reaching state \'S825\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp913;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp913 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Comment("reaching state \'S1227\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp913, "return of NetrLogonControl, state S1227");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label260;
                }
                throw new InvalidOperationException("never reached");
            label260:
;
                goto label263;
            }
            if ((temp923 == 1)) {
                this.Manager.Comment("reaching state \'S279\'");
                bool temp915;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp915);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp915);
                this.Manager.Comment("reaching state \'S480\'");
                int temp918 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetClientAccountTypeChecker3)));
                if ((temp918 == 0)) {
                    this.Manager.Comment("reaching state \'S826\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp916;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp916 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1228\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp916, "return of NetrLogonControl, state S1228");
                    this.Manager.Comment("reaching state \'S1472\'");
                    goto label261;
                }
                if ((temp918 == 1)) {
                    this.Manager.Comment("reaching state \'S827\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp917;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp917 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1229\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp917, "return of NetrLogonControl, state S1229");
                    this.Manager.Comment("reaching state \'S1473\'");
                    goto label261;
                }
                throw new InvalidOperationException("never reached");
            label261:
;
                goto label263;
            }
            if ((temp923 == 2)) {
                this.Manager.Comment("reaching state \'S280\'");
                bool temp919;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp919);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp919);
                this.Manager.Comment("reaching state \'S481\'");
                int temp922 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetClientAccountTypeChecker5)));
                if ((temp922 == 0)) {
                    this.Manager.Comment("reaching state \'S828\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp920;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp920 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1230\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp920, "return of NetrLogonControl, state S1230");
                    this.Manager.Comment("reaching state \'S1474\'");
                    goto label262;
                }
                if ((temp922 == 1)) {
                    this.Manager.Comment("reaching state \'S829\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp921;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp921 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1231\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp921, "return of NetrLogonControl, state S1231");
                    this.Manager.Comment("reaching state \'S1475\'");
                    goto label262;
                }
                throw new InvalidOperationException("never reached");
            label262:
;
                goto label263;
            }
            throw new InvalidOperationException("never reached");
        label263:
;
            this.Manager.EndTest();
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S479");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S479");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S480");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S480");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S481");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS96GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S481");
        }
        #endregion
        
        #region Test Starting in S98
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98() {
            this.Manager.BeginTest("Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98");
            this.Manager.Comment("reaching state \'S98\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp924;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp924);
            this.Manager.AddReturn(GetPlatformInfo, null, temp924);
            this.Manager.Comment("reaching state \'S99\'");
            int temp937 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetPlatformChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetPlatformChecker1)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetPlatformChecker2)));
            if ((temp937 == 0)) {
                this.Manager.Comment("reaching state \'S281\'");
                bool temp925;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp925);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp925);
                this.Manager.Comment("reaching state \'S482\'");
                int temp928 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetClientAccountTypeChecker)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetClientAccountTypeChecker1)));
                if ((temp928 == 0)) {
                    this.Manager.Comment("reaching state \'S830\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp926;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65535,1)\'");
                    temp926 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65535u, 1u);
                    this.Manager.Comment("reaching state \'S1232\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp926, "return of NetrLogonControl, state S1232");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1341();
                    goto label264;
                }
                if ((temp928 == 1)) {
                    this.Manager.Comment("reaching state \'S831\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp927;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65532,1)\'");
                    temp927 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65532u, 1u);
                    this.Manager.Comment("reaching state \'S1233\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_NOT_SUPPORTED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp927, "return of NetrLogonControl, state S1233");
                    Test_QueryAndControlNetlogonBehavior_NetrLogonControlS1340();
                    goto label264;
                }
                throw new InvalidOperationException("never reached");
            label264:
;
                goto label267;
            }
            if ((temp937 == 1)) {
                this.Manager.Comment("reaching state \'S282\'");
                bool temp929;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp929);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp929);
                this.Manager.Comment("reaching state \'S483\'");
                int temp932 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetClientAccountTypeChecker2)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetClientAccountTypeChecker3)));
                if ((temp932 == 0)) {
                    this.Manager.Comment("reaching state \'S832\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp930;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65534,1)\'");
                    temp930 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65534u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1234\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp930, "return of NetrLogonControl, state S1234");
                    this.Manager.Comment("reaching state \'S1476\'");
                    goto label265;
                }
                if ((temp932 == 1)) {
                    this.Manager.Comment("reaching state \'S833\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp931;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp931 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1235\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp931, "return of NetrLogonControl, state S1235");
                    this.Manager.Comment("reaching state \'S1477\'");
                    goto label265;
                }
                throw new InvalidOperationException("never reached");
            label265:
;
                goto label267;
            }
            if ((temp937 == 2)) {
                this.Manager.Comment("reaching state \'S283\'");
                bool temp933;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp933);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp933);
                this.Manager.Comment("reaching state \'S484\'");
                int temp936 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetClientAccountTypeChecker4)), new ExpectedReturn(Test_QueryAndControlNetlogonBehavior_NetrLogonControl.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetClientAccountTypeChecker5)));
                if ((temp936 == 0)) {
                    this.Manager.Comment("reaching state \'S834\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp934;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,65533,1)\'");
                    temp934 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 65533u, 1u);
                    this.Manager.Checkpoint("MS-NRPC_R104215");
                    this.Manager.Comment("reaching state \'S1236\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp934, "return of NetrLogonControl, state S1236");
                    this.Manager.Comment("reaching state \'S1478\'");
                    goto label266;
                }
                if ((temp936 == 1)) {
                    this.Manager.Comment("reaching state \'S835\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp935;
                    this.Manager.Comment("executing step \'call NetrLogonControl(NonDcServer,1,1)\'");
                    temp935 = this.INrpcServerAdapterInstance.NetrLogonControl(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1237\'");
                    this.Manager.Comment("checking step \'return NetrLogonControl/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp935, "return of NetrLogonControl, state S1237");
                    this.Manager.Comment("reaching state \'S1479\'");
                    goto label266;
                }
                throw new InvalidOperationException("never reached");
            label266:
;
                goto label267;
            }
            throw new InvalidOperationException("never reached");
        label267:
;
            this.Manager.EndTest();
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S482");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S482");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S483");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S483");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S484");
        }
        
        private void Test_QueryAndControlNetlogonBehavior_NetrLogonControlS98GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S484");
        }
        #endregion
    }
}
