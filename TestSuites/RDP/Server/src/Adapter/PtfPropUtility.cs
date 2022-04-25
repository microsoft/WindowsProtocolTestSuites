// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.Rdp
{
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
        /// <summary>
        /// Get a string type PTF property.
        /// </summary>
        /// <param name="testSite">The test site where to get from.</param>
        /// <param name="propName">The property name.</param>
        /// <param name="propStrValue">The output property value.</param>
        /// <returns>true if value was converted successfully; otherwise, false.</returns>
        public static bool GetStringPtfProperty(ITestSite testSite, string propName, out string propStrValue)
        {
            string propValue = testSite.Properties[propName];
            if (propValue == null)
            {
                propStrValue = string.Empty;
                return false;
            }
            else
            {
                propStrValue = propValue;
                return true;
            }

        }

        /// <summary>
        /// Get a string type PTF property.
        /// </summary>
        /// <param name="testSite">The test site where to get from.</param>
        /// <param name="propName">The property name.</param>
        /// <param name="propIntValue">The output property value.</param>
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
        /// <param name="propUintValue">The output property value.</param>
        /// <returns>true if value was converted successfully; otherwise, false.</returns>
        public static bool GetUIntPtfProperty(ITestSite testSite, string propName, out uint propUintValue)
        {
            bool bSucceed = false;

            string propValue = testSite.Properties[propName];
            if (propValue == null)
            {
                propUintValue = 0;
                return false;
            }
            else
            {
                bSucceed = uint.TryParse(propValue, out propUintValue);
            }

            return bSucceed;
        }

        private static string GetGroupPropName(string propName, string[] groupNames = null)
        {
            if (groupNames != null && groupNames.Length > 0)
            {
                string groupNamesString = string.Join(".", groupNames);
                return $"{groupNamesString}.{propName}";
            }
            else
            {
                return $"{propName}";
            }
        }

        /// <summary>
        /// Get a string type PTF property.
        /// </summary>
        /// <param name="testSite">The test site where to get from.</param>
        /// <param name="propName">The property name.</param>
        /// <param name="propBoolValue">The output property value.</param>
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
