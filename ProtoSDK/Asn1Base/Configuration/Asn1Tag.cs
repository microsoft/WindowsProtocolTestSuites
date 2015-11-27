// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Contains the universal class tags of ASN.1 types.
    /// </summary>
    public class Asn1TagValue
    {
        //Ref. X.680: 8.6, Value of Universal Class Tags

        /// <summary>
        /// The value of Eoc in ASN.1.
        /// </summary>
        public const byte Eoc = 0;

        /// <summary>
        /// The value of Boolean in ASN.1.
        /// </summary>
        public const byte Boolean = 1;

        /// <summary>
        /// The value of Integer in ASN.1.
        /// </summary>
        public const byte Integer = 2;

        /// <summary>
        /// The value of BitString in ASN.1.
        /// </summary>
        public const byte BitString = 3;

        /// <summary>
        /// The value of OctetString in ASN.1.
        /// </summary>
        public const byte OctetString = 4;

        /// <summary>
        /// The value of Null in ASN.1.
        /// </summary>
        public const byte Null = 5;

        /// <summary>
        /// The value of ObjectIdentifier in ASN.1.
        /// </summary>
        public const byte ObjectIdentifier = 6;

        /// <summary>
        /// The value of ObjectDescriptor in ASN.1.
        /// </summary>
        public const byte ObjectDescriptor = 7;

        /// <summary>
        /// The value of External in ASN.1.
        /// </summary>
        public const byte External = 8;

        /// <summary>
        /// The value of Real in ASN.1.
        /// </summary>
        public const byte Real = 9;

        /// <summary>
        /// The value of Enumerated in ASN.1.
        /// </summary>
        public const byte Enumerated = 10;

        /// <summary>
        /// The value of Utf8String in ASN.1.
        /// </summary>
        public const byte Utf8String = 12;

        /// <summary>
        /// The value of RelativeOid in ASN.1.
        /// </summary>
        public const byte RelativeOid = 13;

        /// <summary>
        /// The value of Sequence in ASN.1.
        /// </summary>
        public const byte Sequence = 16;

        /// <summary>
        /// The value of Set in ASN.1.
        /// </summary>
        public const byte Set = 17;

        /// <summary>
        /// The value of NumericString in ASN.1.
        /// </summary>
        public const byte NumericString = 18;

        /// <summary>
        /// The value of PrintableString in ASN.1.
        /// </summary>
        public const byte PrintableString = 19;

        /// <summary>
        /// The value of T61String in ASN.1.
        /// </summary>
        public const byte T61String = 20;

        /// <summary>
        /// The value of TeletexString in ASN.1.
        /// </summary>
        public const byte TeletexString = 20;

        /// <summary>
        /// The value of VideotexString in ASN.1.
        /// </summary>
        public const byte VideotexString = 21;

        /// <summary>
        /// The value of Ia5String in ASN.1.
        /// </summary>
        public const byte Ia5String = 22;

        /// <summary>
        /// The value of UtcTime in ASN.1.
        /// </summary>
        public const byte UtcTime = 23;

        /// <summary>
        /// The value of GeneralizedTime in ASN.1.
        /// </summary>
        public const byte GeneralizedTime = 24;

        /// <summary>
        /// The value of GraphicString in ASN.1.
        /// </summary>
        public const byte GraphicString = 25;

        /// <summary>
        /// The value of VisibleString in ASN.1.
        /// </summary>
        public const byte VisibleString = 26;

        /// <summary>
        /// The value of GeneralString in ASN.1.
        /// </summary>
        public const byte GeneralString = 27;

        /// <summary>
        /// The value of UniversalString in ASN.1.
        /// </summary>
        public const byte UniversalString = 28;

        /// <summary>
        /// The value of BmpString in ASN.1.
        /// </summary>
        public const byte BmpString = 30;

        /// <summary>
        /// The value of OpenType in ASN.1.
        /// </summary>
        public const byte OpenType = 99;
    }

    /// <summary>
    /// Specifies the types of ASN.1 tags.
    /// </summary>
    public enum Asn1TagType
    {
        /// <summary>
        /// Indicates the tag is a Universal Class Tag.
        /// </summary>
        Universal,

        /// <summary>
        /// Indicates the tag is a Application Class Tag.
        /// </summary>
        Application,

        /// <summary>
        /// Indicates the tag is a Context Class Tag.
        /// </summary>
        Context,

        /// <summary>
        /// Indicates the tag is a Private Class Tag.
        /// </summary>
        Private
    }

    /// <summary>
    /// Specifies the encoding way of a ASN.1 tag.
    /// </summary>
    public enum EncodingWay
    {
        /// <summary>
        /// Indicates the tag should encoded in primitive way.
        /// </summary>
        Primitive,

        /// <summary>
        /// Indicates the tag should encoded in constructed way.
        /// </summary>
        Constructed
    }
}
