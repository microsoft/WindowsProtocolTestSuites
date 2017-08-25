// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.Rdp
{
    /// <summary>
    /// Define the ptf property names.
    /// </summary>
    public class RdpPtfPropNames
    {
        public const string Timeout = "WaitTime";
        public const string IsWindowsImplementation = "IsWindowsImplementation";
        public const string DropConnectionForInvalidRequest = "DropConnectionForInvalidRequest";
        public const string RdpSecurityNegotiation = "RDP.Security.Negotiation";
        public const string RdpSecurityProtocol = "RDP.Security.Protocol";
        public const string RdpSecurityEncryptionLevel = "RDP.Security.Encryption.Level";
        public const string RdpSecurityEncryptionMethod = "RDP.Security.Encryption.Method";
        public const string RDPClientSupportFastPathInput = "RDP.Client.SupportFastPathInput";
        public const string RDPClientSupportAutoReconnect = "RDP.Client.SupportAutoReconnect";
        public const string RDPClientSupportRDPEFS = "RDP.Client.SupportRDPEFS";
        public const string RDPClientSupportServerRedirection = "RDP.Client.SupportServerRedirection";
        public const string RDPClientSupportSoftSync = "RDP.Client.SupportSoftSync";
        public const string RDPClientSupportTunnelingStaticVCTraffic = "RDP.Client.SupportTunnelingStaticVCTraffic";
        public const string RDPClientSupportRdpNegDataEmpty = "RDP.Client.SupportRdpNegDataEmpty";
        public const string MSRDPRFX_64X64BitmapFile = "MSRDPRFX_64X64BitmapFile";
        public const string MSRDPRFX_BitmapFileForVideoMode = "MSRDPRFX_BitmapFileForVideoMode";
    }

    /// <summary>
    /// A helper class to get specified type of PTF properties.
    /// </summary>
    public class PtfPropUtility
    {
        /// <summary>
        /// Get a string type PTF property.
        /// </summary>
        /// <param name="testSite">The test site where to get from.</param>
        /// <param name="propName">The property name.</param>
        /// <param name="propStrValue">The output property value.</param>
        /// <returns>true if value was converted successfully; otherwise, false.</returns>
        public static bool GetStringPtfProperty(ITestSite testSite, string propName, out string propStrValue)
        {
            bool bSucceed = false;

            string propValue = testSite.Properties[propName];
            if (propValue == null)
            {
                propStrValue = string.Empty;
                return false;
            }
            else
            {
                propStrValue = propValue;
                bSucceed = true;
            }

            return bSucceed;
        }

        /// <summary>
        /// Get a string type PTF property.
        /// </summary>
        /// <param name="testSite">The test site where to get from.</param>
        /// <param name="propName">The property name.</param>
        /// <param name="propStrValue">The output property value.</param>
        /// <returns>true if value was converted successfully; otherwise, false.</returns>
        public static bool GetIntPtfProperty(ITestSite testSite, string propName, out int propIntValue)
        {
            bool bSucceed = false;

            string propValue = testSite.Properties[propName];
            if (propValue == null)
            {
                propIntValue = 0;
                return false;
            }
            else
            {
                bSucceed = int.TryParse(propValue, out propIntValue);
            }

            return bSucceed;
        }

        /// <summary>
        /// Get a string type PTF property.
        /// </summary>
        /// <param name="testSite">The test site where to get from.</param>
        /// <param name="propName">The property name.</param>
        /// <param name="propStrValue">The output property value.</param>
        /// <returns>true if value was converted successfully; otherwise, false.</returns>
        public static bool GetUIntPtfProperty(ITestSite testSite, string propName, out uint propIntValue)
        {
            bool bSucceed = false;

            string propValue = testSite.Properties[propName];
            if (propValue == null)
            {
                propIntValue = 0;
                return false;
            }
            else
            {
                bSucceed = uint.TryParse(propValue, out propIntValue);
            }

            return bSucceed;
        }

        /// <summary>
        /// Get a string type PTF property.
        /// </summary>
        /// <param name="testSite">The test site where to get from.</param>
        /// <param name="propName">The property name.</param>
        /// <param name="propStrValue">The output property value.</param>
        /// <returns>true if value was converted successfully; otherwise, false.</returns>
        public static bool GetBoolPtfProperty(ITestSite testSite, string propName, out bool propBoolValue)
        {
            bool bSucceed = false;

            string propValue = testSite.Properties[propName];
            if (propValue == null)
            {
                propBoolValue = false;
                return false;
            }
            else
            {
                bSucceed = bool.TryParse(propValue, out propBoolValue);
            }

            return bSucceed;
        }
    }
}
