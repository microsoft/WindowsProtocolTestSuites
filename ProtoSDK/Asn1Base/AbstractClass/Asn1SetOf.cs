// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Corresponds to the SET OF type in ASN.1 definition.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the SET.</typeparam>
    /// <remarks>
    /// All the user defined SET OF type should be derived from this class.
    /// </remarks>
    [Asn1Tag(Asn1TagType.Universal, Asn1TagValue.Set, EncodingWay = EncodingWay.Constructed)]
    public class Asn1SetOf<T> : Asn1HomogeneousComposition<T>
        where T : Asn1Object, new()
    {
        /// <summary>
        /// Initializes a new instance of the Asn1SetOf class with empty data.
        /// </summary>

        public Asn1SetOf()
        {
            this.Elements = null;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1SetOf class with given data in array form.
        /// </summary>
        /// <param name="elements"></param>
        public Asn1SetOf(T[] elements)
        {
            this.Elements = elements;
        }

    }
}