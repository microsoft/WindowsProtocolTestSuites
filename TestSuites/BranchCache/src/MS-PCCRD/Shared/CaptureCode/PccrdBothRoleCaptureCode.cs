// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrd
{
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery;

    /// <summary>
    /// A class for requirement capture code of both role.
    /// </summary>
    public static class PccrdBothRoleCaptureCode
    {
        /// <summary>
        /// Capture common requirements shared by both of client role and server role.
        /// </summary>
        /// <param name="site">A instance of ITestSite.</param>
        public static void CaptureCommonRequirements(ITestSite site)
        {
            // Since the message parsed by stack layer properly, capture the requirement directly.
            site.CaptureRequirement(
                1,
                @"[In Transport] The Discovery Protocol uses the Web Services Dynamic Discovery (WS-Discovery)
                protocol[, and the actual transport protocol is abstracted by WSD].");

            // Since the message parsed by stack layer properly, capture the requirement directly.
            site.CaptureRequirement(
                13,
                @"[In Namespaces] The XML namespace URI that MUST be used by the implementation of the Discovery Protocol
                is: http://schemas.microsoft.com/p2p/2007/09/PeerDistributionDiscovery.");

            // Since the test suite use UDP as transport and the message parsed by stack layer properly, 
            // capture the requirement directly.
            site.CaptureRequirement(
                176,
                "PCCRD messages sent over UDP MUST be sent using SOAP over UDP [SOAP/UDP].");
        }

        /// <summary>
        /// Capture requirements for Type structure
        /// </summary>
        /// <param name="types">Type structure</param>
        /// <param name="site">A instance of ITestSite.</param>
        public static void CaptureTypesElementRequirements(string types, ITestSite site)
        {
            #region MS-PCCRD_R51
            // Add debug info
            site.Log.Add(LogEntryKind.Debug, "Types: {0}", types);

            site.CaptureRequirementIfAreEqual<string>(
                "PeerDist:PeerDistData",
                types,
                51,
                "[In Types] This element [Types] MUST be set to: PeerDist:PeerDistData.");

            #endregion
        }

        /// <summary>
        /// Capture requirements for Scope structure
        /// </summary>
        /// <param name="scopes">Scope structure</param>
        /// <param name="site">A instance of ITestSite.</param>
        public static void CaptureScopesElementRequirements(ScopesType scopes, ITestSite site)
        {
            // Add debug info
            site.Log.Add(LogEntryKind.Debug, "Scopes: {0}", scopes);

            site.CaptureRequirementIfIsNotNull(
                scopes,
                55,
                @"[In Scopes] Each element in the list is actually a UTF-8-encoded string representation of 
                the HoHoDk value in hexBinary format [XMLSCHEMA1.1/2] [and represents one segment].");
           
            // Add debug info
            site.Log.Add(LogEntryKind.Debug, "Scopes: {0}", scopes);
            site.CaptureRequirementIfIsTrue(
                scopes.Text.Length > 0,
                158,
                @"[The <Scopes> element represents the list of discovery provider scopes, where]
                each element in the list is a string.");
        }
    }
}
