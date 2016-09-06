// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Dpsp
{
    /// <summary>
    /// Dpsp utility class
    /// </summary>
    public static class DpspUtility
    {
        /// <summary>
        /// separator 
        /// </summary>
        internal const char SEPARATOR = ',';

        /// <summary>
        /// equal string
        /// </summary>
        internal const char EQUAL_MARK = '=';

        /// <summary>
        /// space string
        /// </summary>
        internal const string SPACE = " ";

        /// <summary>
        /// quotation mark
        /// </summary>
        internal const char QUOTATION_MARK = '\"';

        /// <summary>
        /// DIGEST
        /// </summary>
        public const string DIGEST = "Digest";

        /// <summary>
        /// username directive
        /// </summary>
        public const string USER_NAME_DIRECTIVE = "username";

        /// <summary>
        /// realm directive
        /// </summary>        
        public const string REALM_DIRECTIVE = "realm";

        /// <summary>
        /// message qop directive
        /// </summary>
        public const string MESSAGE_QOP_DIRECTIVE = "qop";

        /// <summary>
        /// algorithm directive
        /// </summary>
        public const string ALGORITHM_DIRECTIVE = "algorithm";

        /// <summary>
        /// basic digest-uri directive
        /// </summary>
        public const string BASIC_DIGEST_URI_DIRECTIVE = "uri";

        /// <summary>
        /// sasl digest-uri directive
        /// </summary>
        public const string SASL_DIGEST_URI_DIRECTIVE = "digest-uri";

        /// <summary>
        /// nonce directive
        /// </summary>
        public const string NONCE_DIRECTIVE = "nonce";

        /// <summary>
        /// nonce count directive
        /// </summary>
        public const string NONCE_COUNT_DIRECTIVE = "nc";

        /// <summary>
        /// cnonce directive
        /// </summary>
        public const string CNONCE_DIRECTIVE = "cnonce";

        /// <summary>
        /// response directive
        /// </summary>
        public const string RESPONSE_DIRECTIVE = "response";

        /// <summary>
        /// authzid directive
        /// </summary>
        public const string AUTHZID_DIRECTIVE = "authzid";

        /// <summary>
        /// charset directive
        /// </summary>
        public const string CHARSET_DIRECTIVE = "charset";

        /// <summary>
        /// auth_param directive
        /// </summary>
        public const string AUTH_PARAM_DIRECTIVE = "auth-param";

        /// <summary>
        /// opaque directive        
        /// </summary>
        public const string OPAQUE_DIRECTIVE = "opaque";

        /// <summary>
        /// maxbuf directive
        /// </summary>
        public const string MAXBUF_DIRECTIVE = "maxbuf";

        /// <summary>
        /// cipher directive
        /// </summary>
        public const string CIPHER_DIRECTIVE = "cipher";

        /// <summary>
        /// hentity directive
        /// </summary>
        public const string HENTITY_DIRECTIVE = "channel-binding";   

        /// <summary>
        /// back slash mark
        /// </summary>
        internal const char BACK_SLASH_MARK = '\\';

        /// <summary>
        /// trim quotation mark regular expression pattern
        /// </summary>
        internal const string TRIM_QUOTATION_REGULAR_EXPRESSION_PATTERN = "(^\")|(\"$)";

        /// <summary>
        /// attribute value pair regular expression pattern
        /// </summary>
        internal const string ATTRIBUTE_VALUE_PAIR_REGULAR_EXPRESSION_PATTERN = @"
                                                                 (?<attribute>([\w\d-])+)
                                                                 =
                                                                 (?<value> 
                                                                 ""([^\\""]|(\\.))*""
                                                                 |
                                                                 [^,]*)
                                                                 (,|$)";

        /// <summary>
        /// remove escape pattern
        /// </summary>
        internal const string REMOVE_ESCAPE_PATTERN = @"(\\)(.)";

        /// <summary>
        /// escape pattern
        /// </summary>
        internal const string ESCAPE_PATTERN = @"(\\)";

        /// <summary>
        /// double escape pattern
        /// </summary>
        internal const string DOUBLE_ESCAPE_STRING = @"\\\\";

        /// <summary>
        /// replace escape pattern
        /// </summary>
        internal const string ESCAPE_REPLACEMENT_PATTERN = @"$2";

        /// <summary>
        /// Key
        /// </summary>
        internal const string Attribute = "attribute";

        /// <summary>
        /// Value
        /// </summary>
        internal const string VALUE = "value";

        /// <summary>
        /// trim quotation mark of value
        /// </summary>
        /// <param name="value">input string</param>
        /// <returns> the string after trim </returns>
        internal static string TrimQuotationMarks(string value)
        {
            if (value == null)
            {
                return null; 
            }
            
            string ret = Regex.Replace(
                value,
                TRIM_QUOTATION_REGULAR_EXPRESSION_PATTERN, 
                string.Empty);

            string replacePattern = REMOVE_ESCAPE_PATTERN;
            ret = Regex.Replace(
                ret, 
                replacePattern, 
                ESCAPE_REPLACEMENT_PATTERN);

            return ret;
        }


        /// <summary>
        /// add quotation mark to the string value
        /// </summary>
        /// <param name="value">input string</param>
        /// <returns>quoted string</returns>
        internal static string AddQuotationMarks(string value)
        {
            if (value == null)
            {
                return null;
            }

            string ret = value;
            Match m = Regex.Match(
                value, 
                TRIM_QUOTATION_REGULAR_EXPRESSION_PATTERN 
                );

            if (!m.Success && value.Contains(SEPARATOR.ToString()))
            {
                value = QUOTATION_MARK.ToString()
                        + value
                        + QUOTATION_MARK.ToString();

                ret = Regex.Replace(
                value,
                ESCAPE_PATTERN,
                DOUBLE_ESCAPE_STRING);
            }
            
            return ret;
        }
    }
}