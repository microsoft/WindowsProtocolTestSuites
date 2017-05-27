// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class DrsrTTTTestClassBase : TestClassBase
    {
        private List<EnvironmentConfig.Machine> m_tttMachines = new List<EnvironmentConfig.Machine>() { EnvironmentConfig.Machine.PDC };

        protected void SetTTTMachines(EnvironmentConfig.Machine[] machines)
        {
            m_tttMachines.Clear();
            foreach (EnvironmentConfig.Machine m in machines)
            {
                m_tttMachines.Add(m);
            }
        }

        protected int DumpLevel = 0;

        protected int m_attachTime = 0;
        protected override void TestInitialize()
        {
            base.TestInitialize();
        }

        protected override void TestCleanup()
        {
            bool attached = bool.Parse(BaseTestSite.Properties["ATLASAttachTTT"]);
            base.TestCleanup();
        }
    }


    [TestClass]
    public class DrsrTestClassBase : DrsrTTTTestClassBase
    {
        public static IDRSRTestClient drsTestClient = new DRSRTestClient();
        protected ILdapAdapter ldapAdapter;
        protected UpdatesStorage updateStorage;
        protected IDrsrSutControlAdapter sutControlAdapter;

        #region Class Initialization and Cleanup
        public static void BaseInitialize(TestContext context)
        {
            TestClassBase.Initialize(context);
            BaseTestSite.DefaultProtocolDocShortName = BaseTestSite.Properties["ProtocolName"];
            drsTestClient.Initialize(BaseTestSite);
        }

        public static void BaseCleanup()
        {
            TestClassBase.Cleanup();

        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DrsrTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            DrsrTestClassBase.BaseCleanup();
        }
        #endregion



        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            ldapAdapter = BaseTestSite.GetAdapter<ILdapAdapter>();
            sutControlAdapter = BaseTestSite.GetAdapter<IDrsrSutControlAdapter>();
            updateStorage = UpdatesStorage.GetInstance();
            drsTestClient.Reset();
            EnvironmentConfig.ExpectSuccess = true;
            base.TestInitialize();
        }

        protected override void TestCleanup()
        {
            drsTestClient.Reset();
            base.TestCleanup();
        }
        #endregion
    }


}
