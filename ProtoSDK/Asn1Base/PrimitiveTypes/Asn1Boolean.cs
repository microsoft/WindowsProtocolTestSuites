// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents a BOOLEAN in ASN.1 Definition
    /// </summary>
    [Asn1Tag(Asn1TagType.Universal, Asn1TagValue.Boolean)]
    public class Asn1Boolean : Asn1Object
    {
        /// <summary>
        /// Gets or sets the data of this object
        /// </summary>
        public bool Value
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1Boolean class that has the default data false.
        /// </summary>
        public Asn1Boolean()
        {
            Value = false;
        }

        /// <summary>
        /// Initializes a new instance of the Asn1Boolean class with a given value.
        /// </summary>
        /// <param name="val"></param>
        public Asn1Boolean(bool val)
        {
            Value = val;
        }

        //Constraint is not needed for primitive type BOOLEAN.
        //Therefore no need to override VerifyConstraints method.

        #region overrode methods from System.Object

        /// <summary>
        /// Overrode method from System.Object.
        /// </summary>
        /// <param name="obj">The object to be compared.</param>
        /// <returns>True if obj has same data with this instance. False if not.</returns>
        public override bool Equals(Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Asn1Boolean return false.
            Asn1Boolean p = obj as Asn1Boolean;
            if (p == null)
            {
                return false;
            }

            // Return true if the fields match.
            return Value == p.Value;
        }

        /// <summary>
        /// Returns the hash code of the instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        #endregion overrode methods from System.Object

        #region BER

        /// <summary>
        /// Encodes the data of this object to the buffer.
        /// </summary>
        /// <param name="buffer">A buffer to which the encoding result will be written.</param>
        /// <returns>The length of the encoding result of the data.</returns>
        /// <remarks>
        /// For the TLV (Tag, Length, Value) triple in the encoding result,
        /// this method provides the functionality of encoding Value.
        /// The encoding for Tag and Length will be done in Asn1Object::BerEncode method.
        /// </remarks>
        protected override int ValueBerEncode(IAsn1BerEncodingBuffer buffer)
        {
            //Encode data
            if (Value == false)
            {
                buffer.WriteByte(0);
            }
            else
            {
                buffer.WriteByte(255);
            }

            return 1;
        }

        /// <summary>
        /// Decodes the data from the buffer and stores the data in this object.
        /// </summary>
        /// <param name="buffer">A buffer that stores a BER encoding result.</param>
        /// <param name="length">The length of the encoding result of the data in the given buffer.</param>
        /// <returns>The number of the bytes consumed in the buffer to decode the data.</returns>
        /// <remarks>
        /// For the TLV (Tag, Length, Value) triple in the encoding result, 
        /// this method provides the functionality of decoding Value.
        /// The decoding for Tag and Length will be done in Asn1Object::BerDecode method.
        /// </remarks>
        protected override int ValueBerDecode(IAsn1DecodingBuffer buffer, int length)
        {
            if (length != 1)
            {
                throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData);
            }
            byte readContent = buffer.ReadByte();//Since length == 1
            if (readContent == 0)
            {
                Value = false;
            }
            else
            {
                Value = true;
            }
            return length;
        }

        #endregion BER

        #region PER

        /// <summary>
        /// Encodes the content of the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer to which the encoding result will be written.</param>
        protected override void ValuePerEncode(IAsn1PerEncodingBuffer buffer)
        {
            if (Value)
            {
                buffer.WriteBit(true);
            }
            else
            {
                buffer.WriteBit(false);
            }
        }

        /// <summary>
        /// Decodes the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a PER encoding result.</param>
        /// <param name="aligned">Indicating whether the PER decoding is aligned.</param>
        protected override void ValuePerDecode(IAsn1DecodingBuffer buffer, bool aligned = true)
        {
            bool result = buffer.ReadBit();
            if (result)
            {
                Value = true;
            }
            else
            {
                Value = false;
            }
        }

        #endregion PER
    }
}
