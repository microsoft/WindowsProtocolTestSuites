// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Records the size constraint of the type. 
        /// </summary>
        protected internal Asn1SizeConstraint Constraint;

        /// <summary>
        /// Sets the constraint by reflection.
        /// </summary>
        protected Asn1HomogeneousComposition()
        {
            Constraint = null;

            //Gets the size constraint for the structure.
            var allAttributes = GetType().GetCustomAttributes(true);
            foreach (object o in allAttributes)
            {
                if (o is Asn1SizeConstraint)
                {
                    Constraint = o as Asn1SizeConstraint;
                    break;
                }
            }
        }

        #region overrode methods from System.Object

        /// <summary>
        /// Overrode method from System.Object.
        /// </summary>
        /// <param name="obj">The object to be compared.</param>
        /// <returns>True if obj has same data with this instance. False if not.</returns>
        public override bool Equals(object obj)
        {
            // If parameter is null or cannot be cast to Asn1CompositionOfSameType return false.
            var p = obj as Asn1HomogeneousComposition<T>;
            return p != null && Elements.SequenceEqual(p.Elements);
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

        #region PER

        /// <summary>
        /// Encodes the content of the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer to which the encoding result will be written.</param>
        /// <remarks>Length is included.</remarks>
        protected override void ValuePerEncode(IAsn1PerEncodingBuffer buffer)
        {
            Asn1StandardProcedure.PerEncodeArray(buffer, Elements, (encodingBuffer, obj) =>
            {
                obj.PerEncode(encodingBuffer);
            },
            Constraint != null && Constraint.HasMinSize ? Constraint.MinSize : (long?)null, Constraint != null && Constraint.HasMaxSize ? Constraint.MaxSize : (long?)null);
        }

        /// <summary>
        /// Decodes the content of the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a PER encoding result.</param>
        /// <param name="aligned">Indicating whether the PER decoding is aligned.</param>
        /// <remarks>Length is included.</remarks>
        protected override void ValuePerDecode(IAsn1DecodingBuffer buffer, bool aligned = true)
        {
            Elements = Asn1StandardProcedure.PerDecodeArray(buffer, decodingBuffer =>
            {
                T curObj = new T();
                curObj.PerDecode(buffer);
                return curObj;
            }, 
            Constraint != null && Constraint.HasMinSize ? Constraint.MinSize : (long?)null, Constraint != null && Constraint.HasMaxSize ? Constraint.MaxSize : (long?)null);
        }

        #endregion PER
    }
}
