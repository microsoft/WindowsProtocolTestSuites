// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter
{
    public class FSATestConfig : TestConfigBase
    {
        public FSATestConfig(ITestSite site) : base(site)
        {
        }

        /// <summary>
        /// Get property value from grouped ptf config.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="checkNullOrEmpty">Check if the property is null or the value is empty.</param>
        /// <returns>The value of the property.</returns>
        public string GetProperty(string propertyName, bool checkNullOrEmpty = true)
        {
            return GetProperty("FSA", propertyName, checkNullOrEmpty);
        }
    }
}
