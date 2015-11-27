// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Collections.Generic;
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
        /// Stores the metadata of the type of the fields in the C# definition.
        /// </summary>
        private MemberInfo[] fieldsMemberInfo = null;

        /// <summary>
        /// Stores the tags of the fields.
        /// </summary>
        /// <remarks>
        /// This information will be obtained via the Asn1Tag attributes for the fields.
        /// </remarks>
        private Asn1Tag[] attachedTags = null;

        /// <summary>
        /// Stores the flags which specify whether a field is optional.
        /// </summary>
        /// <remarks>
        /// This information will be obtained via the Asn1Field attributes for the fields.
        /// If the field corresponding to fieldsMemberInfo[i] is optional, fieldOptionalFlags[i] will be set to true.
        /// Otherwise, it will be set to false.
        /// </remarks>
        private bool[] fieldOptionalFlags = null;

        /// <summary>
        /// Initialize fieldsMemberInfo, attachedTags and fieldOptionalFlags.
        /// </summary>
        /// <exception cref="Asn1UserDefinedTypeInconsistent">
        /// Thrown when Asn1Field attribute is used not only in fields and properties.
        /// </exception>
        private void CollectMetadata()
        {
            //TODO: ensure that only collect once for each derived class.
            MemberInfo[] mis = this.GetType().GetMembers(
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

            fieldsMemberInfo = list.OrderBy(mi =>
            {
                var attrs = mi.GetCustomAttributes(typeof(Asn1Field), true);
                return (attrs[0] as Asn1Field).Index;
            }).ToArray();

            attachedTags = new Asn1Tag[fieldsMemberInfo.Length];
            fieldOptionalFlags = new bool[fieldsMemberInfo.Length];

            for (int i = 0; i < fieldsMemberInfo.Length; i++)
            {
                var attrs = fieldsMemberInfo[i].GetCustomAttributes(typeof(Asn1Field), true);
                fieldOptionalFlags[i] = (attrs[0] as Asn1Field).Optional;
                attrs = fieldsMemberInfo[i].GetCustomAttributes(typeof(Asn1Tag), true);
                if (attrs.Length != 0)
                {
                    //Only the first tag is valid.
                    Asn1Tag tag = attrs[0] as Asn1Tag;
                    if (tag.TagType != Asn1TagType.Context && tag.TagType != Asn1TagType.Application)
                    {
                        throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                            + " Only Context-Specific and Application tags are allowed for fields.");
                    }
                    attachedTags[i] = tag;
                }
                //attachedTags[i] keeps equaling to null if there is no attched tags for the field.

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
                Asn1Object[] fields = new Asn1Object[fieldsMemberInfo.Length];
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fieldsMemberInfo[i].MemberType == MemberTypes.Property)
                    {
                        fields[i] = (fieldsMemberInfo[i] as PropertyInfo).GetValue(this, null) as Asn1Object;
                    }
                    else if (fieldsMemberInfo[i].MemberType == MemberTypes.Field)
                    {
                        fields[i] = (fieldsMemberInfo[i] as FieldInfo).GetValue(this) as Asn1Object;
                    }
                    else
                    {
                        //Unreachable. Ensured by the AttributeUsage of Asn1Field..
                        throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                            + " Asn1Field property could only be used in properties or fields.");
                    }
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
            for (int i = 0; i < fieldOptionalFlags.Length; i++)
            {
                if (fieldOptionalFlags[i] == false && allFields[i] == null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets new instances of all the fields in the structure.
        /// </summary>
        /// <returns>An array that contains the instances.</returns>
        /// <remarks>This method is used when decoding. All the elements in the returned array don't have data.</remarks>
        private Asn1Object[] GetFieldsTypeInstances()
        {
            Asn1Object[] allFields = new Asn1Object[fieldsMemberInfo.Length];
            for (int i = 0; i < fieldsMemberInfo.Length; i++)
            {
                if (fieldsMemberInfo[i].MemberType == MemberTypes.Property)
                {
                    allFields[i] = Activator.CreateInstance((fieldsMemberInfo[i] as PropertyInfo).PropertyType) as Asn1Object;
                }
                else if (fieldsMemberInfo[i].MemberType == MemberTypes.Field)
                {
                    allFields[i] = Activator.CreateInstance((fieldsMemberInfo[i] as FieldInfo).FieldType) as Asn1Object;
                }
                else
                {
                    //Unreachable. Ensured by the AttributeUsage of Asn1Field..
                    throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                        + " Asn1Field property could only be used in properties or fields.");
                }
                if (allFields[i] == null)
                {
                    throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                        + " Can't create instance for member " + fieldsMemberInfo[i].Name
                        + " in class " + this.GetType().Name + ".");
                }
            }
            return allFields;
        }

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

            // If parameter cannot be cast to Asn1CompositionOfDifferentTypes return false.
            Asn1HeterogeneousComposition p = obj as Asn1HeterogeneousComposition;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match.
            Asn1Object[] fieldsThis = Fields;
            Asn1Object[] fieldsObj = p.Fields;
            return Enumerable.SequenceEqual<Asn1Object>(fieldsThis, fieldsObj);
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
            for (int i = attachedTags.Length - 1; i >= 0; i--)
            {
                if (!(fieldOptionalFlags[i] == true && allFields[i] == null))
                {
                    //The field is not optional or its optional but it exists
                    int curFieldLen = allFields[i].BerEncode(buffer);
                    resultLen += curFieldLen;
                    //Encode the context tag if the field has one
                    if (attachedTags[i] != null)
                    {
                        resultLen += LengthBerEncode(buffer, curFieldLen);
                        resultLen += TagBerEncode(buffer, attachedTags[i]);
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

            Asn1Object[] tempFields = GetFieldsTypeInstances();

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

                if (fieldOptionalFlags[curFieldIndex] == false)
                {
                    //Current field is mandatory.
                    if (attachedTags[curFieldIndex] != null)
                    {
                        //Current field has a context class tag.
                        //The decoded tag must be the context class tag of the field.
                        if (tag.Equals(attachedTags[curFieldIndex]))
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
                    if (attachedTags[curFieldIndex] != null)
                    {
                        //Current field has a context class tag.
                        //Check whether it is the encoding result of this field by the context-specific class tag.
                        if (tag.Equals(attachedTags[curFieldIndex]))
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
            if (tempFields.Length != fieldsMemberInfo.Length)
            {
                throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData);
            }

            for (int i = 0; i < curFieldIndex; i++)
            {
                if (fieldsMemberInfo[i].MemberType == MemberTypes.Property)
                {
                    (fieldsMemberInfo[i] as PropertyInfo).SetValue(this, tempFields[i], null);
                }
                else if (fieldsMemberInfo[i].MemberType == MemberTypes.Field)
                {
                    (fieldsMemberInfo[i] as FieldInfo).SetValue(this, tempFields[i]);
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

    }
}
