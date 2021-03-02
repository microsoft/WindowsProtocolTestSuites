// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.PTMService.Storage
{
    /// <summary>
    /// Storage pool options.
    /// </summary>
    public class StoragePoolOptions
    {
        /// <summary>
        /// Key-value pairs of all nodes in format of {name}-{path}.
        /// </summary>
        public IDictionary<string, string> Nodes { get; set; }
    }
}
