// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    public abstract class RdpTestClassBase : TestClassBase
    {
        #region Adapter Instances

        protected TestConfig testConfig;
        protected IRDPSUTControlAdapter sutControlAdapter;
        #endregion

        #region Class Initialization and Cleanup

        public static void BaseInitialize(TestContext context)
        {
            TestClassBase.Initialize(context);
            BaseTestSite.DefaultProtocolDocShortName = BaseTestSite.Properties["ProtocolName"];
        }

        public static void BaseCleanup()
        {
            TestClassBase.Cleanup();
        }
        #endregion

        protected ITestSite TestSite
        {
            get
            {
                return BaseTestSite;
            }
        }

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            testConfig = new TestConfig(Site);
            this.sutControlAdapter = this.TestSite.GetAdapter<IRDPSUTControlAdapter>();
            CheckPlatformCompatibility();
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }

        /// <summary>
        /// Check platform compatibility.
        /// </summary>
        private void CheckPlatformCompatibility()
        {
            // Check CredSSP, which is currently only supported on Windows.
            if (testConfig.transportProtocol == EncryptedProtocol.NegotiationCredSsp || 
                testConfig.transportProtocol == EncryptedProtocol.DirectCredSsp)
            {
                if (!OperatingSystem.IsWindows())
                {
                    BaseTestSite.Assume.Inconclusive("The transport protocols based on CredSSP are only supported on Windows.");
                }
            }
        }
        #endregion

        /// <summary>
        /// Check For Compatibility Of Operating System For Large Pointer Through Fast Path (Only Available From Server 2022).
        /// </summary>
        public void LargePointerCompatibility()
        {
            if (testConfig.windowsOSVersion.Contains("Windows Server") &&
                testConfig.windowsOSBuildNumber < 20000)
            {
                BaseTestSite.Assume.Inconclusive("Large pointer capability is only supported on Windows Server 2022.");
            }
        }

        /// <summary>
        /// Trigger Resizing Of Pointer On Server To Larger Size.
        /// </summary>
        public void PointerIncreaseSize()
        {
            int? iResult;

            try
            {
                iResult = sutControlAdapter?.PointerIncreaseSize(this.TestContext.TestName);
            }
            catch (Exception ex)
            {
                TestSite.Log.Add(LogEntryKind.Debug, "Exception happened during PointerIncreaseSize(): {0}.", ex);
                TestSite.Assert.Fail("Resizing the pointer failed");
                return;
            }

            if (iResult != null)
            {
                TestSite.Log.Add(LogEntryKind.Debug, "The result of PointerIncreaseSize is {0}.", iResult);
            }

            if (iResult != 1)
            {
                TestSite.Assert.Fail("Resizing the pointer failed");
            }
        }

        /// <summary>
        /// Move the Pointer To Different PositionOn Server.
        /// </summary>
        public void PointerTriggerMotion()
        {
            int? iResult;

            try
            {
                iResult = sutControlAdapter?.PointerTriggerMotion(this.TestContext.TestName);
            }
            catch (Exception ex)
            {
                TestSite.Log.Add(LogEntryKind.Debug, "Exception happened during PointerTriggerMotion(): {0}.", ex);
                TestSite.Assert.Fail("moving the pointer failed");
                return;
            }

            if (iResult != null)
            {
                TestSite.Log.Add(LogEntryKind.Debug, "The result of PointerTriggerMotion is {0}.", iResult);
            }

            if (iResult != 1)
            {
                TestSite.Assert.Fail("moving the pointer failed");
            }
        }

        /// <summary>
        /// Trigger Resizing Of Pointer On Server To Normal Size.
        /// </summary>
        public void PointerReverseToDefaultSize()
        {
            int? iResult;

            try
            {
                iResult = sutControlAdapter?.PointerReverseToDefaultSize(this.TestContext.TestName);
            }
            catch (Exception ex)
            {
                TestSite.Log.Add(LogEntryKind.Debug, "Exception happened during PointerReverseToDefaultSize(): {0}.", ex);
                TestSite.Assert.Fail("Resizing the pointer to the default size failed");
                return;
            }

            if (iResult != null)
            {
                TestSite.Log.Add(LogEntryKind.Debug, "The result of PointerReverseToDefaultSize is {0}.", iResult);
            }

            if (iResult != 1)
            {
                TestSite.Assert.Fail("Resizing the pointer to the default size failed");
            }

        }
    }
}