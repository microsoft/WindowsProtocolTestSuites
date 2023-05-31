// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.MS_XCA
{
    public class XcaTestConfig
    {
        public ITestSite Site
        {
            get;
            set;
        }

        public enum TestType { Compression = 1, Decompression = 2 };

        public XcaTestConfig(ITestSite site)
        {
            Site = site;
        }

        /// <summary>
        /// Get property value from grouped ptf config.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="checkNullOrEmpty">Check if the property is null or the value is empty.</param>
        /// <returns>The value of the property.</returns>
        public string GetProperty(string groupName, string propertyName, bool checkNullOrEmpty = true)
        {
            string propertyValue = Site.Properties[groupName + "." + propertyName];

            if (checkNullOrEmpty && string.IsNullOrEmpty(propertyValue))
            {
                if (propertyValue == null)
                {
                    Site.Assert.Inconclusive("The property {0} does not exist.", propertyName);
                }
                else
                {
                    Site.Assert.Inconclusive("The value of {0} is empty.", propertyName);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get property value from grouped ptf config.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="checkNullOrEmpty">Check if the property is null or the value is empty.</param>
        /// <returns>The value of the property.</returns>
        public string GetProperty(string propertyName, bool checkNullOrEmpty = true)
        {
            return GetProperty("XCA", propertyName, checkNullOrEmpty);
        }

        protected List<T> ParsePropertyToList<T>(string property, string groupName = "XCA") where T : struct
        {
            List<T> list = new List<T>();
            string propertyValue = GetProperty(groupName, property, false);
            if (string.IsNullOrEmpty(propertyValue)) return list;

            string[] values = propertyValue.Split(';');
            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    list.Add(ParsePropertyToEnum<T>(value, property));
                }
            }

            return list;
        }

        protected T ParsePropertyToEnum<T>(string propertyValue, string propertyName) where T : struct
        {
            T result;
            if (!Enum.TryParse(propertyValue, out result))
            {
                // Should fail the case if value is not filled correctly
                Site.Assume.Fail("{0} is not a valid value of {1}.", propertyValue, propertyName);
            }
            return result;
        }

    }
}
