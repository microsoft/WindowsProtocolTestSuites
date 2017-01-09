// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Microsoft.SpecExplorer.Runtime.Testing;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
    using System.Net;
    using System.DirectoryServices.Protocols;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [System.CodeDom.Compiler.GeneratedCodeAttribute("Spec Explorer", "3.5.3146.0")]
    [TestClassAttribute()]
    public partial class BVT_Test_LocateDc_DsrAddressToSiteNamesW : PtfTestClassBase
    {

        public BVT_Test_LocateDc_DsrAddressToSiteNamesW()
        {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "30000");
        }

        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter INrpcServerAdapterInstance;
        #endregion

        #region Class Initialization and Cleanup
        [ClassInitializeAttribute()]
        public static void ClassInitialize(TestContext context)
        {
            PtfTestClassBase.Initialize(context);
        }

        [ClassCleanupAttribute()]
        public static void ClassCleanup()
        {
            PtfTestClassBase.Cleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            this.InitializeTestManager();
            this.INrpcServerAdapterInstance = ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter))));

        }

        protected override void TestCleanup()
        {
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
        public void NRPC_BVT_Test_LocateDc_DsrAddressToSiteNamesWS0()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrAddressToSiteNamesWS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp0, "sutPlatform of GetPlatform, state S1");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp1;
            this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{InvalidSocketAddress})\'");
            temp1 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType.InvalidSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Checkpoint("MS-NRPC_R103451");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp1, "return of DsrAddressToSiteNamesExW, state S24");
            BVT_Test_LocateDc_DsrAddressToSiteNamesWS32();
            this.Manager.EndTest();
        }

        private void BVT_Test_LocateDc_DsrAddressToSiteNamesWS32()
        {
            this.Manager.Comment("reaching state \'S32\'");
        }
        #endregion

        #region Test Starting in S10
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrAddressToSiteNamesWS10()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrAddressToSiteNamesWS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp2;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp2);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp2, "sutPlatform of GetPlatform, state S11");
            this.Manager.Comment("reaching state \'S21\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp3;
            this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{Ipv4SocketAddress})\'");
            temp3 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Checkpoint("MS-NRPC_R103451");
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp3, "return of DsrAddressToSiteNamesExW, state S29");
            BVT_Test_LocateDc_DsrAddressToSiteNamesWS32();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S12
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrAddressToSiteNamesWS12()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrAddressToSiteNamesWS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp4;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp4);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp4, "sutPlatform of GetPlatform, state S13");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp5;
            this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{Ipv6SocketAddress})\'");
            temp5 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType.Ipv6SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Checkpoint("MS-NRPC_R103451");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp5, "return of DsrAddressToSiteNamesExW, state S30");
            BVT_Test_LocateDc_DsrAddressToSiteNamesWS32();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrAddressToSiteNamesWS14()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrAddressToSiteNamesWS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp6;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp6);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp6, "sutPlatform of GetPlatform, state S15");
            this.Manager.Comment("reaching state \'S23\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp7;
            this.Manager.Comment("executing step \'call DsrAddressToSiteNamesExW(PrimaryDc,{InvalidFormatSocketAddre" +
                    "ss})\'");
            temp7 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesExW(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType.InvalidFormatSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Checkpoint("MS-NRPC_R103451");
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return DsrAddressToSiteNamesExW/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp7, "return of DsrAddressToSiteNamesExW, state S31");
            BVT_Test_LocateDc_DsrAddressToSiteNamesWS32();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrAddressToSiteNamesWS2()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrAddressToSiteNamesWS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp8;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp8);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp8, "sutPlatform of GetPlatform, state S3");
            this.Manager.Comment("reaching state \'S17\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp9;
            this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{Ipv4SocketAddress})\'");
            temp9 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType.Ipv4SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Checkpoint("MS-NRPC_R103449");
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp9, "return of DsrAddressToSiteNamesW, state S25");
            BVT_Test_LocateDc_DsrAddressToSiteNamesWS32();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrAddressToSiteNamesWS4()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrAddressToSiteNamesWS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp10;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp10);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp10, "sutPlatform of GetPlatform, state S5");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp11;
            this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{Ipv6SocketAddress})\'");
            temp11 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType.Ipv6SocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Checkpoint("MS-NRPC_R103449");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp11, "return of DsrAddressToSiteNamesW, state S26");
            BVT_Test_LocateDc_DsrAddressToSiteNamesWS32();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrAddressToSiteNamesWS6()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrAddressToSiteNamesWS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp12;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp12);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp12, "sutPlatform of GetPlatform, state S7");
            this.Manager.Comment("reaching state \'S19\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp13;
            this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{InvalidFormatSocketAddress" +
                    "})\'");
            temp13 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType.InvalidFormatSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Checkpoint("MS-NRPC_R103449");
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp13, "return of DsrAddressToSiteNamesW, state S27");
            BVT_Test_LocateDc_DsrAddressToSiteNamesWS32();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_LocateDc_DsrAddressToSiteNamesWS8()
        {
            this.Manager.BeginTest("BVT_Test_LocateDc_DsrAddressToSiteNamesWS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp14;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp14);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp14, "sutPlatform of GetPlatform, state S9");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp15;
            this.Manager.Comment("executing step \'call DsrAddressToSiteNamesW(PrimaryDc,{InvalidSocketAddress})\'");
            temp15 = this.INrpcServerAdapterInstance.DsrAddressToSiteNamesW(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, this.Make<Microsoft.Modeling.Set<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType, Microsoft.Xrt.Runtime.Singleton>(), Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.SocketAddressType.InvalidSocketAddress, this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}));
            this.Manager.Checkpoint("MS-NRPC_R103449");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("checking step \'return DsrAddressToSiteNamesW/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp15, "return of DsrAddressToSiteNamesW, state S28");
            BVT_Test_LocateDc_DsrAddressToSiteNamesWS32();
            this.Manager.EndTest();
        }
        #endregion
    }
}
