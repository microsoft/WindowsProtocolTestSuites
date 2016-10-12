// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pchc
{
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;

    /// <summary>
    /// Verify the both client and server role requirements of MS-PCHC.
    /// </summary>
    public static class PchcBothRoleCaptureCode
    {
        #region Fields

        /// <summary>
        /// An instance of ITestSite used for capture requirements.
        /// </summary>
        private static ITestSite site;

        #endregion

        #region Initialize

        /// <summary>
        /// Initialize ITestSite instance.
        /// </summary>
        /// <param name="testSite">The adapter's ITestSite instance.</param>
        public static void Initialize(ITestSite testSite)
        {
            site = testSite;
        }

        #endregion

        #region Capture Requirements

        /// <summary>
        /// Validate the transport between the client and the hosted cahce.
        /// </summary>
        /// <param name="transProtocol">The transport protocol.</param>
        public static void ValidateTransport(string transProtocol)
        {
            // Add the debug information
            site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R1:The transport protocol {0}",
                "HTTPS");

            // Capture MS-PCHC R1
            site.CaptureRequirementIfAreEqual<string>(
                "https",
                transProtocol.ToLower(),
                1,
                @"[In Transport] The Peer Content Caching and Retrieval: Hosted Cache Protocol uses 
                a client/server transport built on top of Hypertext Transfer Protocol (HTTP) over 
                Transport Layer Security (TLS) (HTTPS) [RFC2818].");
        }

        #endregion
    }
}
