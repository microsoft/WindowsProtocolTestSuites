// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools;
using System;
using System.Linq;
using System.Reflection;

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
        public const string RdpSecurityNegotiation = "Negotiation";
        public const string RdpSecurityProtocol = "Protocol";
        public const string RdpSecurityEncryptionLevel = "Level";
        public const string RdpSecurityEncryptionMethod = "Method";
        public const string RDPClientSupportFastPathInput = "SupportFastPathInput";
        public const string RDPClientSupportAutoReconnect = "SupportAutoReconnect";
        public const string RDPClientSupportRDPEFS = "SupportRDPEFS";
        public const string RDPClientSupportServerRedirection = "SupportServerRedirection";
        public const string RDPClientSupportSoftSync = "SupportSoftSync";
        public const string RDPClientSupportTunnelingStaticVCTraffic = "SupportTunnelingStaticVCTraffic";
        public const string RDPClientSupportRdpNegDataEmpty = "SupportRdpNegDataEmpty";
        public const string MSRDPRFX_Image = "RDPRFXImage";
        public const string MSRDPRFX_VideoModeImage = "RDPRFXVideoModeImage";
        public const string SupportCompression = "SupportCompression";
    }

    public class RdpPtfGroupNames
    {
        public const string Security = "Security";
        public const string Encryption = "Encryption";
        public const string SUTControl = "SUTControl";
        public const string VerifySUTDisplay = "VerifySUTDisplay";
        public const string IQA = "IQA";
    }

    /// <summary>
    /// A helper class to get specified type of PTF properties.
    /// </summary>
    public class PtfPropUtility
    {
        private static string GetGroupPropName(string propName, string [] groupNames = null) {
            if (groupNames != null && groupNames.Length > 0)
            {
                string groupNamesString = string.Join(".", groupNames);
                return $"{groupNamesString}.{propName}";
            }
            else {
                return $"{propName}";
            }
        }

        /// <summary>
        /// Get a T type PTF property.
        /// </summary>
        /// <typeparam name="T">The property value type.</typeparam>
        /// <param name="testSite">The test site where to get from.</param>
        /// <param name="propName">The property name.</param>
        /// <param name="propStrValue">The output property value.</param>
        /// <param name="groupNames">The groups which the property locate.</param>
        /// <returns>true if value was converted successfully; otherwise, false.</returns>
        public static bool GetPtfPropertyValue<T>(ITestSite testSite, string propName, out T propValue, string[] groupNames = null) 
        {
            bool bSucceed = false;

            propValue = default(T);

            object propObjectValue = testSite.Properties[GetGroupPropName(propName, groupNames)];
            if (propObjectValue == null)
            {
                propValue = default(T);
                return false;
            }
            else 
            {
                Type t = typeof(T);
                if (t.FullName.Equals("System.String"))
                {
                    propValue = (T)propObjectValue;

                    bSucceed = true;
                }
                else
                {
                    var tryParse = t.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder
                    , new Type[] { propObjectValue.GetType(), t.MakeByRefType() }
                    , new ParameterModifier[] { new ParameterModifier(2) });

                    if (tryParse != null)
                    {
                        var parameters = new object[] { propObjectValue, Activator.CreateInstance(t) };
                        bSucceed = (bool)tryParse.Invoke(null, parameters);
                        if (bSucceed)
                        {
                            propValue = (T)parameters[1];
                        }
                        else
                        {
                            propValue = default(T);
                        }
                    }
                }
            }

            return bSucceed;
        }
    }
}
