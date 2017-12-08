// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    public abstract class RdpTestClassBase : TestClassBase
    {
        #region Adapter Instances
        protected IRdpbcgrAdapter rdpbcgrAdapter;

        #endregion

        #region Variables

        protected EncryptedProtocol transportProtocol;
        protected requestedProtocols_Values requestProtocol;
        protected TimeSpan timeout;

        #endregion Variables

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

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            this.rdpbcgrAdapter = (IRdpbcgrAdapter)this.Site.GetAdapter(typeof(IRdpbcgrAdapter));

            this.rdpbcgrAdapter.Reset();
            LoadConfig();

        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region Private Methods

        protected void LoadConfig()
        {
            #region Security Approach and Protocol
            string strRDPSecurityProtocol;
            bool isNegotiationBased = true;
            if (!PtfPropUtility.GetBoolPtfProperty(Site, RdpPtfPropNames.RdpSecurityNegotiation, out isNegotiationBased))
            {
                assumeFailForInvalidPtfProp(RdpPtfPropNames.RdpSecurityNegotiation);
            }

            requestProtocol = requestedProtocols_Values.PROTOCOL_RDP_FLAG;
            if (!PtfPropUtility.GetStringPtfProperty(Site, RdpPtfPropNames.RdpSecurityProtocol, out strRDPSecurityProtocol))
            {
                assumeFailForInvalidPtfProp(RdpPtfPropNames.RdpSecurityProtocol);
            }
            else
            {//TLS, CredSSP, or RDP
                if (strRDPSecurityProtocol.Equals("TLS", StringComparison.CurrentCultureIgnoreCase))
                {
                    requestProtocol = requestedProtocols_Values.PROTOCOL_SSL_FLAG;
                    this.Site.Assume.IsTrue(
                        isNegotiationBased,
                        "When TLS is used as the security protocol, {0} is set to 'TLS', {1} must be true.",
                        RdpPtfPropNames.RdpSecurityProtocol,
                        RdpPtfPropNames.RdpSecurityNegotiation);
                    transportProtocol = EncryptedProtocol.NegotiationTls;
                }
                else if (strRDPSecurityProtocol.Equals("CredSSP", StringComparison.CurrentCultureIgnoreCase))
                {
                    requestProtocol = requestedProtocols_Values.PROTOCOL_HYBRID_FLAG;
                    if (isNegotiationBased)
                    {
                        transportProtocol = EncryptedProtocol.NegotiationCredSsp;
                    }
                    else
                    {
                        transportProtocol = EncryptedProtocol.DirectCredSsp;
                    }
                }
                else if (strRDPSecurityProtocol.Equals("RDP", StringComparison.CurrentCultureIgnoreCase))
                {
                    requestProtocol = requestedProtocols_Values.PROTOCOL_RDP_FLAG;
                    this.Site.Assume.IsTrue(
                        isNegotiationBased,
                        "When RDP is used as the security protocol, {0} is set to 'RDP', {1} must be true.",
                        RdpPtfPropNames.RdpSecurityProtocol,
                        RdpPtfPropNames.RdpSecurityNegotiation);
                    transportProtocol = EncryptedProtocol.Rdp;
                }
                else
                {
                    assumeFailForInvalidPtfProp(RdpPtfPropNames.RdpSecurityProtocol);
                }
            }

            #region WaitTime
            string strWaitTime = Site.Properties["WaitTime"];
            if (strWaitTime != null)
            {
                int waitSeconds = Int32.Parse(strWaitTime);
                timeout = new TimeSpan(0, 0, waitSeconds);
            }
            #endregion

            #endregion
        }

        //override, assume fail for an invalid PTF property.
        private void assumeFailForInvalidPtfProp(string propName)
        {
            this.Site.Assume.Fail("The property \"{0}\" is invalid or not present in PTFConfig file!", propName);
        }

        #endregion Private Methods
    }
}