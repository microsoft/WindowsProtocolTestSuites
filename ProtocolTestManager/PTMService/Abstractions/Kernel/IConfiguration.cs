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
        /// Selected rules
        /// </summary>
        public IEnumerable<RuleGroup> SelectedRules { get; }

        /// <summary>
        /// The properties of configuration.
        /// </summary>
        public IEnumerable<PropertyGroup> Properties { get; set; }

        /// <summary>
        /// The adapters of configuration.
        /// </summary>
        public IEnumerable<Adapter> Adapters { get; set; }

        /// <summary>
        /// Get applicable test cases of configuration.
        /// </summary>
        /// <returns>All the applicable test cases.</returns>
        public IEnumerable<string> GetApplicableTestCases();
    }
}
