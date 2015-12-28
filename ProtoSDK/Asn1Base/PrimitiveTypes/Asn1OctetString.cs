// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents an OCTET STRING in ASN.1 Definition.
    /// </summary>
    [Asn1Tag(Asn1TagType.Universal, Asn1TagValue.OctetString)]
    public class Asn1OctetString : Asn1ByteString
    {
        /// <summary>
        /// Initializes a new instance of the Asn1OctetString class with an empty string.
        /// </summary>
        public Asn1OctetString()
        {
            this.Value = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1OctetString class with a given string.
        /// </summary>
        /// <param name="s"></param>
        public Asn1OctetString(string s)
        {
            this.Value = s;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1OctetString class with a given byte array.
        /// </summary>
        /// <param name="bytes">The byte array to initialize the string.</param>
        /// <remarks>A byte is equivalent to a char in OCTET STRING.</remarks>
        public Asn1OctetString(byte[] bytes)
        {
            this.ByteArrayValue = bytes; 
        }

        //TODO: add constraints by overriding VerifyConstraints method, check ASN.1 Doc for detailed constraints on CHAR SET.

        //BER encoding/decoding are implemented in base class Asn1ByteString.
        
        //PER pending
    }
}
