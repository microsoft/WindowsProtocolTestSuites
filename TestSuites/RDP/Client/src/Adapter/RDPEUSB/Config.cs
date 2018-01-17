// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools;
using System;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.Rdpeusb
{
    public partial class ConfigPropNames
    {
        public const string Timeout = "WaitTime";
        public const string IsWindowsImplementation = "IsWindowsImplementation";
        public const string RdpSecurityNegotiation = "RDP.Security.Negotiation";
        public const string RdpSecurityProtocol = "RDP.Security.Protocol";
        public const string RdpSecurityEncryptionLevel = "RDP.Security.Encryption.Level";
        public const string RdpSecurityEncryptionMethod = "RDP.Security.Encryption.Method";
    }

    public static class Config
    {
        private enum RdpSecurityProtocolValues
        {
            TLS,
            CredSSP,
            RDP
        }

        private enum EncryptionLevelValues
        {
            None,
            Low,
            Client,
            High,
            FIPS
        }




        public static TimeSpan Timeout { get; private set; }
        public static bool IsWindowsImplementation { get; private set; }
        public static bool IsNegotiationBased { get; private set; }
        public static selectedProtocols_Values SelectedProtocol { get; private set; }
        public static EncryptedProtocol TransportProtocol { get; private set; }
        public static EncryptionLevel EncLevel { get; private set; }
        public static EncryptionMethods EncMethod { get; private set; }
        public static TS_UD_SC_CORE_version_Values RdpVersion { get; private set; }


        public static void LoadConfig(ITestSite site)
        {
            Timeout = TimeSpan.FromSeconds(CommonUtility.GetIntProperty(site, ConfigPropNames.Timeout));

            IsWindowsImplementation = CommonUtility.GetBoolProperty(site, ConfigPropNames.IsWindowsImplementation);

            IsNegotiationBased = CommonUtility.GetBoolProperty(site, ConfigPropNames.RdpSecurityNegotiation);

            RdpSecurityProtocolValues rsp = CommonUtility.GetEnumProperty<RdpSecurityProtocolValues>(site, ConfigPropNames.RdpSecurityProtocol);
            switch (rsp)
            {
                case RdpSecurityProtocolValues.TLS:
                    SelectedProtocol = selectedProtocols_Values.PROTOCOL_SSL_FLAG;
                    site.Assume.IsTrue(
                        IsNegotiationBased,
                        "When TLS is used as the security protocol, {0} is set to 'TLS', {1} must be true.",
                        ConfigPropNames.RdpSecurityProtocol,
                        ConfigPropNames.RdpSecurityNegotiation);
                    TransportProtocol = EncryptedProtocol.NegotiationTls;
                    break;

                case RdpSecurityProtocolValues.CredSSP:
                    SelectedProtocol = selectedProtocols_Values.PROTOCOL_HYBRID_FLAG;
                    TransportProtocol = IsNegotiationBased ? EncryptedProtocol.NegotiationCredSsp : EncryptedProtocol.DirectCredSsp;
                    break;

                case RdpSecurityProtocolValues.RDP:
                    SelectedProtocol = selectedProtocols_Values.PROTOCOL_RDP_FLAG;
                    TransportProtocol = EncryptedProtocol.Rdp;
                    break;

                default:
                    site.Assume.Fail("The property '{0}' is not valid, it must be set to one of TLS, CredSSP or RDP.", ConfigPropNames.RdpSecurityProtocol);
                    break;
            }

            EncryptionLevelValues ev = CommonUtility.GetEnumProperty<EncryptionLevelValues>(site, ConfigPropNames.RdpSecurityEncryptionLevel);
            switch (ev)
            {
                case EncryptionLevelValues.None:
                    EncLevel = EncryptionLevel.ENCRYPTION_LEVEL_NONE;
                    break;
                case EncryptionLevelValues.Low:
                    EncLevel = EncryptionLevel.ENCRYPTION_LEVEL_LOW;
                    break;
                case EncryptionLevelValues.Client:
                    EncLevel = EncryptionLevel.ENCRYPTION_LEVEL_CLIENT_COMPATIBLE;
                    break;
                case EncryptionLevelValues.High:
                    EncLevel = EncryptionLevel.ENCRYPTION_LEVEL_HIGH;
                    break;
                case EncryptionLevelValues.FIPS:
                    EncLevel = EncryptionLevel.ENCRYPTION_LEVEL_FIPS;
                    break;
                default:
                    site.Assume.Fail("The property '{0}' is not valid, it MUST be one of None, Low, Client, High, FIPS.", ConfigPropNames.RdpSecurityEncryptionLevel);
                    break;
            }

            Dictionary<string, EncryptionMethods> encMethodValues = new Dictionary<string, EncryptionMethods>();
            encMethodValues.Add("None", EncryptionMethods.ENCRYPTION_METHOD_NONE);
            encMethodValues.Add("40bit", EncryptionMethods.ENCRYPTION_METHOD_40BIT);
            encMethodValues.Add("56bit", EncryptionMethods.ENCRYPTION_METHOD_56BIT);
            encMethodValues.Add("128bit", EncryptionMethods.ENCRYPTION_METHOD_128BIT);
            encMethodValues.Add("FIPS", EncryptionMethods.ENCRYPTION_METHOD_FIPS);

            string str = CommonUtility.GetProperty(site, ConfigPropNames.RdpSecurityEncryptionMethod);
            site.Assume.IsTrue(
                encMethodValues.ContainsKey(str),
                "The property '{0}' must be 'None', '40bit', '56bit', '128bit', or 'FIPS'.",
                ConfigPropNames.RdpSecurityEncryptionMethod);
            EncMethod = encMethodValues[str];

            RdpVersion = TS_UD_SC_CORE_version_Values.V2;

            ValidateConfig(site.Assume);
        }

        private static void ValidateConfig(IChecker assume)
        {
            if (EncLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                assume.IsTrue(
                    EncMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE,
                    "When Encryption Level is set to None, the Encryption Method must also set to None."
                    );
            }

            if (EncLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS)
            {
                assume.IsTrue(
                    EncMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS,
                    "When Encryption Level is set to FIPS, the Encryption Method must be set to FIPS."
                    );
            }

            if (TransportProtocol == EncryptedProtocol.Rdp)
            {
                assume.IsTrue(
                    EncLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE,
                    "When use standard RDP security, the encryption level must be greater than 'None'."
                    );
            }
            else
            {
                assume.IsTrue(
                    EncLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE,
                    "When use enhanced security protocols (TLS or CredSSP), the encryption level must be set to 'None'."
                    );
            }
        }
    }
}
