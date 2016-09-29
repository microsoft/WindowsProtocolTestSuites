// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrtp
{
    using System.Collections.Generic;
    using Microsoft.Protocols.TestTools;

    /// <summary>
    /// This class is used to verify the PCCRTP both role requirements.
    /// </summary>
    internal static class PccrtpBothRoleCapture
    {
        #region Fields

        /// <summary>
        /// An instance of ITestSite used to capture requirements.
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
        /// Verify both role requirements about HTTP header.
        /// </summary>
        /// <param name="httpHeader">Indicates the pair of key and value of the HTTP request or response header.</param>
        public static void VerifyPccrtpCommonHeader(Dictionary<string, string> httpHeader)
        {
            #region MS-PCCRTP_R6

            bool isVerifyR6 = httpHeader.ContainsKey("X-P2P-PeerDist") 
                && !string.IsNullOrEmpty(httpHeader["X-P2P-PeerDist"]);

            // Add the debug information
            site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRTP_R6. The header field {0} contain X-P2P-PeerDist field.",
                isVerifyR6 ? string.Empty : "does not ");

            // Verify MS-PCCRTP requirement: MS-PCCRTP_R6
            site.CaptureRequirementIfIsTrue(
                isVerifyR6,
                6,
                @"[Message Syntax]The syntax of this header[extension-header] field value is described as follows
                [extension-header = X-P2P-PeerDist
                X-P2P-PeerDist = ""X-P2P-PeerDist"" "":"" peerdist-params].");

            #endregion

            #region MS-PCCRTP_R13

            string[] headerValues = httpHeader["X-P2P-PeerDist"].Split(new char[] { ',' });
            string versionString = headerValues[0].Substring(headerValues[0].IndexOf('=') + 1);

            // Add the debug information
            site.Log.Add(
                LogEntryKind.Debug,
                "Verify MS-PCCRTP_R13. Actual value of version is: {0}.",
                versionString);

            bool isVerifyR13 = versionString.Contains(".") && !versionString.Contains(" ");

            // Verify MS-PCCRTP requirement: MS-PCCRTP_R13
            site.CaptureRequirementIfIsTrue(
                isVerifyR13,
                13,
                @"[In Message Syntax] Note that there can be no spaces between major-version and ""."" as well as ""."" 
                and minor-version [version = ""Version"" ""="" major-version ""."" minor-version].");

            #endregion

            #region MS-PCCRTP_R14

            string[] versionStrValues = headerValues[0].Split(new char[] { '.', '=' });
            int majorValue;
            int minorValue;
            string marjorVersionStr = versionStrValues[1];
            string minorVersionStr = versionStrValues[2];
            bool isVerifyR14 = int.TryParse(marjorVersionStr, out majorValue)
                && int.TryParse(minorVersionStr, out minorValue);

            // Add the debug information
            site.Log.Add(
                LogEntryKind.Debug,
                "Verify MS-PCCRTP_R14. Actual value of version is: {0}.",
                headerValues[0]);

            // Verify MS-PCCRTP requirement: MS-PCCRTP_R14
            site.CaptureRequirementIfIsTrue(
                isVerifyR14,
                14,
                @"[In Message Syntax] The major and minor versions MUST be considered as separate multidigit numbers
                [major-version = 1*DIGIT
                minor-version = 1*DIGIT].");

            #endregion
        }

        /// <summary>
        /// Verify requirements about transport.
        /// </summary>
        /// <param name="protocolVersion">The HTTP protocol version.</param>
        public static void VerifyTransport(string protocolVersion)
        {
            #region MS-PCCRTP_R4

            // Add the debug information
            site.Log.Add(
                LogEntryKind.Debug,
                "Verify MS-PCCRTP_R4. Actual value of HTTP version is: {0}.",
                protocolVersion);

            // Verify MS-PCCRTP requirement: MS-PCCRTP_R4
            site.CaptureRequirementIfAreEqual<string>(
                "1.1",
                protocolVersion,
                4,
                @"[In Transport] HTTP/1.1 is the primary transport for all messages used 
                by the PeerDist content encoding.");

            #endregion
        }

        #endregion
    }
}