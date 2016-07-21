// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Dpsp
{
    /// <summary>
    /// base class of Dpsp response
    /// </summary>
    public abstract class DpspResponse
    {
        /// <summary>
        /// attribute value map
        /// </summary>
        protected StringDictionary attributeValueMap;

        /// <summary>
        /// UserName:
        /// The user's name in the specified realm
        /// </summary>
        public string UserName
        {
            get
            {
                return DpspUtility.TrimQuotationMarks(GetAttributeValue(DpspUtility.USER_NAME_DIRECTIVE));
            }

            set
            {
                SetAttributeValue(DpspUtility.USER_NAME_DIRECTIVE,
                    DpspUtility.AddQuotationMarks(value));
            }
        }

        /// <summary>
        /// Realm:
        /// A string to be displayed to users so they know which username and password to use.
        /// </summary>
        public string Realm
        {
            get
            {
                return DpspUtility.TrimQuotationMarks(GetAttributeValue(DpspUtility.REALM_DIRECTIVE));
            }

            set
            {
                SetAttributeValue(DpspUtility.REALM_DIRECTIVE,
                    DpspUtility.AddQuotationMarks(value));
            }
        }

        /// <summary>
        /// Nonce:
        /// A server-specified data string 
        /// which should be uniquely generated each time a 401 response is made.
        /// It is recommended that this string be base64 or hexadecimal data.
        /// </summary>
        public string Nonce
        {
            get
            {
                return DpspUtility.TrimQuotationMarks(GetAttributeValue(DpspUtility.NONCE_DIRECTIVE));
            }

            set
            {
                SetAttributeValue(DpspUtility.NONCE_DIRECTIVE,
                    DpspUtility.AddQuotationMarks(value));
            }
        }

        /// <summary>
        /// NonceCount:
        /// The nc-value is the hexadecimal count of the number of requests (including the current request) 
        /// that the client has sent with the nonce value in this request. 
        /// </summary>
        public UInt32 NonceCount
        {
            get
            {
                return Convert.ToUInt32(GetAttributeValue(DpspUtility.NONCE_COUNT_DIRECTIVE));
            }

            set
            {
                SetAttributeValue(DpspUtility.NONCE_COUNT_DIRECTIVE, Convert.ToString(value));
            }
        }

        /// <summary>
        /// Cnonce:
        /// The cnonce-value is an opaque quoted string value provided by the client 
        /// and used by both client and server to avoid chosen plaintext attacks, 
        /// to provide mutual authentication, and to provide some message integrity protection.
        /// It is recommended that this string be base64 or hexadecimal data.
        /// </summary>
        public string Cnonce
        {
            get
            {
                return DpspUtility.TrimQuotationMarks(GetAttributeValue(DpspUtility.CNONCE_DIRECTIVE));
            }

            set
            {
                SetAttributeValue(DpspUtility.CNONCE_DIRECTIVE,
                    DpspUtility.AddQuotationMarks(value));
            }
        }

        /// <summary>
        /// Response:
        /// A string of 32 lower case hex digits computed as defined in TD. 
        /// It's a 128bit MD5 hash value, which proves that the user knows a password 
        /// </summary>
        public byte[] Response
        {
            get
            {
                string hexString = DpspUtility.TrimQuotationMarks(
                    GetAttributeValue(DpspUtility.RESPONSE_DIRECTIVE));

                byte[] responseValue = new byte[hexString.Length / 2];
                for (int i = 0; i < hexString.Length / 2; i++)
                {
                    responseValue[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }

                return responseValue;
            }

            set
            {
                if (value != null)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < value.Length; i++)
                    {
                        // convert a byte to lower-case hex string
                        string hexValue = BitConverter.ToString(new byte[] { value[i] }).ToLower();
                        sb.Append(hexValue);
                    }

                    string ret = sb.ToString();
                    SetAttributeValue(DpspUtility.RESPONSE_DIRECTIVE,
                        DpspUtility.AddQuotationMarks(ret));
                }
            }
        }

        /// <summary>
        /// MessageQop:
        /// Indicates what "quality of protection" the client has applied to the message. 
        /// </summary>
        public string MessageQop
        {
            get
            {
                return DpspUtility.TrimQuotationMarks(GetAttributeValue(DpspUtility.MESSAGE_QOP_DIRECTIVE));
            }

            set
            {
                SetAttributeValue(DpspUtility.MESSAGE_QOP_DIRECTIVE,
                    DpspUtility.AddQuotationMarks(value));
            }
        }

        /// <summary>
        /// AuthParam:
        /// Windows uses the capability specified in [RFC2617] 
        /// to add a new directive via the auth-param in the digest-challenge message 
        /// ([RFC2831] section 2.1.1). 
        /// The server sends: charset=utf-8 in the auth-param field. 
        /// This indicates that the server can process UTF-8 encoded strings and 
        /// that the client might use Unicode encoding for the username field 
        /// and in the password if it can also process UTF-8. 
        /// Windows clients will use [UNICODE] encoding when it is offered by the server 
        /// to allow authentication with a region's supported character sets.
        /// </summary>
        public string AuthParam
        {
            get
            {
                return DpspUtility.TrimQuotationMarks(GetAttributeValue(DpspUtility.AUTH_PARAM_DIRECTIVE));
            }

            set
            {
                SetAttributeValue(DpspUtility.AUTH_PARAM_DIRECTIVE,
                    DpspUtility.AddQuotationMarks(value));
            }
        }

        /// <summary>
        /// empty ctor
        /// </summary>
        protected DpspResponse()
        {
            attributeValueMap = new StringDictionary();
        }

        /// <summary>
        /// Get key value
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns>key value</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when key is null 
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when key is empty
        /// </exception>
        public string GetAttributeValue(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return attributeValueMap[key];
        }


        /// <summary>
        /// Set key value
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="value">value</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when key or value is null 
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when key is empty
        /// </exception>
        public void SetAttributeValue(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (key.Length == 0)
            {
                throw new ArgumentException("key should not be empty", "key");
            }

            // Accept null value
            attributeValueMap[key] = value;
        }


        /// <summary>
        /// Decode
        /// </summary>
        /// <param name="digestCredentials">http digest credentials string</param>
        /// <param name="dpspResponse">dpspResponse instance</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when digestCredentials or digestCredentials is null 
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when digestCredentials or dpspResponse is invalid 
        /// </exception>
        protected static void Decode(
            string digestCredentials,
            DpspResponse dpspResponse)
        {
            if (digestCredentials == null)
            {
                throw new ArgumentNullException("digestCredentials");
            }

            if (digestCredentials.Length == 0)
            {
                throw new ArgumentException(
                    "digestCredentials should not be empty",
                    "digestCredentials");
            }

            if (dpspResponse == null)
            {
                throw new ArgumentNullException("dpspResponse");
            }

            digestCredentials = digestCredentials.TrimStart();

            if (!digestCredentials.StartsWith(DpspUtility.DIGEST + DpspUtility.SPACE))
            {
                throw new ArgumentException(
                    "digestResponseCredentials should start with " + DpspUtility.DIGEST + "(SPACE)",
                    "digestCredentials");
            }

            // ABNF credentials = "Digest" digest-response
            string digestResponse = digestCredentials.Substring(DpspUtility.DIGEST.Length + DpspUtility.SPACE.Length);

            Regex regex = new Regex(
                DpspUtility.ATTRIBUTE_VALUE_PAIR_REGULAR_EXPRESSION_PATTERN,
                RegexOptions.IgnorePatternWhitespace);

            MatchCollection matchCollection = regex.Matches(digestResponse);

            if (matchCollection.Count == 0)
            {
                throw new ArgumentException("invalid digestResponse", "digestCredentials");
            }

            foreach (Match match in matchCollection)
            {
                string key;
                string value;

                if (match.Groups[DpspUtility.Attribute].Success)
                {
                    key = match.Groups[DpspUtility.Attribute].Value;
                }
                else
                {
                    throw new ArgumentException("invalid digestResponse", "digestCredentials");
                }

                if (match.Groups[DpspUtility.VALUE].Success)
                {
                    value = match.Groups[DpspUtility.VALUE].Value;
                }
                else
                {
                    throw new ArgumentException("invalid digestResponse", "digestCredentials");
                }

                dpspResponse.SetAttributeValue(key, value);
            }
        }
    }
}