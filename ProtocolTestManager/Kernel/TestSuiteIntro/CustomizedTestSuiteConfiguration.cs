// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// Customized test suite configuration item.
    /// </summary>
    public class CustomizedTestSuiteConfigurationItem
    {
        /// <summary>
        /// Test suite name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Test suite location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Test suite version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Test suite dotnet target, where true indicates dotnet core. 
        /// </summary>
        public Boolean IsCore { get; set; }
    }

    /// <summary>
    /// Customized test suite configuration.
    /// </summary>
    class CustomizedTestSuiteConfiguration
    {
        /// <summary>
        /// Load configuration.
        /// </summary>
        /// <param name="configFilePath">The path to configuration file.</param>
        /// <returns>The loaded configuration items.</returns>
        public static IEnumerable<CustomizedTestSuiteConfigurationItem> Load(string configFilePath)
        {
            var serializer = new JavaScriptSerializer();

            var content = File.ReadAllText(configFilePath);

            var items = serializer.Deserialize<CustomizedTestSuiteConfigurationItem[]>(content);

            return items;
        }

        /// <summary>
        /// Save configuration.
        /// </summary>
        /// <param name="configFilePath">The path to configuration file.</param>
        /// <param name="items">The configuration items to save.</param>
        public static void Save(string configFilePath, IEnumerable<CustomizedTestSuiteConfigurationItem> items)
        {
            var serializer = new JavaScriptSerializer();

            var content = serializer.Serialize(items.ToArray());

            File.WriteAllText(configFilePath, content);
        }
    }
}
