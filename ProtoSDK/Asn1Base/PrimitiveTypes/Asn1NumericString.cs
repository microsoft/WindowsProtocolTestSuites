// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text.RegularExpressions;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents a NumericString in ASN.1 Definition.
    /// </summary>
    [Asn1Tag(Asn1TagType.Universal, Asn1TagValue.NumericString)]
    public class Asn1NumericString : Asn1ByteString
    {
        /// <summary>
        /// Initializes a new instance of the Asn1NumericString class with an empty string.
        /// </summary>
        public Asn1NumericString()
        {
            this.Value = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1NumericString class with a given string.
        /// </summary>
        /// <param name="s"></param>
        public Asn1NumericString(string s)
        {
            this.Value = s;
        }

        /// <summary>
        /// Checks the constraints for NumericString.
        /// </summary>
        /// <returns>True if constraints are satisfied; False if not satisfied.</returns>
        /// <remarks>
        /// Only 0~9 and SPACE are allowed in NumericString.
        /// Ref. X.680: 41.2.
        /// </remarks>
        protected override bool VerifyConstraints()
        {
            Regex r = new Regex(@"^[\d ]*$");
            return r.IsMatch(this.Value); 
        }

        //BER encoding/decoding are implemented in base class Asn1ByteString.

        #region PER

        #endregion PER
    }
}

