// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Dpsp
{
    /// <summary>
    /// Dpsp sasl response class - rfc 2617
    /// </summary>
    public class DpspBasicResponse : DpspResponse
    {
        /// <summary>
        /// DigestUri:
        /// The URI from Request-URI of the Request-Line; duplicated here 
        /// because proxies are allowed to change the Request-Line in transit. 
        /// digest-uri       = "uri" "=" digest-uri-value
        /// digest-uri-value = request-uri   ; As specified by HTTP/1.1
        /// Request-URI    = "*" | absoluteURI | abs_path | authority
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string DigestUri
        {
            get
            {
                return DpspUtility.TrimQuotationMarks(
                    GetAttributeValue(DpspUtility.BASIC_DIGEST_URI_DIRECTIVE));
            }

            set
            {
                SetAttributeValue(DpspUtility.BASIC_DIGEST_URI_DIRECTIVE, 
                    DpspUtility.AddQuotationMarks(value));
            }
        }

        /// <summary>
        /// Opaque:
        /// A string of data, specified by the server, which should be returned 
        /// by the client unchanged in the Authorization header of subsequent requests with URIs 
        /// in the same protection space. 
        /// It is recommended that this string be base64 or hexadecimal data. 
        /// </summary>
        public string Opaque
        {
            get
            {
                return DpspUtility.TrimQuotationMarks(GetAttributeValue(DpspUtility.OPAQUE_DIRECTIVE));
            }

            set
            {
                SetAttributeValue(DpspUtility.OPAQUE_DIRECTIVE, 
                    DpspUtility.AddQuotationMarks(value));
            }
        }

        /// <summary>
        /// Algorithm:
        /// A string indicating a pair of algorithms used to produce the digest and a checksum. 
        /// If this is not present it is assumed to be "MD5". 
        /// If the algorithm is not understood, 
        /// the challenge should be ignored (and a different one used, if there is more than one). 
        /// </summary>
        public string Algorithm
        {
            get
            {
                return DpspUtility.TrimQuotationMarks(GetAttributeValue(DpspUtility.ALGORITHM_DIRECTIVE)); 
            }

            set
            {
                SetAttributeValue(DpspUtility.ALGORITHM_DIRECTIVE, 
                    DpspUtility.AddQuotationMarks(value));
            }
        }

        /// <summary>
        /// empty ctor
        /// </summary>
        public DpspBasicResponse():
            base()
        {
            
        }

        /// <summary>
        /// Decode digestCredentials to get a DpspBasicResponse instance
        /// </summary>
        /// <param name="digestCredentials">Dpsp digest credentials</param>
        /// <returns>DpspBasicResponse instance</returns>
        public static DpspBasicResponse Decode(string digestCredentials)
        {
            DpspBasicResponse dpspBasicResponse = new DpspBasicResponse();
            DpspResponse.Decode(digestCredentials, dpspBasicResponse);
            return dpspBasicResponse;
        }
    }
}