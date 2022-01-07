// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestManager.WSPServerPlugin
{
    /// <summary>
    /// The detection information
    /// </summary>
    public class DetectionInfo
    {
        // Parameters for Detecting
        public string ServerComputerName { get; set; } = "SutComputer";

        public string ServerVersion { get; set; }

        public string ServerOffset { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string DomainName { get; set; }

        public string SupportedSecurityPackage { get; set; }

        public bool UseServerGssToken { get; set; }

        public TimeSpan SMB2ClientTimeout { get; set; }

        public string ShareName { get; set; }

        public string QueryPath { get; set; }

        public string QueryText { get; set; }

        public string CatalogName { get; set; }

        public string ClientName { get; set; }

        public string ClientOffset { get; set; }

        public string ClientVersion { get; set; }

        public bool IsWDSInstalled { get; set; }

        public bool IsServerWindows { get; set; }

        public string LanguageLocale { get; set; }

        public string LcidValue { get; set; }
    }
}
