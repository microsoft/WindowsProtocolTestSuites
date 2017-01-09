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
    public partial class Test_LocateDc_DsrAddressToSiteNamesW : PtfTestClassBase {
        
        public Test_LocateDc_DsrAddressToSiteNamesW() {
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
        public void Test_LocateDc_DsrAddressToSiteNamesWS0() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp4 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS0GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS0GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS0GetPlatformChecker2)));
            if ((temp4 == 0)) {
                this.Manager.Comment("reaching state \'S62\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp1;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{InvalidFormatSocketAddre" +
                        "ss})\'");
                temp1 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidFormatSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103451");
                this.Manager.Comment("reaching state \'S155\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp1, "return of DsrAddressToSiteNamesExW, state S155");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label0;
            }
            if ((temp4 == 1)) {
                this.Manager.Comment("reaching state \'S63\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp2;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{InvalidFormatSocketAddre" +
                        "ss})\'");
                temp2 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidFormatSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103451");
                this.Manager.Comment("reaching state \'S156\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp2, "return of DsrAddressToSiteNamesExW, state S156");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label0;
            }
            if ((temp4 == 2)) {
                this.Manager.Comment("reaching state \'S64\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp3;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{InvalidFormatSocketAddre" +
                        "ss})\'");
                temp3 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidFormatSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103451");
                this.Manager.Comment("reaching state \'S157\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp3, "return of DsrAddressToSiteNamesExW, state S157");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS0GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS248() {
            this.Manager.Comment("reaching state \'S248\'");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS0GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS249() {
            this.Manager.Comment("reaching state \'S249\'");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS0GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS250() {
            this.Manager.Comment("reaching state \'S250\'");
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS10() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp5;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp5);
            this.Manager.AddReturn(GetPlatformInfo, null, temp5);
            this.Manager.Comment("reaching state \'S11\'");
            int temp9 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS10GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS10GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS10GetPlatformChecker2)));
            if ((temp9 == 0)) {
                this.Manager.Comment("reaching state \'S77\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp6;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{Ipv6SocketAddress})\'");
                temp6 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv6SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103449");
                this.Manager.Comment("reaching state \'S170\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp6, "return of DsrAddressToSiteNamesW, state S170");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label1;
            }
            if ((temp9 == 1)) {
                this.Manager.Comment("reaching state \'S78\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp7;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{Ipv6SocketAddress})\'");
                temp7 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv6SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103449");
                this.Manager.Comment("reaching state \'S171\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp7, "return of DsrAddressToSiteNamesW, state S171");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label1;
            }
            if ((temp9 == 2)) {
                this.Manager.Comment("reaching state \'S79\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp8;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp8 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S172\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp8, "return of DsrAddressToSiteNamesW, state S172");
                this.Manager.Comment("reaching state \'S255\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS10GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS10GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS10GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS12() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp10;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp10);
            this.Manager.AddReturn(GetPlatformInfo, null, temp10);
            this.Manager.Comment("reaching state \'S13\'");
            int temp14 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS12GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS12GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS12GetPlatformChecker2)));
            if ((temp14 == 0)) {
                this.Manager.Comment("reaching state \'S80\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp11;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{InvalidSocketAddress})\'");
                temp11 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103449");
                this.Manager.Comment("reaching state \'S173\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp11, "return of DsrAddressToSiteNamesW, state S173");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label2;
            }
            if ((temp14 == 1)) {
                this.Manager.Comment("reaching state \'S81\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp12;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{InvalidSocketAddress})\'");
                temp12 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103449");
                this.Manager.Comment("reaching state \'S174\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp12, "return of DsrAddressToSiteNamesW, state S174");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label2;
            }
            if ((temp14 == 2)) {
                this.Manager.Comment("reaching state \'S82\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp13;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp13 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S175\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp13, "return of DsrAddressToSiteNamesW, state S175");
                this.Manager.Comment("reaching state \'S256\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS12GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS12GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS12GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        #endregion
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS14() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp15;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp15);
            this.Manager.AddReturn(GetPlatformInfo, null, temp15);
            this.Manager.Comment("reaching state \'S15\'");
            int temp19 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS14GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS14GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS14GetPlatformChecker2)));
            if ((temp19 == 0)) {
                this.Manager.Comment("reaching state \'S83\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp16;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{Ipv4SocketAddress})\'");
                temp16 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103449");
                this.Manager.Comment("reaching state \'S176\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp16, "return of DsrAddressToSiteNamesW, state S176");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label3;
            }
            if ((temp19 == 1)) {
                this.Manager.Comment("reaching state \'S84\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp17;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{Ipv4SocketAddress})\'");
                temp17 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103449");
                this.Manager.Comment("reaching state \'S177\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp17, "return of DsrAddressToSiteNamesW, state S177");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label3;
            }
            if ((temp19 == 2)) {
                this.Manager.Comment("reaching state \'S85\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp18;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp18 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S178\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp18, "return of DsrAddressToSiteNamesW, state S178");
                this.Manager.Comment("reaching state \'S257\'");
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS14GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS14GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS14GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS16() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp20;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp20);
            this.Manager.AddReturn(GetPlatformInfo, null, temp20);
            this.Manager.Comment("reaching state \'S17\'");
            int temp24 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS16GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS16GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS16GetPlatformChecker2)));
            if ((temp24 == 0)) {
                this.Manager.Comment("reaching state \'S86\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp21;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{InvalidFormatSocketAddress" +
                        "})\'");
                temp21 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidFormatSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103449");
                this.Manager.Comment("reaching state \'S179\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp21, "return of DsrAddressToSiteNamesW, state S179");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label4;
            }
            if ((temp24 == 1)) {
                this.Manager.Comment("reaching state \'S87\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp22;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{InvalidFormatSocketAddress" +
                        "})\'");
                temp22 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidFormatSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103449");
                this.Manager.Comment("reaching state \'S180\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp22, "return of DsrAddressToSiteNamesW, state S180");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label4;
            }
            if ((temp24 == 2)) {
                this.Manager.Comment("reaching state \'S88\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp23;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp23 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S181\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp23, "return of DsrAddressToSiteNamesW, state S181");
                this.Manager.Comment("reaching state \'S258\'");
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS16GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS16GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS16GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        #endregion
        
        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS18() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp25;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp25);
            this.Manager.AddReturn(GetPlatformInfo, null, temp25);
            this.Manager.Comment("reaching state \'S19\'");
            int temp29 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS18GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS18GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS18GetPlatformChecker2)));
            if ((temp29 == 0)) {
                this.Manager.Comment("reaching state \'S89\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp26;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(NonDcServer,{Ipv4SocketAddress})\'");
                temp26 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103507");
                this.Manager.Checkpoint("MS-NRPC_R103452");
                this.Manager.Comment("reaching state \'S182\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp26, "return of DsrAddressToSiteNamesExW, state S182");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label5;
            }
            if ((temp29 == 1)) {
                this.Manager.Comment("reaching state \'S90\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp27;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(NonDcServer,{Ipv4SocketAddress})\'");
                temp27 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103507");
                this.Manager.Checkpoint("MS-NRPC_R103452");
                this.Manager.Comment("reaching state \'S183\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp27, "return of DsrAddressToSiteNamesExW, state S183");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label5;
            }
            if ((temp29 == 2)) {
                this.Manager.Comment("reaching state \'S91\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp28;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp28 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S184\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp28, "return of DsrAddressToSiteNamesW, state S184");
                this.Manager.Comment("reaching state \'S259\'");
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS18GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS18GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS18GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS2() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp30;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp30);
            this.Manager.AddReturn(GetPlatformInfo, null, temp30);
            this.Manager.Comment("reaching state \'S3\'");
            int temp34 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS2GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS2GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS2GetPlatformChecker2)));
            if ((temp34 == 0)) {
                this.Manager.Comment("reaching state \'S65\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp31;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp31 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S158\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp31, "return of DsrAddressToSiteNamesW, state S158");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label6;
            }
            if ((temp34 == 1)) {
                this.Manager.Comment("reaching state \'S66\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp32;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp32 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S159\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp32, "return of DsrAddressToSiteNamesW, state S159");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label6;
            }
            if ((temp34 == 2)) {
                this.Manager.Comment("reaching state \'S67\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp33;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp33 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S160\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp33, "return of DsrAddressToSiteNamesW, state S160");
                this.Manager.Comment("reaching state \'S251\'");
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS2GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS2GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS2GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        #endregion
        
        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS20() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp35;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp35);
            this.Manager.AddReturn(GetPlatformInfo, null, temp35);
            this.Manager.Comment("reaching state \'S21\'");
            int temp39 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS20GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS20GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS20GetPlatformChecker2)));
            if ((temp39 == 0)) {
                this.Manager.Comment("reaching state \'S92\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp36;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(NonDcServer,{Ipv6SocketAddress})\'");
                temp36 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv6SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103507");
                this.Manager.Checkpoint("MS-NRPC_R103452");
                this.Manager.Comment("reaching state \'S185\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp36, "return of DsrAddressToSiteNamesExW, state S185");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label7;
            }
            if ((temp39 == 1)) {
                this.Manager.Comment("reaching state \'S93\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp37;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(NonDcServer,{InvalidFormatSocketAdd" +
                        "ress})\'");
                temp37 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidFormatSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103507");
                this.Manager.Checkpoint("MS-NRPC_R103452");
                this.Manager.Comment("reaching state \'S186\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp37, "return of DsrAddressToSiteNamesExW, state S186");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label7;
            }
            if ((temp39 == 2)) {
                this.Manager.Comment("reaching state \'S94\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp38;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp38 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S187\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp38, "return of DsrAddressToSiteNamesW, state S187");
                this.Manager.Comment("reaching state \'S260\'");
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS20GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS20GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS20GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        #endregion
        
        #region Test Starting in S22
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS22() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp40;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp40);
            this.Manager.AddReturn(GetPlatformInfo, null, temp40);
            this.Manager.Comment("reaching state \'S23\'");
            int temp44 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS22GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS22GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS22GetPlatformChecker2)));
            if ((temp44 == 0)) {
                this.Manager.Comment("reaching state \'S95\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp41;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(NonDcServer,{InvalidSocketAddress})" +
                        "\'");
                temp41 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103507");
                this.Manager.Checkpoint("MS-NRPC_R103452");
                this.Manager.Comment("reaching state \'S188\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp41, "return of DsrAddressToSiteNamesExW, state S188");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label8;
            }
            if ((temp44 == 1)) {
                this.Manager.Comment("reaching state \'S96\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp42;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(NonDcServer,{Ipv6SocketAddress})\'");
                temp42 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv6SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103507");
                this.Manager.Checkpoint("MS-NRPC_R103452");
                this.Manager.Comment("reaching state \'S189\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp42, "return of DsrAddressToSiteNamesExW, state S189");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label8;
            }
            if ((temp44 == 2)) {
                this.Manager.Comment("reaching state \'S97\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp43;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp43 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S190\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp43, "return of DsrAddressToSiteNamesW, state S190");
                this.Manager.Comment("reaching state \'S261\'");
                goto label8;
            }
            throw new InvalidOperationException("never reached");
        label8:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS22GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS22GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS22GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        #endregion
        
        #region Test Starting in S24
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS24() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp45;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp45);
            this.Manager.AddReturn(GetPlatformInfo, null, temp45);
            this.Manager.Comment("reaching state \'S25\'");
            int temp49 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS24GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS24GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS24GetPlatformChecker2)));
            if ((temp49 == 0)) {
                this.Manager.Comment("reaching state \'S100\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp46;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp46 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S193\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp46, "return of DsrAddressToSiteNamesW, state S193");
                this.Manager.Comment("reaching state \'S262\'");
                goto label9;
            }
            if ((temp49 == 1)) {
                this.Manager.Comment("reaching state \'S98\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp47;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(NonDcServer,{InvalidFormatSocketAdd" +
                        "ress})\'");
                temp47 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidFormatSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103507");
                this.Manager.Checkpoint("MS-NRPC_R103452");
                this.Manager.Comment("reaching state \'S191\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp47, "return of DsrAddressToSiteNamesExW, state S191");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label9;
            }
            if ((temp49 == 2)) {
                this.Manager.Comment("reaching state \'S99\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp48;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(NonDcServer,{InvalidSocketAddress})" +
                        "\'");
                temp48 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103507");
                this.Manager.Checkpoint("MS-NRPC_R103452");
                this.Manager.Comment("reaching state \'S192\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp48, "return of DsrAddressToSiteNamesExW, state S192");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label9;
            }
            throw new InvalidOperationException("never reached");
        label9:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS24GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS24GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS24GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        #endregion
        
        #region Test Starting in S26
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS26() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp50;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp50);
            this.Manager.AddReturn(GetPlatformInfo, null, temp50);
            this.Manager.Comment("reaching state \'S27\'");
            int temp54 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS26GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS26GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS26GetPlatformChecker2)));
            if ((temp54 == 0)) {
                this.Manager.Comment("reaching state \'S101\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp51;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{Ipv6SocketAddress})\'");
                temp51 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv6SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103451");
                this.Manager.Comment("reaching state \'S194\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp51, "return of DsrAddressToSiteNamesExW, state S194");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label10;
            }
            if ((temp54 == 1)) {
                this.Manager.Comment("reaching state \'S102\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp52;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{Ipv6SocketAddress})\'");
                temp52 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv6SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103451");
                this.Manager.Comment("reaching state \'S195\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp52, "return of DsrAddressToSiteNamesExW, state S195");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label10;
            }
            if ((temp54 == 2)) {
                this.Manager.Comment("reaching state \'S103\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp53;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp53 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S196\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp53, "return of DsrAddressToSiteNamesW, state S196");
                this.Manager.Comment("reaching state \'S263\'");
                goto label10;
            }
            throw new InvalidOperationException("never reached");
        label10:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS26GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS26GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS26GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        #endregion
        
        #region Test Starting in S28
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS28() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp55;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp55);
            this.Manager.AddReturn(GetPlatformInfo, null, temp55);
            this.Manager.Comment("reaching state \'S29\'");
            int temp59 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS28GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS28GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS28GetPlatformChecker2)));
            if ((temp59 == 0)) {
                this.Manager.Comment("reaching state \'S104\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp56;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{InvalidSocketAddress})\'");
                temp56 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103451");
                this.Manager.Comment("reaching state \'S197\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp56, "return of DsrAddressToSiteNamesExW, state S197");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label11;
            }
            if ((temp59 == 1)) {
                this.Manager.Comment("reaching state \'S105\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp57;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{InvalidSocketAddress})\'");
                temp57 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103451");
                this.Manager.Comment("reaching state \'S198\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp57, "return of DsrAddressToSiteNamesExW, state S198");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label11;
            }
            if ((temp59 == 2)) {
                this.Manager.Comment("reaching state \'S106\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp58;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp58 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S199\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp58, "return of DsrAddressToSiteNamesW, state S199");
                this.Manager.Comment("reaching state \'S264\'");
                goto label11;
            }
            throw new InvalidOperationException("never reached");
        label11:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS28GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS28GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS28GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        #endregion
        
        #region Test Starting in S30
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS30() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp60;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp60);
            this.Manager.AddReturn(GetPlatformInfo, null, temp60);
            this.Manager.Comment("reaching state \'S31\'");
            int temp64 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS30GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS30GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS30GetPlatformChecker2)));
            if ((temp64 == 0)) {
                this.Manager.Comment("reaching state \'S107\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp61;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{Ipv4SocketAddress})\'");
                temp61 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103451");
                this.Manager.Comment("reaching state \'S200\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp61, "return of DsrAddressToSiteNamesExW, state S200");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label12;
            }
            if ((temp64 == 1)) {
                this.Manager.Comment("reaching state \'S108\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp62;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{Ipv4SocketAddress})\'");
                temp62 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103451");
                this.Manager.Comment("reaching state \'S201\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp62, "return of DsrAddressToSiteNamesExW, state S201");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label12;
            }
            if ((temp64 == 2)) {
                this.Manager.Comment("reaching state \'S109\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp63;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp63 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S202\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp63, "return of DsrAddressToSiteNamesW, state S202");
                this.Manager.Comment("reaching state \'S265\'");
                goto label12;
            }
            throw new InvalidOperationException("never reached");
        label12:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS30GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS30GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS30GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        #endregion
        
        #region Test Starting in S32
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS32() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS32");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp65;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp65);
            this.Manager.AddReturn(GetPlatformInfo, null, temp65);
            this.Manager.Comment("reaching state \'S33\'");
            int temp69 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS32GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS32GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS32GetPlatformChecker2)));
            if ((temp69 == 0)) {
                this.Manager.Comment("reaching state \'S110\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp66;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp66 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S203\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp66, "return of DsrAddressToSiteNamesW, state S203");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label13;
            }
            if ((temp69 == 1)) {
                this.Manager.Comment("reaching state \'S111\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp67;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp67 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S204\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp67, "return of DsrAddressToSiteNamesW, state S204");
                this.Manager.Comment("reaching state \'S266\'");
                goto label13;
            }
            if ((temp69 == 2)) {
                this.Manager.Comment("reaching state \'S112\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp68;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp68 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S205\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp68, "return of DsrAddressToSiteNamesW, state S205");
                this.Manager.Comment("reaching state \'S267\'");
                goto label13;
            }
            throw new InvalidOperationException("never reached");
        label13:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS32GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS32GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS32GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        #endregion
        
        #region Test Starting in S34
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS34() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS34");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp70;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp70);
            this.Manager.AddReturn(GetPlatformInfo, null, temp70);
            this.Manager.Comment("reaching state \'S35\'");
            int temp74 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS34GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS34GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS34GetPlatformChecker2)));
            if ((temp74 == 0)) {
                this.Manager.Comment("reaching state \'S113\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp71;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{InvalidSocketAddress})\'");
                temp71 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S206\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp71, "return of DsrAddressToSiteNamesW, state S206");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label14;
            }
            if ((temp74 == 1)) {
                this.Manager.Comment("reaching state \'S114\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp72;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp72 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S207\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp72, "return of DsrAddressToSiteNamesW, state S207");
                this.Manager.Comment("reaching state \'S268\'");
                goto label14;
            }
            if ((temp74 == 2)) {
                this.Manager.Comment("reaching state \'S115\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp73;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp73 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S208\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp73, "return of DsrAddressToSiteNamesW, state S208");
                this.Manager.Comment("reaching state \'S269\'");
                goto label14;
            }
            throw new InvalidOperationException("never reached");
        label14:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS34GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS34GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS34GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        #endregion
        
        #region Test Starting in S36
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS36() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS36");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp75;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp75);
            this.Manager.AddReturn(GetPlatformInfo, null, temp75);
            this.Manager.Comment("reaching state \'S37\'");
            int temp79 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS36GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS36GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS36GetPlatformChecker2)));
            if ((temp79 == 0)) {
                this.Manager.Comment("reaching state \'S116\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp76;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv6SocketAddress})\'");
                temp76 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv6SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S209\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp76, "return of DsrAddressToSiteNamesW, state S209");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label15;
            }
            if ((temp79 == 1)) {
                this.Manager.Comment("reaching state \'S117\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp77;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp77 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S210\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp77, "return of DsrAddressToSiteNamesW, state S210");
                this.Manager.Comment("reaching state \'S270\'");
                goto label15;
            }
            if ((temp79 == 2)) {
                this.Manager.Comment("reaching state \'S118\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp78;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp78 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S211\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp78, "return of DsrAddressToSiteNamesW, state S211");
                this.Manager.Comment("reaching state \'S271\'");
                goto label15;
            }
            throw new InvalidOperationException("never reached");
        label15:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS36GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS36GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS36GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        #endregion
        
        #region Test Starting in S38
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS38() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS38");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp80;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp80);
            this.Manager.AddReturn(GetPlatformInfo, null, temp80);
            this.Manager.Comment("reaching state \'S39\'");
            int temp84 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS38GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS38GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS38GetPlatformChecker2)));
            if ((temp84 == 0)) {
                this.Manager.Comment("reaching state \'S119\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp81;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{InvalidFormatSocketAddre" +
                        "ss})\'");
                temp81 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidFormatSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S212\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp81, "return of DsrAddressToSiteNamesW, state S212");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label16;
            }
            if ((temp84 == 1)) {
                this.Manager.Comment("reaching state \'S120\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp82;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp82 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S213\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp82, "return of DsrAddressToSiteNamesW, state S213");
                this.Manager.Comment("reaching state \'S272\'");
                goto label16;
            }
            if ((temp84 == 2)) {
                this.Manager.Comment("reaching state \'S121\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp83;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp83 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S214\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp83, "return of DsrAddressToSiteNamesW, state S214");
                this.Manager.Comment("reaching state \'S273\'");
                goto label16;
            }
            throw new InvalidOperationException("never reached");
        label16:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS38GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS38GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS38GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS4() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp85;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp85);
            this.Manager.AddReturn(GetPlatformInfo, null, temp85);
            this.Manager.Comment("reaching state \'S5\'");
            int temp89 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS4GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS4GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS4GetPlatformChecker2)));
            if ((temp89 == 0)) {
                this.Manager.Comment("reaching state \'S68\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp86;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv6SocketAddress})\'");
                temp86 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv6SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S161\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp86, "return of DsrAddressToSiteNamesW, state S161");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label17;
            }
            if ((temp89 == 1)) {
                this.Manager.Comment("reaching state \'S69\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp87;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{InvalidFormatSocketAddre" +
                        "ss})\'");
                temp87 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidFormatSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S162\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp87, "return of DsrAddressToSiteNamesW, state S162");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label17;
            }
            if ((temp89 == 2)) {
                this.Manager.Comment("reaching state \'S70\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp88;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp88 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S163\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp88, "return of DsrAddressToSiteNamesW, state S163");
                this.Manager.Comment("reaching state \'S252\'");
                goto label17;
            }
            throw new InvalidOperationException("never reached");
        label17:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS4GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS4GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS4GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        #endregion
        
        #region Test Starting in S40
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS40() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS40");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp90;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp90);
            this.Manager.AddReturn(GetPlatformInfo, null, temp90);
            this.Manager.Comment("reaching state \'S41\'");
            int temp94 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS40GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS40GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS40GetPlatformChecker2)));
            if ((temp94 == 0)) {
                this.Manager.Comment("reaching state \'S122\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp91;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{Ipv6SocketAddress})\'");
                temp91 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv6SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103449");
                this.Manager.Comment("reaching state \'S215\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp91, "return of DsrAddressToSiteNamesW, state S215");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label18;
            }
            if ((temp94 == 1)) {
                this.Manager.Comment("reaching state \'S123\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp92;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp92 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S216\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp92, "return of DsrAddressToSiteNamesW, state S216");
                this.Manager.Comment("reaching state \'S274\'");
                goto label18;
            }
            if ((temp94 == 2)) {
                this.Manager.Comment("reaching state \'S124\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp93;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp93 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S217\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp93, "return of DsrAddressToSiteNamesW, state S217");
                this.Manager.Comment("reaching state \'S275\'");
                goto label18;
            }
            throw new InvalidOperationException("never reached");
        label18:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS40GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS40GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS40GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        #endregion
        
        #region Test Starting in S42
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS42() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS42");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp95;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp95);
            this.Manager.AddReturn(GetPlatformInfo, null, temp95);
            this.Manager.Comment("reaching state \'S43\'");
            int temp99 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS42GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS42GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS42GetPlatformChecker2)));
            if ((temp99 == 0)) {
                this.Manager.Comment("reaching state \'S125\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp96;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{InvalidSocketAddress})\'");
                temp96 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103449");
                this.Manager.Comment("reaching state \'S218\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp96, "return of DsrAddressToSiteNamesW, state S218");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label19;
            }
            if ((temp99 == 1)) {
                this.Manager.Comment("reaching state \'S126\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp97;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp97 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S219\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp97, "return of DsrAddressToSiteNamesW, state S219");
                this.Manager.Comment("reaching state \'S276\'");
                goto label19;
            }
            if ((temp99 == 2)) {
                this.Manager.Comment("reaching state \'S127\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp98;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp98 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S220\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp98, "return of DsrAddressToSiteNamesW, state S220");
                this.Manager.Comment("reaching state \'S277\'");
                goto label19;
            }
            throw new InvalidOperationException("never reached");
        label19:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS42GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS42GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS42GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        #endregion
        
        #region Test Starting in S44
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS44() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS44");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp100;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp100);
            this.Manager.AddReturn(GetPlatformInfo, null, temp100);
            this.Manager.Comment("reaching state \'S45\'");
            int temp104 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS44GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS44GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS44GetPlatformChecker2)));
            if ((temp104 == 0)) {
                this.Manager.Comment("reaching state \'S128\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp101;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{Ipv4SocketAddress})\'");
                temp101 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103449");
                this.Manager.Comment("reaching state \'S221\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp101, "return of DsrAddressToSiteNamesW, state S221");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label20;
            }
            if ((temp104 == 1)) {
                this.Manager.Comment("reaching state \'S129\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp102;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp102 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S222\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp102, "return of DsrAddressToSiteNamesW, state S222");
                this.Manager.Comment("reaching state \'S278\'");
                goto label20;
            }
            if ((temp104 == 2)) {
                this.Manager.Comment("reaching state \'S130\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp103;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp103 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S223\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp103, "return of DsrAddressToSiteNamesW, state S223");
                this.Manager.Comment("reaching state \'S279\'");
                goto label20;
            }
            throw new InvalidOperationException("never reached");
        label20:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS44GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS44GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS44GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        #endregion
        
        #region Test Starting in S46
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS46() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS46");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp105;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp105);
            this.Manager.AddReturn(GetPlatformInfo, null, temp105);
            this.Manager.Comment("reaching state \'S47\'");
            int temp109 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS46GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS46GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS46GetPlatformChecker2)));
            if ((temp109 == 0)) {
                this.Manager.Comment("reaching state \'S131\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp106;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{InvalidFormatSocketAddress" +
                        "})\'");
                temp106 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidFormatSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103449");
                this.Manager.Comment("reaching state \'S224\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp106, "return of DsrAddressToSiteNamesW, state S224");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label21;
            }
            if ((temp109 == 1)) {
                this.Manager.Comment("reaching state \'S132\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp107;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp107 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S225\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp107, "return of DsrAddressToSiteNamesW, state S225");
                this.Manager.Comment("reaching state \'S280\'");
                goto label21;
            }
            if ((temp109 == 2)) {
                this.Manager.Comment("reaching state \'S133\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp108;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp108 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S226\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp108, "return of DsrAddressToSiteNamesW, state S226");
                this.Manager.Comment("reaching state \'S281\'");
                goto label21;
            }
            throw new InvalidOperationException("never reached");
        label21:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS46GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS46GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS46GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        #endregion
        
        #region Test Starting in S48
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS48() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS48");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp110;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp110);
            this.Manager.AddReturn(GetPlatformInfo, null, temp110);
            this.Manager.Comment("reaching state \'S49\'");
            int temp114 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS48GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS48GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS48GetPlatformChecker2)));
            if ((temp114 == 0)) {
                this.Manager.Comment("reaching state \'S134\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp111;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(NonDcServer,{Ipv4SocketAddress})\'");
                temp111 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103507");
                this.Manager.Checkpoint("MS-NRPC_R103452");
                this.Manager.Comment("reaching state \'S227\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp111, "return of DsrAddressToSiteNamesExW, state S227");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label22;
            }
            if ((temp114 == 1)) {
                this.Manager.Comment("reaching state \'S135\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp112;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp112 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S228\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp112, "return of DsrAddressToSiteNamesW, state S228");
                this.Manager.Comment("reaching state \'S282\'");
                goto label22;
            }
            if ((temp114 == 2)) {
                this.Manager.Comment("reaching state \'S136\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp113;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp113 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S229\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp113, "return of DsrAddressToSiteNamesW, state S229");
                this.Manager.Comment("reaching state \'S283\'");
                goto label22;
            }
            throw new InvalidOperationException("never reached");
        label22:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS48GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS48GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS48GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        #endregion
        
        #region Test Starting in S50
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS50() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS50");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp115;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp115);
            this.Manager.AddReturn(GetPlatformInfo, null, temp115);
            this.Manager.Comment("reaching state \'S51\'");
            int temp119 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS50GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS50GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS50GetPlatformChecker2)));
            if ((temp119 == 0)) {
                this.Manager.Comment("reaching state \'S137\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp116;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(NonDcServer,{InvalidSocketAddress})" +
                        "\'");
                temp116 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103507");
                this.Manager.Checkpoint("MS-NRPC_R103452");
                this.Manager.Comment("reaching state \'S230\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp116, "return of DsrAddressToSiteNamesExW, state S230");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label23;
            }
            if ((temp119 == 1)) {
                this.Manager.Comment("reaching state \'S138\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp117;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp117 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S231\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp117, "return of DsrAddressToSiteNamesW, state S231");
                this.Manager.Comment("reaching state \'S284\'");
                goto label23;
            }
            if ((temp119 == 2)) {
                this.Manager.Comment("reaching state \'S139\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp118;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp118 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S232\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp118, "return of DsrAddressToSiteNamesW, state S232");
                this.Manager.Comment("reaching state \'S285\'");
                goto label23;
            }
            throw new InvalidOperationException("never reached");
        label23:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS50GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS50GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS50GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        #endregion
        
        #region Test Starting in S52
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS52() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS52");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp120;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp120);
            this.Manager.AddReturn(GetPlatformInfo, null, temp120);
            this.Manager.Comment("reaching state \'S53\'");
            int temp124 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS52GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS52GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS52GetPlatformChecker2)));
            if ((temp124 == 0)) {
                this.Manager.Comment("reaching state \'S140\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp121;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(NonDcServer,{Ipv6SocketAddress})\'");
                temp121 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv6SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103507");
                this.Manager.Checkpoint("MS-NRPC_R103452");
                this.Manager.Comment("reaching state \'S233\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp121, "return of DsrAddressToSiteNamesExW, state S233");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label24;
            }
            if ((temp124 == 1)) {
                this.Manager.Comment("reaching state \'S141\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp122;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp122 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S234\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp122, "return of DsrAddressToSiteNamesW, state S234");
                this.Manager.Comment("reaching state \'S286\'");
                goto label24;
            }
            if ((temp124 == 2)) {
                this.Manager.Comment("reaching state \'S142\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp123;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp123 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S235\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp123, "return of DsrAddressToSiteNamesW, state S235");
                this.Manager.Comment("reaching state \'S287\'");
                goto label24;
            }
            throw new InvalidOperationException("never reached");
        label24:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS52GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS52GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS52GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        #endregion
        
        #region Test Starting in S54
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS54() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS54");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp125;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp125);
            this.Manager.AddReturn(GetPlatformInfo, null, temp125);
            this.Manager.Comment("reaching state \'S55\'");
            int temp129 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS54GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS54GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS54GetPlatformChecker2)));
            if ((temp129 == 0)) {
                this.Manager.Comment("reaching state \'S143\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp126;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(NonDcServer,{InvalidFormatSocketAdd" +
                        "ress})\'");
                temp126 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidFormatSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103507");
                this.Manager.Checkpoint("MS-NRPC_R103452");
                this.Manager.Comment("reaching state \'S236\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp126, "return of DsrAddressToSiteNamesExW, state S236");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label25;
            }
            if ((temp129 == 1)) {
                this.Manager.Comment("reaching state \'S144\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp127;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp127 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S237\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp127, "return of DsrAddressToSiteNamesW, state S237");
                this.Manager.Comment("reaching state \'S288\'");
                goto label25;
            }
            if ((temp129 == 2)) {
                this.Manager.Comment("reaching state \'S145\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp128;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp128 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S238\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp128, "return of DsrAddressToSiteNamesW, state S238");
                this.Manager.Comment("reaching state \'S289\'");
                goto label25;
            }
            throw new InvalidOperationException("never reached");
        label25:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS54GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS54GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS54GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        #endregion
        
        #region Test Starting in S56
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS56() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS56");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp130;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp130);
            this.Manager.AddReturn(GetPlatformInfo, null, temp130);
            this.Manager.Comment("reaching state \'S57\'");
            int temp134 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS56GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS56GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS56GetPlatformChecker2)));
            if ((temp134 == 0)) {
                this.Manager.Comment("reaching state \'S146\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp131;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{Ipv6SocketAddress})\'");
                temp131 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv6SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103451");
                this.Manager.Comment("reaching state \'S239\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp131, "return of DsrAddressToSiteNamesExW, state S239");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label26;
            }
            if ((temp134 == 1)) {
                this.Manager.Comment("reaching state \'S147\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp132;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp132 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S240\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp132, "return of DsrAddressToSiteNamesW, state S240");
                this.Manager.Comment("reaching state \'S290\'");
                goto label26;
            }
            if ((temp134 == 2)) {
                this.Manager.Comment("reaching state \'S148\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp133;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp133 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S241\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp133, "return of DsrAddressToSiteNamesW, state S241");
                this.Manager.Comment("reaching state \'S291\'");
                goto label26;
            }
            throw new InvalidOperationException("never reached");
        label26:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS56GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS56GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS56GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        #endregion
        
        #region Test Starting in S58
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS58() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS58");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp135;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp135);
            this.Manager.AddReturn(GetPlatformInfo, null, temp135);
            this.Manager.Comment("reaching state \'S59\'");
            int temp139 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS58GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS58GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS58GetPlatformChecker2)));
            if ((temp139 == 0)) {
                this.Manager.Comment("reaching state \'S149\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp136;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{InvalidSocketAddress})\'");
                temp136 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103451");
                this.Manager.Comment("reaching state \'S242\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp136, "return of DsrAddressToSiteNamesExW, state S242");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label27;
            }
            if ((temp139 == 1)) {
                this.Manager.Comment("reaching state \'S150\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp137;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp137 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S243\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp137, "return of DsrAddressToSiteNamesW, state S243");
                this.Manager.Comment("reaching state \'S292\'");
                goto label27;
            }
            if ((temp139 == 2)) {
                this.Manager.Comment("reaching state \'S151\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp138;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp138 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S244\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp138, "return of DsrAddressToSiteNamesW, state S244");
                this.Manager.Comment("reaching state \'S293\'");
                goto label27;
            }
            throw new InvalidOperationException("never reached");
        label27:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS58GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS58GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS58GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS6() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp140;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp140);
            this.Manager.AddReturn(GetPlatformInfo, null, temp140);
            this.Manager.Comment("reaching state \'S7\'");
            int temp144 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS6GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS6GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS6GetPlatformChecker2)));
            if ((temp144 == 0)) {
                this.Manager.Comment("reaching state \'S71\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp141;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{InvalidSocketAddress})\'");
                temp141 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S164\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp141, "return of DsrAddressToSiteNamesW, state S164");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label28;
            }
            if ((temp144 == 1)) {
                this.Manager.Comment("reaching state \'S72\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp142;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv6SocketAddress})\'");
                temp142 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv6SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S165\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp142, "return of DsrAddressToSiteNamesW, state S165");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label28;
            }
            if ((temp144 == 2)) {
                this.Manager.Comment("reaching state \'S73\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp143;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp143 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S166\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp143, "return of DsrAddressToSiteNamesW, state S166");
                this.Manager.Comment("reaching state \'S253\'");
                goto label28;
            }
            throw new InvalidOperationException("never reached");
        label28:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS6GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS6GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS6GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        #endregion
        
        #region Test Starting in S60
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS60() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS60");
            this.Manager.Comment("reaching state \'S60\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp145;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp145);
            this.Manager.AddReturn(GetPlatformInfo, null, temp145);
            this.Manager.Comment("reaching state \'S61\'");
            int temp149 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS60GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS60GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS60GetPlatformChecker2)));
            if ((temp149 == 0)) {
                this.Manager.Comment("reaching state \'S152\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp146;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{Ipv4SocketAddress})\'");
                temp146 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R103451");
                this.Manager.Comment("reaching state \'S245\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp146, "return of DsrAddressToSiteNamesExW, state S245");
                Test_LocateDc_DsrAddressToSiteNamesWS248();
                goto label29;
            }
            if ((temp149 == 1)) {
                this.Manager.Comment("reaching state \'S153\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp147;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp147 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S246\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp147, "return of DsrAddressToSiteNamesW, state S246");
                this.Manager.Comment("reaching state \'S294\'");
                goto label29;
            }
            if ((temp149 == 2)) {
                this.Manager.Comment("reaching state \'S154\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp148;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp148 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S247\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp148, "return of DsrAddressToSiteNamesW, state S247");
                this.Manager.Comment("reaching state \'S295\'");
                goto label29;
            }
            throw new InvalidOperationException("never reached");
        label29:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS60GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS60GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS60GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrAddressToSiteNamesWS8() {
            this.Manager.BeginTest("Test_LocateDc_DsrAddressToSiteNamesWS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp150;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp150);
            this.Manager.AddReturn(GetPlatformInfo, null, temp150);
            this.Manager.Comment("reaching state \'S9\'");
            int temp154 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS8GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS8GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrAddressToSiteNamesW.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrAddressToSiteNamesWS8GetPlatformChecker2)));
            if ((temp154 == 0)) {
                this.Manager.Comment("reaching state \'S74\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp151;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{InvalidFormatSocketAddre" +
                        "ss})\'");
                temp151 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidFormatSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S167\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp151, "return of DsrAddressToSiteNamesW, state S167");
                Test_LocateDc_DsrAddressToSiteNamesWS250();
                goto label30;
            }
            if ((temp154 == 1)) {
                this.Manager.Comment("reaching state \'S75\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp152;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{InvalidSocketAddress})\'");
                temp152 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.InvalidSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S168\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp152, "return of DsrAddressToSiteNamesW, state S168");
                Test_LocateDc_DsrAddressToSiteNamesWS249();
                goto label30;
            }
            if ((temp154 == 2)) {
                this.Manager.Comment("reaching state \'S76\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp153;
                this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(NonDcServer,{Ipv4SocketAddress})\'");
                temp153 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103505");
                this.Manager.Checkpoint("MS-NRPC_R103450");
                this.Manager.Comment("reaching state \'S169\'");
                this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NOT_SUPPORTED, temp153, "return of DsrAddressToSiteNamesW, state S169");
                this.Manager.Comment("reaching state \'S254\'");
                goto label30;
            }
            throw new InvalidOperationException("never reached");
        label30:
;
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS8GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS8GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_LocateDc_DsrAddressToSiteNamesWS8GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        #endregion
    }
}
