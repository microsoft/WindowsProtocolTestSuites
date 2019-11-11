// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.Rdp
{
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
    }
}
