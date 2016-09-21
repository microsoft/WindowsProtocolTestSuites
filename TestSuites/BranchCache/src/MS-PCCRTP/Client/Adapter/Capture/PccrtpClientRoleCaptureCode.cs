// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrtp
{
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;

    /// <summary>
    /// This class is used to verify the requirements related to PCCRTP request.
    /// </summary>
    public partial class PCCRTPClientAdapter
    {
        /// <summary>
        /// Verify requirements about PCCRTP request.
        /// </summary>
        private void VerifyPccrtpRequestRequirements()
        {
            #region MS-PCCRTP_R49

            if (this.sutOsVersion == OSVersion.WinVista
                || this.sutOsVersion == OSVersion.Win7
                || this.sutOsVersion == OSVersion.Win2K8
                || this.sutOsVersion == OSVersion.Win2K8R2)
            {
                // Add the debug information
                this.Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRTP_R49");

                // Verify MS-PCCRTP requirement: MS-PCCRTP_R49
                this.Site.CaptureRequirementIfAreEqual<string>(
                    @"GET",
                    this.pccrtpRequest.HttpRequest.HttpMethod.ToUpper(),
                    49,
                    @"[In Appendix A: Product Behavior] <4> Section 3.1.4: HTTP/1.1 clients 
                    in Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2 
                    use the PeerDist content encoding for GET requests only.");
            }

            #endregion

            object peerDistParam = this.pccrtpRequest.HttpRequest.Headers[PCCRTPClientAdapter.XP2PPEERDIST];
            string peerDistValue = peerDistParam.ToString();
            string[] peerDist = peerDistValue.Split(new char[] { ',' });
            string[] version = peerDist[0].Split(new char[] { '=' });
            
            if (this.pccrtpRequest.HttpRequest.Headers[PCCRTPClientAdapter.ACCEPTENCODING] != null)
            {
                #region MS-PCCRTP_R8

                // Add the debug information
                this.Site.Log.Add(
                    LogEntryKind.Debug,
                    "Verify MS-PCCRTP_R8 : [Accept-Encoding={0}].",
                    this.pccrtpRequest.HttpRequest.Headers[PCCRTPClientAdapter.ACCEPTENCODING]);

                // Verify MS-PCCRTP requirement: MS-PCCRTP_R8
                bool isVerifyR8 = this.pccrtpRequest.HttpRequest.Headers[PCCRTPClientAdapter.ACCEPTENCODING].Contains("peerdist");

                // Capture MS-PCCRTP_R8
                this.Site.CaptureRequirementIfIsTrue(
                    isVerifyR8,
                    8,
                    @"[In Message Syntax] The PeerDist content-encoding value can be specified 
                    in the Accept-Encoding and Content-Encoding header fields,as shown in the 
                    following examples:
                    Accept-Encoding: gzip, deflate, peerdist");

                #endregion

                #region MS-PCCRTP_R23

                // Add the debug information
                this.Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRTP_R23");

                // Capture MS-PCCRTP_R23
                this.Site.CaptureRequirementIfIsNotNull(
                    peerDistParam,
                    23,
                    @"[In Higher-Layer Triggered Events] If the client chooses to use the 
                    PeerDist content encoding for an HTTP request, the client MUST also 
                    include the PeerDist parameters header field in the same HTTP request.");

                #endregion

                #region MS-PCCRTP_R20

                // Add the debug information
                this.Site.Log.Add(
                    LogEntryKind.Debug,
                    @"Verify MS-PCCRTP_R20 : The X-P2P-PeerDist value is [{0}].",
                    peerDistValue);

                // Verify MS-PCCRTP requirement: MS-PCCRTP_R20
                bool isVerifyR20 = peerDistValue.Contains("MissingDataRequest");

                // Capture MS-PCCRTP_R20
                this.Site.CaptureRequirementIfIsFalse(
                    isVerifyR20,
                    20,
                    @"[In Message Syntax] This parameter [missing-data-request parameter] MUST NOT 
                    be specified when the PeerDist content encoding is specified in the Accept-Encoding 
                    header field value.");

                #endregion
            }

            if (PCCRTPClientAdapter.IsMissingDataRequest)
            {
                #region MS-PCCRTP_R1901

                // Add the debug information
                this.Site.Log.Add(
                    LogEntryKind.Debug,
                    @"Verify MS-PCCRTP_R1901 : The X-P2P-PeerDist value is [{0}].",
                    peerDistValue);

                // Verify MS-PCCRTP requirement: MS-PCCRTP_R1901
                bool isVerifyR1901 = peerDistValue.Contains("MissingDataRequest");

                // Capture MS-PCCRTP_R1901
                this.Site.CaptureRequirementIfIsTrue(
                    isVerifyR1901,
                    1901,
                    @"[In Message Syntax]  The missing-data-request parameter [missing-data-request 
                    = ""MissingDataRequest"" ""="" ( ""true""  )] is used to indicate the client  was unable 
                    to retrieve data from its peers.");

                #endregion
            }

            #region MS-PCCRTP_R24

            // Capture MS-PCCRTP_R24
            // The MS-PCCRTP_R23 verify the request message include the PeerDist parameters header field,
            // the MS-PCCRTP_R13 verify the PeerDist parameters header format is [version=major.minor],
            // the MS-PCCRTP_R14 verify the major and minor versions are numbers,
            // so this requirement can be verified directly.
            this.Site.CaptureRequirement(
                24,
                @"[In Higher-Layer Triggered Events]  As shown in the following 
                example[X-P2P-PeerDist: Version=1.0],the PeerDist parameters header 
                field MUST contain the Version parameter containing the highest version 
                of the PeerDist content encoding that the client supports.");

            #endregion

            #region MS-PCCRTP_R1101

            // Capture MS-PCCRTP_R1101
            // The MS-PCCRTP_R23 verify the request message include the PeerDist parameters header field,
            // the MS-PCCRTP_R13 verify the PeerDist parameters header format,
            // the MS-PCCRTP_R14 verify the major and minor versions are numbers,
            // so this requirement can be verified directly.
            this.Site.CaptureRequirement(
                1101,
                @"[In Message Syntax] The extension-header field can appear in requests, 
                the syntax of this header field value is described as follows:
                extension-header = X-P2P-PeerDist 
                X-P2P-PeerDist = ""X-P2P-PeerDist"" "":"" peerdist-params 
                [peerdist-params = 1#( version | [content-len] | [missing-data-request] )
                version = ""Version"" ""="" major-version ""."" minor-version].");

            #endregion

            #region MS-PCCRTP_R47

            if (this.sutOsVersion == OSVersion.WinVista
                || this.sutOsVersion == OSVersion.Win7
                || this.sutOsVersion == OSVersion.Win2K8
                || this.sutOsVersion == OSVersion.Win2K8R2)
            {
                // Add the debug information
                this.Site.Log.Add(
                    LogEntryKind.Debug,
                    "Verify MS-PCCRTP_R47 : The PeerDist version is {0}.",
                    version[1]);

                // Verify MS-PCCRTP requirement: MS-PCCRTP_R47
                this.Site.CaptureRequirementIfAreEqual<string>(
                    @"1.0",
                    version[1],
                    47,
                    @"[In Appendix A: Product Behavior]<2> Section 1.7: HTTP/1.1 clients in 
                    Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2 
                    set the PeerDist version parameter to 1.0.");
            }

            #endregion
        }
    }
}
