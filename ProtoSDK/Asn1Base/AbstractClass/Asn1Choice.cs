// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Contains some common procedures and properties for CHOICE.
    /// </summary>
    /// <remarks>
    /// All CHOICE ASN.1 type should be derived from this class.
    /// </remarks>
    public abstract class Asn1Choice : Asn1Object
    {
        #region index

        //In derived classes, there should be some public const int values which specify the indices of the choices.
        //There values should have Asn1ChoiceIndex attribute.

        /// <summary>
        /// Stores all the above public const values.
        /// </summary>
        /// <remarks>
        /// Also contains the undefinedIndex;
        /// </remarks>
        private List<long?> allowedIndices = null;

        /// <summary>
        /// Stores the index of the current choice.
        /// </summary>
        /// <remarks>
        /// Should be one of the values in allowedIndices.
        /// </remarks>
        private long? currentChoice;

        /// <summary>
        /// Gets the selected ID for the current choice.
        /// </summary>
        public long? SelectedChoice
        {
            get { return currentChoice; }
        }

        /// <summary>
        /// Ensures the current choice is in allowedIndices.
        /// </summary>
        /// <returns>Return true if it is in allowedIndices. Return false if neither.</returns>
        protected sealed override bool VerifyConstraints()
        {
            return allowedIndices.Contains(this.currentChoice);
        }

        /// <summary>
        /// Specifies an undefined index.
        /// </summary>
        protected static readonly long? undefinedIndex = null;

        #endregion index

        #region choice elements

        //In derived classes, there should be some protected properties/fields.
        //Each property/field corresponds to a choice element in the definition.
        //Additionally, it has the same type as the corresponding choice element.
        //Each property/field should have the Asn1ChoiceElement attribute.
        //This attribute will build a mapping from the propery/field to the index.

        //Stores the metadata
        private MemberInfo[] fieldsMemberInfo = null;
        private Asn1Tag[] attachedTags = null;

        /// <summary>
        /// Indicates the index of the chosen field for the CHOICE in fieldsMemberInfo.
        /// </summary>
        private long? choiceIndexInFieldsMemberInfo = undefinedIndex;

        /// <summary>
        /// Collects the metadata in the definition.
        /// </summary>
        private void CollectMetadata()
        {
            //TODO: ensure that only collect once for each derived class.

            //Get the indices and choices
            allowedIndices = new List<long?>();
            List<MemberInfo> choiceList = new List<MemberInfo>();
            List<long?> allIndicesDefinedInFields = new List<long?>();

            MemberInfo[] mis = this.GetType().GetMembers(
                BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance |
                BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            foreach (var mi in mis)
            {
                if (mi.IsDefined(typeof(Asn1ChoiceIndex), true))
                {
                    object o;
                    if (mi.MemberType == MemberTypes.Property)
                    {
                        PropertyInfo pi = mi as PropertyInfo;
                        o = pi.GetValue(this, null);
                    }
                    else if (mi.MemberType == MemberTypes.Field)
                    {
                        FieldInfo fi = mi as FieldInfo;
                        o = fi.GetValue(this);
                    }
                    else
                    {
                        throw new Asn1UnreachableExcpetion(ExceptionMessages.Unreachable);
                    }
                    long allowedIndex = Convert.ToInt64(o);
                    if (allowedIndices.Contains(allowedIndex))
                    {
                        throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent +
                        " Duplicated indices.");
                    }
                    allowedIndices.Add(allowedIndex);
                }
                if (mi.IsDefined(typeof(Asn1ChoiceElement), true))
                {
                    if (mi.MemberType == MemberTypes.Field ||
                        mi.MemberType == MemberTypes.Property)
                    {
                        choiceList.Add(mi);
                        var attrs = mi.GetCustomAttributes(typeof(Asn1ChoiceElement), true);
                        allIndicesDefinedInFields.Add((attrs[0] as Asn1ChoiceElement).Index);
                    }
                    else
                    {
                        throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                            + " Asn1ChoiceElement property could only be used in properties or fields.");
                    }
                }
            }

            //Ensure the consistency of the definition.
            allowedIndices.Sort();
            allIndicesDefinedInFields.Sort();
            if (!Enumerable.SequenceEqual<long?>(allowedIndices, allIndicesDefinedInFields))
            {
                throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent +
                    " Stated Indices are different from the indices in choice elements' attributes.");
            }

            allowedIndices.Add(undefinedIndex);

            //Sort the fields by the defined order
            fieldsMemberInfo = choiceList.OrderBy(mi =>
            {
                var attrs = mi.GetCustomAttributes(typeof(Asn1ChoiceElement), true);
                return (attrs[0] as Asn1ChoiceElement).Index;
            }).ToArray();

            //Get the tags in defined order
            attachedTags = new Asn1Tag[fieldsMemberInfo.Length];

            for (int i = 0; i < fieldsMemberInfo.Length; i++)
            {
                var attrs = fieldsMemberInfo[i].GetCustomAttributes(typeof(Asn1Tag), true);
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

        #endregion choice elements

        /// <summary>
        /// Initializes a new instance of the Asn1Choice class with a given value.
        /// </summary>
        /// <param name="choiceIndex">The corresponding index of the value in the choices.</param>
        /// <param name="obj"></param>
        protected Asn1Choice(long? choiceIndex, Asn1Object obj)
        {
            CollectMetadata();
            SetData(choiceIndex, obj);
        }

        /// <summary>
        /// Initializes a new instance of the Asn1Choice class with undefined value. 
        /// </summary>
        protected Asn1Choice()
            : this(undefinedIndex, null)
        {

        }

        /// <summary>
        /// Stores the data in the CHOICE structure.
        /// </summary>
        /// <param name="index">The corresponding index of the choices defined in the type for obj. 
        /// Use the const value defined in the class as this parameter.</param>
        /// <param name="obj">The data to be stored in the CHOICE.</param>
        public void SetData(long? index, Asn1Object obj)
        {
            if (index == undefinedIndex)
            {
                this.currentChoice = undefinedIndex;
                this.choiceIndexInFieldsMemberInfo = undefinedIndex;
                return;
            }
            if (allowedIndices.Contains(index))
            {
                for (int i = 0; i < fieldsMemberInfo.Length; i++)
                {
                    var attrs = fieldsMemberInfo[i].GetCustomAttributes(typeof(Asn1ChoiceElement), true);
                    if ((attrs[0] as Asn1ChoiceElement).Index == index)
                    {
                        currentChoice = index;
                        choiceIndexInFieldsMemberInfo = i;
                        //Store the data
                        if (fieldsMemberInfo[i].MemberType == MemberTypes.Property)
                        {
                            (fieldsMemberInfo[i] as PropertyInfo).SetValue(this, obj, null);
                        }
                        else if (fieldsMemberInfo[i].MemberType == MemberTypes.Field)
                        {
                            (fieldsMemberInfo[i] as FieldInfo).SetValue(this, obj);
                        }
                        else
                        {
                            throw new Asn1UnreachableExcpetion(ExceptionMessages.Unreachable);
                        }
                        break;
                    }
                }
            }
            else
            {
                throw new Asn1InvalidArgument(ExceptionMessages.InvalidChoiceIndex);
            }
        }

        /// <summary>
        /// Gets the data stored in the CHOICE structure.
        /// </summary>
        /// <returns>The reference of the object.</returns>
        public Asn1Object GetData()
        {
            if (choiceIndexInFieldsMemberInfo == undefinedIndex)
            {
                return null;
            }

            MemberInfo mi = fieldsMemberInfo[(int)choiceIndexInFieldsMemberInfo];

            if (mi.MemberType == MemberTypes.Property)
            {
                return (mi as PropertyInfo).GetValue(this, null) as Asn1Object;
            }
            else if (mi.MemberType == MemberTypes.Field)
            {
                return (mi as FieldInfo).GetValue(this) as Asn1Object;
            }
            else
            {
                throw new Asn1UnreachableExcpetion(ExceptionMessages.Unreachable);
            }
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

            // If parameter cannot be cast to Asn1Choice return false.
            Asn1Choice p = obj as Asn1Choice;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match.
            return GetData().Equals(p.GetData());
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

        /// <summary>
        /// Gets the top most tag of the structure while in BER encoding.
        /// </summary>
        public override Asn1Tag TopTag
        {
            get
            {
                Object[] attrs = this.GetType().GetCustomAttributes(typeof(Asn1Tag), true);
                if (attrs.Length == 0)
                {
                    return GetData().TopTag;
                }
                else
                {
                    return (attrs[0] as Asn1Tag);
                }
            }
        }

        /// <summary>
        /// Gets instances of all the choices in the structure.
        /// </summary>
        /// <returns>An array that contains the instances.</returns>
        private Asn1Object[] GetChoiceTypeInstances()
        {
            Asn1Object[] allChoices = new Asn1Object[fieldsMemberInfo.Length];
            for (int i = 0; i < fieldsMemberInfo.Length; i++)
            {
                if (fieldsMemberInfo[i].MemberType == MemberTypes.Property)
                {
                    allChoices[i] = Activator.CreateInstance((fieldsMemberInfo[i] as PropertyInfo).PropertyType) as Asn1Object;
                }
                else if (fieldsMemberInfo[i].MemberType == MemberTypes.Field)
                {
                    allChoices[i] = Activator.CreateInstance((fieldsMemberInfo[i] as FieldInfo).FieldType) as Asn1Object;
                }
                else
                {
                    throw new Asn1UnreachableExcpetion(ExceptionMessages.Unreachable);
                }
            }
            return allChoices;
        }

        /// <summary>
        /// Encodes the object by BER.
        /// </summary>
        /// <param name="buffer">A buffer that stores the BER encoding result.</param>
        /// <param name="explicitTag">Indicates whether the tags should be encoded explicitly. In our Test Suites, it will always be true.</param>
        /// <returns>The length of the encoding result of this object.</returns>
        /// <exception cref="Asn1EmptyDataException">
        /// Thrown when trying to encode an undefined data. 
        /// </exception>
        public override int BerEncode(IAsn1BerEncodingBuffer buffer, bool explicitTag = true)
        {
            //TODO: deal with explicitTag
            if (currentChoice == undefinedIndex)
            {
                throw new Asn1EmptyDataException(ExceptionMessages.EmptyData);
            }

            int length = GetData().BerEncode(buffer);

            if (attachedTags[(int)choiceIndexInFieldsMemberInfo] != null)
            {
                length += LengthBerEncode(buffer, length);
                length += TagBerEncode(buffer, attachedTags[(int)choiceIndexInFieldsMemberInfo]);
            }

            Object[] attrs = this.GetType().GetCustomAttributes(typeof(Asn1Tag), true);
            if (attrs.Length != 0)
            {
                Asn1Tag tag = attrs[0] as Asn1Tag;
                length += LengthBerEncode(buffer, length);
                length += TagBerEncode(buffer, tag);
            }
            return length;
        }

        /// <summary>
        /// Decodes the object by BER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a BER encoding result.</param>
        /// <returns>The number of the bytes consumed in the buffer to decode this object.</returns>
        /// <exception cref="Asn1DecodingUnexpectedData">
        /// Thrown when the data in the buffer can not be properly decoded.
        /// </exception>
        public override int BerDecode(IAsn1DecodingBuffer buffer)
        {
            int length = 0;
            Object[] attrs = this.GetType().GetCustomAttributes(typeof(Asn1Tag), true);
            Asn1Tag topTag;
            if (attrs.Length != 0)
            {
                Asn1Tag topTagInDefinition = attrs[0] as Asn1Tag;
                length += TagBerDecode(buffer, out topTag);
                if (!topTag.Equals(topTagInDefinition))
                {
                    throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData);
                }
                int tempLength;
                length += LengthBerDecode(buffer, out tempLength);
            }

            //Determine which choice should be decoded.
            int? decodingTargetIndex = null;
            int tagLen = TagBerDecode(buffer, out topTag);
            Asn1Object[] choiceInstances = GetChoiceTypeInstances();
            for (int i = 0; i < fieldsMemberInfo.Length; i++)
            {
                if (attachedTags[i] != null)
                {
                    if (topTag.Equals(attachedTags[i]))
                    {
                        //Decode this choice.
                        length += tagLen;
                        int lengthAfterTopTag;
                        length += LengthBerDecode(buffer, out lengthAfterTopTag);
                        decodingTargetIndex = i;
                        break;
                    }
                    //else check the next choice.
                }
                else
                {
                    if (topTag.Equals(choiceInstances[i].TopTag))
                    {
                        //Decode this choice.
                        decodingTargetIndex = i;
                        buffer.SeekBytePosition(-tagLen);
                        break;
                    }
                    //else check the next choice.
                }
            }
            if (decodingTargetIndex == null)
            {
                throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData
                    + " None of the choices fit the data in the buffer.");
            }
            //Decode the data.
            length += choiceInstances[(int)decodingTargetIndex].BerDecode(buffer);
            //Store the data in the CHOICE.
            attrs = fieldsMemberInfo[(int)decodingTargetIndex].GetCustomAttributes(typeof(Asn1ChoiceElement), true);
            SetData((attrs[0] as Asn1ChoiceElement).Index, choiceInstances[(int)decodingTargetIndex]);
            return length;
        }

    }
}
