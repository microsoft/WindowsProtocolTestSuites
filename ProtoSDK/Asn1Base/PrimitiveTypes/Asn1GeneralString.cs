// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents a GeneralString in ASN.1 Definition.
    /// </summary>
    [Asn1Tag(Asn1TagType.Universal, Asn1TagValue.GeneralString)]
    public class Asn1GeneralString : Asn1ByteString
    {
        /// <summary>
        /// Initializes a new instance of the Asn1GeneralString class with an empty string.
        /// </summary>
        public Asn1GeneralString()
        {
            this.Value = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1GeneralString class with a given string.
        /// </summary>
        /// <param name="s"></param>
        public Asn1GeneralString(string s)
        {
            this.Value = s;
        }


        //BER encoding/decoding are implemented in base class Asn1ByteString.

        //PER pending
    }
}
