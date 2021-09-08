// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

namespace Microsoft.Protocols.TestManager.PTMService.Common.Profile
{
    public class ProfileRequest
    {
        /// <summary>
        /// Configuration id of profile to import / export.
        /// </summary>
        public int ConfigurationId { get; set; }

        /// <summary>
        /// Test suite id.
        /// </summary>
        public int TestSuiteId { get; set; }

        /// <summary>
        /// File name with extension.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Stream content of profile package when using profile import.
        /// </summary>
        public Stream Stream { get; set; }
    }
}