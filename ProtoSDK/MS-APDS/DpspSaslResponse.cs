// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Dpsp
{
    /// <summary>
    /// Dpsp sasl response class - rfc2183
    /// </summary>
    public class DpspSaslResponse : DpspResponse
    {
        /// <summary>
        /// DigestUri:
        /// The URI from Request-URI of the Request-Line; duplicated here 
        /// because proxies are allowed to change the Request-Line in transit. 
        /// digest-uri       = "digest-uri" "=" digest-uri-value
        /// digest-uri-value =  serv-type "/" host [ "/" serv-name ]
        /// serv-type        = 1*ALPHA
        /// host             = 1*( ALPHA | DIGIT | "-" | "." )
        /// serv-name        = host
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string DigestUri
        {
            get
            {
                return DpspUtility.TrimQuotationMarks(
                    GetAttributeValue(DpspUtility.SASL_DIGEST_URI_DIRECTIVE));
            }

            set
            {
                SetAttributeValue(DpspUtility.SASL_DIGEST_URI_DIRECTIVE, value);
            }
        }

        /// <summary>
        /// Maxbuf:
        /// A number indicating the size of the largest buffer the server is able to receive 
        /// when using "auth-int" or "auth-conf". 
        /// </summary>
        public UInt32 MaxBuf
        {
            get
            {
                return Convert.ToUInt32(GetAttributeValue(DpspUtility.MAXBUF_DIRECTIVE));
            }

            set
            {
                SetAttributeValue(DpspUtility.MAXBUF_DIRECTIVE, Convert.ToString(value));
            }
        }

        /// <summary>
        /// Charset:
        /// This directive, if present, 
        /// specifies that the server supports UTF-8 encoding for the username and password. 
        /// If not present, the username and password must be encoded in ISO 8859-1 
        /// (of which US-ASCII is a subset). 
        /// The directive is needed for backwards compatibility with HTTP Digest, 
        /// which only supports ISO 8859-1.
        /// </summary>
        public string CharSet
        {
            get
            {
                return DpspUtility.TrimQuotationMarks(GetAttributeValue(DpspUtility.CHARSET_DIRECTIVE));
            }

            set
            {
                SetAttributeValue(DpspUtility.CHARSET_DIRECTIVE, value);
            }
        }

        /// <summary>
        /// Cipher:
        /// A list of ciphers that the server supports. 
        /// This directive must be present exactly once 
        /// if "auth-conf" is offered in the "qop-options" directive
        /// </summary>
        public string Cipher
        {
            get
            {
                return DpspUtility.TrimQuotationMarks((GetAttributeValue(DpspUtility.CIPHER_DIRECTIVE)));
            }

            set
            {
                SetAttributeValue(DpspUtility.CIPHER_DIRECTIVE, value);
            }
        }

        /// <summary>
        /// AuthzId:
        /// The "authorization ID" as per RFC 2222, encoded in UTF-8. 
        /// This directive is optional. If present, and the authenticating user has sufficient privilege, 
        /// and the server supports it, then after authentication 
        /// the server will use this identity for making all accesses and access checks. 
        /// If the client specifies it, and the server does not support it, 
        /// then the response-value will be incorrect, and authentication will fail. 
        /// </summary>
        public string AuthzId
        {
            get
            {
                return DpspUtility.TrimQuotationMarks(GetAttributeValue(DpspUtility.AUTHZID_DIRECTIVE));
            }

            set
            {
                SetAttributeValue(DpspUtility.AUTHZID_DIRECTIVE, value);
            }
        }

        /// <summary>
        /// Empty ctor
        /// </summary>
        public DpspSaslResponse()
            : base()
        { }

        /// <summary>
        /// Decode digestCredentials to get a DpspSaslResponse instance
        /// </summary>
        /// <param name="digestCredentials">Dpsp digest credentials</param>
        /// <returns>DpspSaslResponse instance</returns>
        public static DpspSaslResponse Decode(string digestCredentials)
        {
            DpspSaslResponse dpspSaslResponse = new DpspSaslResponse();
            DpspResponse.Decode(digestCredentials, dpspSaslResponse);
            return dpspSaslResponse;
        }
    }
}