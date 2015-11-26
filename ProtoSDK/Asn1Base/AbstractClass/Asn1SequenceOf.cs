// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Corresponds to the SEQUENCE OF type in ASN.1 definition.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the SEQUENCE.</typeparam>
    /// <remarks>
    /// All the user defined SEQUENCE OF type should be derived from this class.
    /// </remarks>
    [Asn1Tag(Asn1TagType.Universal, Asn1TagValue.Sequence, EncodingWay = EncodingWay.Constructed)]
    public class Asn1SequenceOf<T> : Asn1HomogeneousComposition<T>
        where T : Asn1Object, new()
    {
        /// <summary>
        /// Initializes a new instance of the Asn1SequenceOf class with empty data.
        /// </summary>
        public Asn1SequenceOf()
        {
            this.Elements = null;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1SequenceOf class with given data in array form.
        /// </summary>
        /// <param name="elements"></param>
        public Asn1SequenceOf(T[] elements)
        {
            this.Elements = elements;
        }

    }
}