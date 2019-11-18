// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    /// <summary>
    /// Load and store the ptf properties
    /// </summary>
    public class TestConfig
    {
        public TimeSpan timeout;
        public string domain;
        public string serverName;
        public int serverPort;
        public string userName;
        public string password;
        public string localAddress;
        public bool verifyPduEnabled;
        public bool verifyShouldBehaviors;
        public SslProtocols tlsVersion = SslProtocols.None;
        public bool isWindowsImplementation;
        public EncryptedProtocol transportProtocol;
        public requestedProtocols_Values requestProtocol;
        public Version rdpVersion;
        public bool isEDYCSupported;

        public bool isELESupported;
        public bool issueTemporaryLicenseForTheFirstTime;

        public ITestSite Site
        {
            get;
            set;
        }

        public TestConfig(ITestSite site)
        {
            Site = site;
            LoadConfiguation();
        }

        /// <summary>
        /// Load configuration from PTF config file
        /// </summary>
        private void LoadConfiguation()
        {
            string tempStr;

            if (!PtfPropUtility.GetStringPtfProperty(Site, "RDP.ServerName", out serverName))
            {
                AssumeFailForInvalidPtfProp("RDP.ServerName");
            }

            if (PtfPropUtility.GetStringPtfProperty(Site, "RDP.ServerDomain", out tempStr))
            {
                if (tempStr != null && tempStr.Length > 0)
                {
                    this.domain = tempStr;
                }
                else
                {
                    this.domain = this.serverName;
                }
            }

            if (!PtfPropUtility.GetIntPtfProperty(Site, "RDP.ServerPort", out serverPort))
            {
                AssumeFailForInvalidPtfProp("RDP.ServerPort");
            }

            if (!PtfPropUtility.GetStringPtfProperty(Site, "RDP.ServerUserName", out userName))
            {
                AssumeFailForInvalidPtfProp("RDP.ServerUserName");
            }

            if (!PtfPropUtility.GetStringPtfProperty(Site, "RDP.ServerUserPassword", out password))
            {
                AssumeFailForInvalidPtfProp("RDP.ServerUserPassword");
            }

            if (!PtfPropUtility.GetStringPtfProperty(Site, "RDP.ClientName", out localAddress))
            {
                AssumeFailForInvalidPtfProp("RDP.ClientName");
            }

            int waitTime;
            if (!PtfPropUtility.GetIntPtfProperty(Site, "Timeout", out waitTime))
            {
                AssumeFailForInvalidPtfProp("Timeout");
            }
            timeout = new TimeSpan(0, 0, waitTime);

            PtfPropUtility.GetBoolPtfProperty(Site, "IsWindowsImplementation", out isWindowsImplementation);
            PtfPropUtility.GetBoolPtfProperty(Site, "VerifyRdpbcgrMessages", out verifyPduEnabled);
            PtfPropUtility.GetBoolPtfProperty(Site, "VerifyShouldBehaviors", out verifyShouldBehaviors);
            PtfPropUtility.GetBoolPtfProperty(Site, "RDPEDYCSupported", out isEDYCSupported);
            PtfPropUtility.GetBoolPtfProperty(Site, "RDPELESupported", out isELESupported);
            PtfPropUtility.GetBoolPtfProperty(Site, "IssueTemporaryLicenseForTheFirstTime", out issueTemporaryLicenseForTheFirstTime);

            if (PtfPropUtility.GetStringPtfProperty(Site, "RDP.Version", out tempStr))
            {
                rdpVersion = new Version(tempStr);
            }
            else
            {
                AssumeFailForInvalidPtfProp("RDP.Version");
            }

            #region Security Approach and Protocol
            string strRDPSecurityProtocol;
            string strRDPSecurityTlsVersion;
            bool isNegotiationBased = true;
            if (!PtfPropUtility.GetBoolPtfProperty(Site, "RDP.Security.Negotiation", out isNegotiationBased))
            {
                AssumeFailForInvalidPtfProp("RDP.Security.Negotiation");
            }

            requestProtocol = requestedProtocols_Values.PROTOCOL_RDP_FLAG;
            if (!PtfPropUtility.GetStringPtfProperty(Site, "RDP.Security.Protocol", out strRDPSecurityProtocol))
            {
                AssumeFailForInvalidPtfProp("RDP.Security.Protocol");
            }

            //TLS, CredSSP, or RDP
            if (strRDPSecurityProtocol.Equals("TLS", StringComparison.CurrentCultureIgnoreCase))
            {
                if (PtfPropUtility.GetStringPtfProperty(Site, "RDP.Security.TLS.Version", out strRDPSecurityTlsVersion))
                {
                    // TLS1.0, TLS1.1, TLS1.2 or None
                    if (strRDPSecurityTlsVersion.Equals("TLS1.0", StringComparison.CurrentCultureIgnoreCase))
                    {
                        tlsVersion = SslProtocols.Tls;
                    }
                    else if (strRDPSecurityTlsVersion.Equals("TLS1.1", StringComparison.CurrentCultureIgnoreCase))
                    {
                        tlsVersion = SslProtocols.Tls11;
                    }
                    else if (strRDPSecurityTlsVersion.Equals("TLS1.2", StringComparison.CurrentCultureIgnoreCase))
                    {
                        tlsVersion = SslProtocols.Tls12;
                    }
                    else if (strRDPSecurityTlsVersion.Equals("None", StringComparison.CurrentCultureIgnoreCase))
                    {
                        tlsVersion = SslProtocols.None;
                    }
                    else
                    {
                        this.Site.Log.Add(LogEntryKind.Comment, "TLS is used as security protocol and the TLS Version is {0}.", strRDPSecurityTlsVersion);
                        this.Site.Assume.Fail("When TLS is used as the security protocol, {0} must be one of TLS1.0, TLS1.1, or TLS1.2; actually it is set to {1}",
                            "RDP.Security.TLS.Version",
                            tlsVersion);
                    }
                }
                else
                {
                    AssumeFailForInvalidPtfProp("RDP.Security.TLS.Version");
                }

                requestProtocol = requestedProtocols_Values.PROTOCOL_SSL_FLAG;
                this.Site.Assume.IsTrue(
                    isNegotiationBased,
                    "When TLS is used as the security protocol, {0} is set to 'TLS', {1} must be true.",
                    "RDP.Security.Protocol",
                    "RDP.Security.Negotiation");
                transportProtocol = EncryptedProtocol.NegotiationTls;
            }
            else if (strRDPSecurityProtocol.Equals("CredSSP", StringComparison.CurrentCultureIgnoreCase))
            {
                requestProtocol = requestedProtocols_Values.PROTOCOL_SSL_FLAG | requestedProtocols_Values.PROTOCOL_HYBRID_FLAG | requestedProtocols_Values.PROTOCOL_HYBRID_EX;
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
                AssumeFailForInvalidPtfProp("RDP.Security.Protocol");
            }

            #endregion
        }

        //override, assume fail for an invalid PTF property.
        private void AssumeFailForInvalidPtfProp(string propName)
        {
            this.Site.Assume.Fail("The property \"{0}\" is invalid or not present in PTFConfig file!", propName);
        }
    }
}
