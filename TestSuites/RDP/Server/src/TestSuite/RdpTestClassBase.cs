// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdpedyc;
using Microsoft.Protocols.TestSuites.Rdpemt;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    public abstract class RdpTestClassBase : TestClassBase
    {
        #region Adapter Instances
        protected IRdpbcgrAdapter rdpbcgrAdapter;
        protected IRdpemtAdapter rdpemtAdapter;
        protected IRdpedycAdapter rdpedycAdapter;

        #endregion

        #region Variables

        protected EncryptedProtocol transportProtocol;
        protected requestedProtocols_Values requestProtocol;
        protected bool isWindowsImplementation;
        protected TimeSpan timeout;

        protected Version rdpVersion;

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

            this.rdpemtAdapter = (IRdpemtAdapter)this.Site.GetAdapter(typeof(IRdpemtAdapter));

            this.rdpemtAdapter.Reset();

            LoadConfig();

           
            this.rdpedycAdapter = (IRdpedycAdapter)this.Site.GetAdapter(typeof(IRdpedycAdapter));

            this.rdpedycAdapter.Reset();
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
                    string strTlsVersion;
                    if (PtfPropUtility.GetStringPtfProperty(Site, RdpPtfPropNames.RdpSecurityTlsVersion, out strTlsVersion))
                    {
                        if (!strTlsVersion.Equals("TLS1.0", StringComparison.CurrentCultureIgnoreCase) &&
                            !strTlsVersion.Equals("TLS1.1", StringComparison.CurrentCultureIgnoreCase) &&
                            !strTlsVersion.Equals("TLS1.2", StringComparison.CurrentCultureIgnoreCase))
                        {
                            this.Site.Assume.Fail("When TLS is used as the security protocol, {0} must be one of TLS1.0, TLS1.1, or TLS1.2; actually it is set to {1}",
                                RdpPtfPropNames.RdpSecurityTlsVersion,
                                strTlsVersion);
                        }
                        else
                        {
                            this.Site.Log.Add(LogEntryKind.Comment, "TLS is used as security protocol and the TLS Version is {0}.", strTlsVersion);
                        }
                    }
                    else
                    {
                        assumeFailForInvalidPtfProp(RdpPtfPropNames.RdpSecurityTlsVersion);
                    }
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
                    transportProtocol = EncryptedProtocol.Rdp;
                }
                else
                {
                    assumeFailForInvalidPtfProp(RdpPtfPropNames.RdpSecurityProtocol);
                }
            }

            #region Is Windows Implementation
            string strIsWindows = Site.Properties["IsWindowsImplementation"];
            if (strIsWindows != null)
            {
                isWindowsImplementation = Boolean.Parse(strIsWindows);
            }
            #endregion

            #region WaitTime
            string strWaitTime = Site.Properties["WaitTime"];
            if (strWaitTime != null)
            {
                int waitSeconds = Int32.Parse(strWaitTime);
                timeout = new TimeSpan(0, 0, waitSeconds);
            }
            #endregion

            #endregion


            string rdpVersionProp = this.Site.Properties["RDP.Version"];
            if (!string.IsNullOrEmpty(rdpVersionProp))
            {
                rdpVersion = new Version(rdpVersionProp);
            }
            else
            {
                this.Site.Assume.Fail("The property \"{0}\" is invalid or not present in PTFConfig file!", "RDP.Version");
            }
        }

        //override, assume fail for an invalid PTF property.
        private void assumeFailForInvalidPtfProp(string propName)
        {
            this.Site.Assume.Fail("The property \"{0}\" is invalid or not present in PTFConfig file!", propName);
        }

        #endregion Private Methods
    }
}