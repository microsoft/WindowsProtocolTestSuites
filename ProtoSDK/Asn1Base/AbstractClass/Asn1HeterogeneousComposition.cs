// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Contains some common procedures and properties for SET and SEQUENCE.
    /// </summary>
    /// <remarks>
    /// Fields in an Asn1HeterogeneousComposition may have different types, 
    /// e.g., field 1 is an Asn1Integer, field 2 is an Asn1OctetString, ...
    /// SET and SEQUENCE should be derived from this class.
    /// </remarks>
    public abstract class Asn1HeterogeneousComposition : Asn1Object
    {
        /// <summary>
        /// Stores the metadata of a single field in a SEQUENCE/SET.
        /// </summary>
        private class FieldMetaData : Asn1ConstraintedFieldMetadata
        {
            //Fields from base class:
            //Asn1Object outReference
            //MemberInfo MemberInfo
            //Asn1Tag AttachedTag
            //Asn1Constraint Constraint

            /// <summary>
            /// Indicates whether the field is optional.
            /// </summary>
            /// <remarks>
            /// This information will be obtained via the Asn1Field attributes for the field.
            /// If the field is optional, it will be set to true.
            /// Otherwise, it will be set to false.
            /// </remarks>
            public bool Optional;

            /// <summary>
            /// Create a new instance of FieldMetaData with a given MemberInfo.
            /// </summary>
            /// <param name="outRef">Stores the out reference of the object.</param>
            /// <param name="info"></param>
            /// <remarks>
            /// AttachedTag, Optional, Constraint will be set automatically by reflection.
            /// </remarks>
            public FieldMetaData(Asn1Object outRef, MemberInfo info)
                : base(outRef, info)
            {
                //Get Optional
                var attrs = MemberInfo.GetCustomAttributes(typeof(Asn1Field), true);
                if (attrs == null || attrs.Length == 0)
                {
                    throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent + " No Asn1Field is specified.");
                }
                Optional = (attrs[0] as Asn1Field).Optional;
            }
        }

        private FieldMetaData[] fieldsMetaData;

        /// <summary>
        /// Initialize fieldsMemberInfo, attachedTags fieldOptionalFlags and fieldsConstraints.
        /// </summary>
        /// <exception cref="Asn1UserDefinedTypeInconsistent">
        /// Thrown when Asn1Field attribute is used not only in fields and properties.
        /// </exception>
        private void CollectMetadata()
        {

            //Get metadata for fields
            MemberInfo[] mis = GetType().GetMembers(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            List<MemberInfo> list = new List<MemberInfo>();
            foreach (var mi in mis)
            {
                if (mi.IsDefined(typeof(Asn1Field), true))
                {
                    if (mi.MemberType == MemberTypes.Field ||
                        mi.MemberType == MemberTypes.Property)
                    {
                        list.Add(mi);
                    }
                    else
                    {
                        //Unreachable. Ensured by the AttributeUsage of Asn1Field.
                        throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                            + " Asn1Field attribute could only be used in properties or fields.");
                    }
                }
            }

            //Sort by Asn1Field.Index, make its order consistent with the ASN.1 definition
            MemberInfo[] sortedMetadata = list.OrderBy(mi =>
            {
                var attrs = mi.GetCustomAttributes(typeof(Asn1Field), true);
                if (attrs == null || attrs.Length == 0)
                {
                    throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent + " No Asn1Field is specified.");
                }
                return (attrs[0] as Asn1Field).Index;
            }).ToArray();

            fieldsMetaData = new FieldMetaData[sortedMetadata.Length];
            for (int i = 0; i < fieldsMetaData.Length; i++)
            {
                fieldsMetaData[i] = new FieldMetaData(this, sortedMetadata[i]);
            }
        }

        /// <summary>
        /// Collects the metadata of the class.
        /// </summary>
        protected Asn1HeterogeneousComposition()
        {
            CollectMetadata();
        }

        /// <summary>
        /// Gets all the data in the structure.
        /// </summary>
        /// <returns>An array that contains references to all the data.</returns>
        private Asn1Object[] Fields
        {
            get
            {
                Asn1Object[] fields = new Asn1Object[fieldsMetaData.Length];
                for (int i = 0; i < fields.Length; i++)
                {
                    fields[i] = fieldsMetaData[i].ValueInOutObject;
                }
                return fields;
            }
        }

        /// <summary>
        /// Gets new instances of all the fields in the structure.
        /// </summary>
        /// <returns>An array that contains the instances.</returns>
        /// <remarks>This method is used when decoding. All the elements in the returned array don't have data.</remarks>
        private Asn1Object[] FieldsTypeInstances
        {
            get
            {
                Asn1Object[] fields = new Asn1Object[fieldsMetaData.Length];
                for (int i = 0; i < fieldsMetaData.Length; i++)
                {
                    fields[i] = fieldsMetaData[i].NewInstance;
                }
                return fields;
            }
        }

        /// <summary>
        /// Checks whether all the mandatory fields have had values.
        /// </summary>
        /// <remarks>
        /// This method should be invoked before encoding and after decoding.
        /// </remarks>
        /// <returns>True if all the mandatory fields have values. False if not.</returns>
        protected override bool VerifyConstraints()
        {
            Asn1Object[] allFields = Fields;
            for (int i = 0; i < fieldsMetaData.Length; i++)
            {
                if (fieldsMetaData[i].Optional == false && allFields[i] == null)
                {
                    return false;
                }
            }
            return true;
        }

        #region overrode methods from System.Object

        /// <summary>
        /// Overrode method from System.Object.
        /// </summary>
        /// <param name="obj">The object to be compared.</param>
        /// <returns>True if obj has same data with this instance. False if not.</returns>
        public override bool Equals(object obj)
        {
            // If parameter is null or cannot be cast to Asn1CompositionOfDifferentTypes return false.
            Asn1HeterogeneousComposition p = obj as Asn1HeterogeneousComposition;
            if (p == null)
            {
                return false;
            }

            // Return true if the fields match.
            var fieldsThis = Fields;
            var fieldsObj = p.Fields;
            return fieldsThis.SequenceEqual(fieldsObj);
        }

        /// <summary>
        /// Overrode method from System.Object.
        /// </summary>
        /// <returns>A string that represents this instance.</returns>
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
            int resultLen = 0;

            //Encode inversely
            Asn1Object[] allFields = Fields;
            for (int i = fieldsMetaData.Length - 1; i >= 0; i--)
            {
                if (!(fieldsMetaData[i].Optional && allFields[i] == null))
                {
                    //The field is not optional or its optional but it exists
                    int curFieldLen = allFields[i].BerEncode(buffer);
                    resultLen += curFieldLen;
                    //Encode the context tag if the field has one
                    if (fieldsMetaData[i].AttachedTag != null)
                    {
                        resultLen += LengthBerEncode(buffer, curFieldLen);
                        resultLen += TagBerEncode(buffer, fieldsMetaData[i].AttachedTag);
                    }
                }
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

            Asn1Tag tag = null; //The current decoded tag.
            int tagLen = 0;//The consumed length of the bytes in the buffer when decoding the current tag.
            bool tagUsed = true;

            int curFieldIndex = 0;//The index of the field that currently is trying to decode.

            Asn1Object[] tempFields = FieldsTypeInstances;

            while (consumedLen < length)
            {
                if (curFieldIndex >= tempFields.Length)
                {
                    throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData);
                }

                if (tagUsed)
                {
                    tagLen = TagBerDecode(buffer, out tag);
                    //The decoded tag may be context , application or universal tag.
                }

                if (fieldsMetaData[curFieldIndex].Optional == false)
                {
                    //Current field is mandatory.
                    if (fieldsMetaData[curFieldIndex].AttachedTag != null)
                    {
                        //Current field has a context class tag.
                        //The decoded tag must be the context class tag of the field.
                        if (tag.Equals(fieldsMetaData[curFieldIndex].AttachedTag))
                        {
                            consumedLen += tagLen;
                            int lengthAfterCtxTag;
                            consumedLen += LengthBerDecode(buffer, out lengthAfterCtxTag);
                            //Decodes this field by Asn1Object::BerDecode when the if-else statement for fieldOptionalFlags ends.
                        }
                        else
                        {
                            throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData + " Context Tag Decode fail.");
                        }
                    }
                    else
                    {
                        //Current field doesn't have a context class tag.
                        //The decoded tag must be the application or universal class tag of the field.
                        //Retreat the buffer since the decoding of application or universal class is part of Asn1Object::BerDecode.
                        buffer.SeekBytePosition(-tagLen);
                        //Decode this field by Asn1Object::BerDecode when the if-else statement for fieldOptionalFlags ends.
                    }
                }
                else
                {
                    //Current field is optional.
                    if (fieldsMetaData[curFieldIndex].AttachedTag != null)
                    {
                        //Current field has a context class tag.
                        //Check whether it is the encoding result of this field by the context-specific class tag.
                        if (tag.Equals(fieldsMetaData[curFieldIndex].AttachedTag))
                        {
                            //Yes
                            consumedLen += tagLen;
                            int lengthAfterCtxTag;
                            consumedLen += LengthBerDecode(buffer, out lengthAfterCtxTag);
                            //Decode this field by Asn1Object::BerDecode when the if-else statement for OptionalFlags ends.
                        }
                        else
                        {
                            //No
                            tagUsed = false;
                            tempFields[curFieldIndex] = null;
                            curFieldIndex++;
                            //Check the next field.
                            continue;
                        }
                    }
                    else
                    {
                        //Current field doesn't have a context class tag.
                        //Check whether it is the encoding result of this field by the top most class tag.
                        if (tag.Equals(tempFields[curFieldIndex].TopTag))
                        {
                            //Yes
                            buffer.SeekBytePosition(-tagLen);
                            //Decode this field by Asn1Object::BerDecode when the if-else statement for OptionalFlags ends.
                        }
                        else
                        {
                            //No
                            tagUsed = false;
                            tempFields[curFieldIndex] = null;
                            curFieldIndex++;
                            //Check the next field.
                            continue;
                        }
                    }
                }
                consumedLen += tempFields[curFieldIndex].BerDecode(buffer);
                tagUsed = true;
                curFieldIndex++;
            }

            //Ensure consumedLen equals to length.
            if (consumedLen > length)
            {
                throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData);
            }

            //Stores the decoded fields in this object.
            if (tempFields.Length != fieldsMetaData.Length)
            {
                throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData);
            }

            for (int i = 0; i < curFieldIndex; i++)
            {
                if (fieldsMetaData[i].MemberInfo.MemberType == MemberTypes.Property)
                {
                    (fieldsMetaData[i].MemberInfo as PropertyInfo).SetValue(this, tempFields[i], null);
                }
                else if (fieldsMetaData[i].MemberInfo.MemberType == MemberTypes.Field)
                {
                    (fieldsMetaData[i].MemberInfo as FieldInfo).SetValue(this, tempFields[i]);
                }
                else
                {
                    //Unreachable. Ensured by the AttributeUsage of Asn1Field.
                    throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                        + " Asn1Field property could only be used in properties or fields.");
                }
            }
            return consumedLen;
        }

        #endregion BER

        #region PER

        /// <summary>
        /// Encodes the content of the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer to which the encoding result will be written.</param>
        protected override void ValuePerEncode(IAsn1PerEncodingBuffer buffer)
        {
            //Ref. X.691: 18, a bit-map preamble records the presence or absence of default and optional components
            Asn1Object[] fields = Fields;
            List<bool> bitMap = new List<bool>();
            for (int i = 0; i < fields.Length; i++)
            {
                if (fieldsMetaData[i].Optional)
                {
                    //1: presence; 0: absence
                    bitMap.Add(fields[i] != null);
                }
            }
            if (bitMap.Count >= 64 * 1024) //64k
            {
                throw new NotImplementedException("More than 64K optional fields are not supported yet.");
                //Ref. X.691: 18.3
            }
            buffer.WriteBits(bitMap.ToArray());
            foreach (var v in fields)
            {
                if (v != null)
                {
                    v.PerEncode(buffer);
                }
            }
        }

        /// <summary>
        /// Decodes the content of the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a PER encoding result.</param>
        /// <param name="aligned">Indicating whether the PER decoding is aligned.</param>
        protected override void ValuePerDecode(IAsn1DecodingBuffer buffer, bool aligned = true)
        {
            int optionalFieldsCount = 0;
            foreach (var v in fieldsMetaData)
            {
                if (v.Optional)
                {
                    optionalFieldsCount++;
                }
            }
            if (optionalFieldsCount >= 64 * 1024) //64k
            {
                throw new NotImplementedException("More than 64K optional fields are not supported yet.");
                //Ref. X.691: 18.3
            }
            bool[] bitMap = buffer.ReadBits(optionalFieldsCount);
            Asn1Object[] decodingResult = FieldsTypeInstances;
            int curOptionalFlagIndex = 0; //index in bitMap
            for (int i = 0; i < decodingResult.Length; i++)
            {
                if (fieldsMetaData[i].Optional == false ||
                    fieldsMetaData[i].Optional && bitMap[curOptionalFlagIndex++])
                {
                    decodingResult[i].PerDecode(buffer, aligned);
                }
                else
                {
                    //fieldOptionalFlags[i] equals to true and bitMap[curOptionalFlagIndex] equals to false.
                    decodingResult[i] = null;
                }
            }

            for (int i = 0; i < decodingResult.Length; i++)
            {
                fieldsMetaData[i].ValueInOutObject = decodingResult[i];
            }
        }

        #endregion PER

    }
}
