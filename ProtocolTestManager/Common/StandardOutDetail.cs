// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.Common
{
    /// <summary>
    /// Represents a detailed StandardOut log
    /// </summary>
    public class StandardOutDetail
    {
        /// <summary>
        /// The type of the StandardOut log
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The content of the StandardOut log
        /// </summary>
        public string Content { get; set; }
    }
}
