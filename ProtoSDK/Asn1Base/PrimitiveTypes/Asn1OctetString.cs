// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
            : this(string.Empty)
        {

        }

        /// <summary>
        /// Initializes a new instance of the Asn1OctetString class with a given string.
        /// </summary>
        /// <param name="s"></param>
        public Asn1OctetString(string s)
        {
            Value = s;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1OctetString class with a given byte array.
        /// </summary>
        /// <param name="bytes">The byte array to initialize the string.</param>
        /// <remarks>A byte is equivalent to a char in OCTET STRING.</remarks>
        public Asn1OctetString(byte[] bytes)
        {
            ByteArrayValue = bytes;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1OctetString class with a given string and size constraints.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="maxSize"></param>
        /// <param name="minSize"></param>
        protected internal Asn1OctetString(string s, long? maxSize, long? minSize)
        {
            Value = s;
            Constraint = new Asn1StringConstraint();
            if (maxSize != null)
            {
                Constraint.MaxSize = (long)maxSize;
            }
            if (minSize != null)
            {
                Constraint.MinSize = (long)minSize;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Asn1OctetString class with a given byte array and size constraints.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="maxSize"></param>
        /// <param name="minSize"></param>
        protected internal Asn1OctetString(byte[] bytes, long? maxSize, long? minSize)
        {
            ByteArrayValue = bytes;
            Constraint = new Asn1StringConstraint();
            if (maxSize != null)
            {
                Constraint.MaxSize = (long)maxSize;
            }
            if (minSize != null)
            {
                Constraint.MinSize = (long)minSize;
            }
        }

        //BER encoding/decoding are implemented in base class Asn1ByteString.

        /// <summary>
        /// Encodes the content of the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer to which the encoding result will be written.</param>
        /// <remarks>Length is included.</remarks>
        protected override void ValuePerEncode(IAsn1PerEncodingBuffer buffer)
        {
            if (Constraint != null && Constraint.HasMinSize && Constraint.HasMinSize
                && Constraint.MinSize == Constraint.MaxSize && Constraint.MinSize <= 2) //Ref. X.691:16.6
            {
                buffer.WriteBits(ByteArrayValue, 0, ByteArrayValue.Length * 8);
            }
            else
            {
                Asn1StandardProcedure.PerEncodeArray(buffer, ByteArrayValue,
                (encodingBuffer, b) => { encodingBuffer.WriteByte(b); }
                , Constraint != null && Constraint.HasMinSize ? Constraint.MinSize : (long?)null, Constraint != null && Constraint.HasMaxSize ? Constraint.MaxSize : (long?)null);
            }
        }

        /// <summary>
        /// Decodes the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a PER encoding result.</param>
        /// <param name="aligned">Indicating whether the PER decoding is aligned.</param>
        /// <remarks>Length is included.</remarks>
        protected override void ValuePerDecode(IAsn1DecodingBuffer buffer, bool aligned = true)
        {
            if (Constraint != null && Constraint.HasMinSize && Constraint.HasMinSize
                && Constraint.MinSize == Constraint.MaxSize && Constraint.MinSize <= 2) //Ref. X.691:16.6
            {
                long minSize = Constraint.MinSize;
                bool[] bitResult = buffer.ReadBits((int)(8 * minSize));
                byte[] byteResult = new byte[(int)minSize];
                //Convert bool array to byte array
                int byteIndex = 0, bitIndex = 0;
                foreach (bool b in bitResult)
                {
                    if (b)
                    {
                        byteResult[byteIndex] |= (byte)(1 << (7 - bitIndex));
                    }
                    bitIndex++;
                    if (bitIndex == 8)
                    {
                        bitIndex = 0;
                        byteIndex++;
                    }
                }
                ByteArrayValue = byteResult;
            }
            else
            {
                ByteArrayValue = Asn1StandardProcedure.PerDecodeArray(buffer,
                decodingBuffer => decodingBuffer.ReadByte(),
                Constraint != null && Constraint.HasMinSize ? Constraint.MinSize : (long?)null, Constraint != null && Constraint.HasMaxSize ? Constraint.MaxSize : (long?)null);
            }
        }
    }
}
