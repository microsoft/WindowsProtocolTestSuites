// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Contains some common procedures and properties for SET OF and SEQUENCE OF.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the composition.</typeparam>
    /// <remarks>
    /// All the fields in an Asn1HomogeneousComposition must have the same type.
    /// SET OF and SEQUENCE OF should be derived from this class.
    /// </remarks>
    public abstract class Asn1HomogeneousComposition<T> : Asn1Object
        where T : Asn1Object, new()
    {
        /// <summary>
        /// Gets or sets the elements in the composition.
        /// </summary>
        public T[] Elements
        {
            get;
            set;
        }

        //If there are some constraints like SIZE, override VerifyConstraints method in the derived classes

        #region overrode methods from System.Object

        /// <summary>
        /// Overrode method from System.Object.
        /// </summary>
        /// <param name="obj">The object to be compared.</param>
        /// <returns>True if obj has same data with this instance. False if not.</returns>
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Asn1CompositionOfSameType return false.
            Asn1HomogeneousComposition<T> p = obj as Asn1HomogeneousComposition<T>;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match.
            return Enumerable.SequenceEqual<T>(this.Elements, p.Elements);
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
            if (Elements == null)
            {
                throw new Asn1EmptyDataException(ExceptionMessages.EmptyData);
            }
            int resultLen = 0;
            //Encode inversely
            for (int i = Elements.Length - 1; i >= 0; i--)
            {
                resultLen += Elements[i].BerEncode(buffer);
            }
            return resultLen;
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
            int consumedLen = 0;
            List<T> list = new List<T>();
            while (consumedLen < length)
            {
                T curObj = new T();
                consumedLen += curObj.BerDecode(buffer);
                list.Add(curObj);
            }
            Elements = list.ToArray();

            //Ensure consumedLen equals to length
            if (consumedLen > length)
            {
                Elements = null;
                throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData);
            }

            return consumedLen;
        }
        #endregion BER

    }
}
