// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel
{
    /// <summary>
    /// Interface of configuration.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// The Id of configuration.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The name of configuration.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of configuration.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The related test suite.
        /// </summary>
        public ITestSuite TestSuite { get; }

        /// <summary>
        /// The storage root of configuration.
        /// </summary>
        public IStorageNode StorageRoot { get; }

        /// <summary>
        /// The rules of configuration.
        /// </summary>
        public IEnumerable<RuleGroup> Rules { get; set; }

        /// <summary>
        /// The index of Target Filter.
        /// </summary>
        public int TargetFilterIndex { get; }

        /// <summary>
        /// The index of Mapping Filter.
        /// </summary>
        public int MappingFilterIndex { get; }

        /// <summary>
        /// A table which helps lookup rules in mapping filter by category name in target filter
        /// Key: category name of a rule in target filter
        /// Value: list of rules in mapping filter
        /// </summary>
        public Dictionary<string, List<Rule>> FeatureMappingTable { get; }

        /// <summary>
        /// A table which helps lookup rules in target filter by category name in mapping filter
        /// Key: category name of a rule in mapping filter
        /// Value: list of rules in target filter
        /// </summary>
        public Dictionary<string, List<Rule>> ReverseMappingTable { get; }

        /// <summary>
        /// Selected rules
        /// </summary>
        public IEnumerable<RuleGroup> SelectedRules { get; }

        /// <summary>
        /// Get the properties of configuration.
        /// </summary>
        public IEnumerable<PropertyGroup> GetProperties(IAutoDetection detector);

        /// <summary>
        /// Set the properties of configuration.
        /// </summary>
        public void SetProperties(IEnumerable<PropertyGroup> groups);

        /// <summary>
        /// The adapters of configuration.
        /// </summary>
        public IEnumerable<Adapter> Adapters { get; set; }

        /// <summary>
        /// Get applicable test cases of configuration.
        /// </summary>
        /// <returns>All the applicable test cases.</returns>
        public IEnumerable<string> GetApplicableTestCases();

        /// <summary>
        /// Indicate whether current configuration has been configured.
        /// </summary>
        public bool IsConfigured { get; }
    }
}
