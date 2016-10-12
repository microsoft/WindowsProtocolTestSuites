// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrtp
{
    using System.Linq;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;

    /// <summary>
    /// Verify server adapter requirements about MS-PCCRTP.
    /// </summary>
    public partial class PCCRTPServerAdapter
    {
        /// <summary>
        /// This class is used to verify the requirements related to PCCRTP response.
        /// </summary>
        /// <param name="pccrtpResponse">The PCCRTP response message.</param>
        private void VerifyPccrtpResponse(PccrtpResponse pccrtpResponse)
        {
            #region MS-PCCRTP_R11

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRTP_R11. Actual value of X-P2P-PeerDist field is: {0}.",
                pccrtpResponse.HttpHeader[XP2PPEERDIST]);

            bool isVerifyR11 = pccrtpResponse.HttpResponse.Headers[XP2PPEERDIST].Contains("Version")
                && pccrtpResponse.HttpResponse.Headers[XP2PPEERDIST].Contains("ContentLength");

            // Verify MS-PCCRTP requirement: MS-PCCRTP_R11
            Site.CaptureRequirementIfIsTrue(
                isVerifyR11,
                11,
                @"[In Message Syntax] The extension-header field can appear in  responses, the syntax of this header
                field value is described as follows:
                extension-header = X-P2P-PeerDist 
                X-P2P-PeerDist = ""X-P2P-PeerDist"" "":"" peerdist-params [peerdist-params = 1#( version | [content-len]
                | [missing-data-request] )
                version = ""Version"" ""="" major-version ""."" minor-version].
                ");

            #endregion

            #region MS-PCCRTP_R18

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                "Verify MS-PCCRTP_R18. Actual Value of Content-Length is {0}",
                pccrtpResponse.HttpHeader[CONTENTLENGTH]);

            long contentLength;

            // Verify MS-PCCRTP requirement: MS-PCCRTP_R18
            bool isVerifyR18 = pccrtpResponse.HttpHeader.Values.Contains(pccrtpResponse.HttpHeader[CONTENTLENGTH])
                && long.TryParse(pccrtpResponse.HttpHeader[CONTENTLENGTH], out contentLength);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR18,
                18,
                @"[In Message Syntax] The content-len [content-len = ""ContentLength"" ""="" 1*DIGIT] parameter contains
                the length of the entity-body before the PeerDist content encoding is applied to it.");
            
            #endregion

            #region MS-PCCRTP_R34

            bool isVerifyR34 = pccrtpResponse.HttpHeader.Values.Contains(pccrtpResponse.HttpHeader["X-P2P-PeerDist"]);

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                "Verify MS-PCCRTP_R34. The response {0} contain PeerDist parameters header field.",
                isVerifyR34 ? string.Empty : "does not ");

            // Verify MS-PCCRTP requirement: MS-PCCRTP_R34
            Site.CaptureRequirementIfIsTrue(
                isVerifyR34,
                34,
                @"[In Receiving a PeerDist-Supporting Request] HTTP/1.1 server MUST also include the PeerDist parameters
                header field in the response.");
            
            #endregion

            #region MS-PCCRTP_R35

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                "Verify MS-PCCRTP_R35. Actual Value of Version is {0}",
                pccrtpResponse.HttpHeader[XP2PPEERDIST]);

            // Verify MS-PCCRTP requirement: MS-PCCRTP_R35
            bool isVerifyR35 = pccrtpResponse.HttpHeader[XP2PPEERDIST].Contains("Version");

            Site.CaptureRequirementIfIsTrue(
                isVerifyR35,
                35,
                @"[In Receiving a PeerDist-Supporting Request] The PeerDist parameters header field MUST contain 
                the Version parameter containing the version of the PeerDist content encoding used in the response.");

            #endregion

            #region MS-PCCRTP_R36

            bool isVerifyR36 = pccrtpResponse.HttpHeader[XP2PPEERDIST].Contains("ContentLength");

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                "Verify MS-PCCRTP_R36. Actual Value of ContentLength is {0} in PeerDist Header field",
                isVerifyR36 ? string.Empty : "not");

            // Verify MS-PCCRTP requirement: MS-PCCRTP_R36
            Site.CaptureRequirementIfIsTrue(
                isVerifyR36,
                36,
                @"[In Receiving a PeerDist-Supporting Request] the PeerDist parameters header field MUST also contain 
                the ContentLength parameter specifying the content length of the response entity-body 
                before the PeerDist content encoding has been applied to it.");

            #endregion

            #region MS-PCCRTP_R48

            string[] headerValues = pccrtpResponse.HttpHeader[XP2PPEERDIST].Split(new char[] { ',' });
            string versionString = headerValues[0].Substring(headerValues[0].IndexOf('=') + 1);

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                "Verify MS-PCCRTP_R48. Actual Value of peerdist version is {0}",
                versionString);

            if (PCCRTPServerAdapter.SutOsVersion == OSVersion.Win2K8R2)
            {
                // Verify MS-PCCRTP requirement: MS-PCCRTP_R48
                Site.CaptureRequirementIfAreEqual<string>(
                    "1.0",
                    versionString,
                    48,
                    @"[In Appendix A: Product Behavior]<3> Section 1.7: HTTP/1.1 servers in Windows Server 2008 R2 
                set the PeerDist version parameter to 1.0.");
            }

            #endregion

            #region MS-PCCRTP_R52

            if (PCCRTPServerAdapter.SutOsVersion == OSVersion.Win2K8R2)
            {
                bool isContainEtag = pccrtpResponse.HttpResponse.Headers.AllKeys.Contains("ETag");
                bool isContainLastModified = pccrtpResponse.HttpResponse.Headers.AllKeys.Contains("Last-Modified");
                bool isContainBoth = pccrtpResponse.HttpResponse.Headers.AllKeys.Contains("ETag")
                    && pccrtpResponse.HttpResponse.Headers.AllKeys.Contains("Last-Modified");

                // Add the debug information
                Site.Log.Add(
                    LogEntryKind.Debug,
                    @"Verify MS-PCCRTP_R52.The response actually does {0} contain an ETag header field, 
                    does {1} contain a Last-Modified header field, does {2} contain both header fields.",
                    isContainEtag ? string.Empty : "not",
                    isContainLastModified ? string.Empty : "not",
                    isContainBoth ? string.Empty : "not");

                bool isVerifyR52 = isContainEtag || isContainLastModified || isContainBoth;

                // Verify MS-PCCRTP requirement: MS-PCCRTP_R52
                Site.CaptureRequirementIfIsTrue(
                    isVerifyR52,
                    52,
                    @"[In Appendix A: Product Behavior]  <7> Section 3.2:The HTTP/1.1 server in Windows Server 2008 R2 
                    sends a PeerDist-encoded response only when the response contains an ETag header field or a 
                    Last-Modified header field or both header fields.");
            }

            #endregion

            #region MS-PCCRTP_R54

            if (PCCRTPServerAdapter.SutOsVersion == OSVersion.Win2K8R2)
            {
                bool isVerifyR54 = pccrtpResponse.HttpResponse.ContentEncoding.Equals("peerdist")
                    && !pccrtpResponse.ContentInfo.Equals(null);

                // Add the debug information
                Site.Log.Add(
                    LogEntryKind.Debug,
                    @"Verify MS-PCCRTP_R54. The HTTP/1.1 server in Windows Server 2008 R2 does {0} 
                    use the algorithms and data structures defined in [MS-PCCRC].",
                    isVerifyR54 ? string.Empty : "not");

                // Verify MS-PCCRTP requirement: MS-PCCRTP_R54
                Site.CaptureRequirementIfIsTrue(
                    isVerifyR54,
                    54,
                    @"[In Appendix A: Product Behavior]<9> Section 3.2.5.1:   The HTTP/1.1 server 
                    in Windows Server 2008 R2 uses the algorithms and data structures defined in [MS-PCCRC] 
                    to generate the PeerDist Content Information only when it receives an HTTP/1.1 request. ");
            }
                
            #endregion

            #region MS-PCCRRP_R8001

            bool isVerifyR8001 = pccrtpResponse.HttpHeader.ContainsKey(CONTENTENCODING) 
                && pccrtpResponse.HttpHeader[CONTENTENCODING].Contains("peerdist");

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRTP_R8001, the PeerDist content-encoding value is {0} specified 
                in the Accept-Encoding and Content-Encoding header fields.",
                isVerifyR8001 ? string.Empty : "not ");

            // Verify MS-PCCRTP requirement: MS-PCCRTP_R8001
            Site.CaptureRequirementIfIsTrue(
                isVerifyR8001,
                8001,
                @"[In Message Syntax] The PeerDist content-encoding value can be specified in the Accept-Encoding 
                and Content-Encoding header fields,as shown in the following examples: Content-Encoding: peerdist.");

            #endregion
        }
    }
}
