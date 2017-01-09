// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

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
        //The ascending order of the values should be the same as the the canonical order for the alternatives. Ref: X.691 22.2 and X.680 8.6

        /// <summary>
        /// Stores all the above public const values.
        /// </summary>
        private List<long?> definedAllowedIndices;

        /// <summary>
        /// Gets the selected choice index for the current choice.
        /// </summary>
        public long? SelectedChoice { get; private set; }

        /// <summary>
        /// Ensures the current choice is in allowedIndices.
        /// </summary>
        /// <returns>Return true if it is in allowedIndices. Return false if neither.</returns>
        protected sealed override bool VerifyConstraints()
        {
            return SelectedChoice == UndefinedIndex || definedAllowedIndices.Contains(SelectedChoice);
        }

        /// <summary>
        /// Specifies an undefined index.
        /// </summary>
        protected static readonly long? UndefinedIndex = null;

        /// <summary>
        /// Indicates the array index of the chosen field for the CHOICE in fieldsMemberInfo.
        /// </summary>
        /// <remarks>
        /// definedAllowedIndices = [1,3,5,7,9], SelectedChoice is 7 and then choiceIndexInFieldsMemberInfo is 3.
        /// </remarks>
        private long? choiceIndexInFieldsMemberInfo = UndefinedIndex;

        #endregion index

        #region choice elements

        //In derived classes, there should be some protected properties/fields.
        //Each property/field corresponds to a choice element in the definition.
        //Additionally, it has the same type as the corresponding choice element.
        //Each property/field should have the Asn1ChoiceElement attribute.
        //This attribute will build a mapping from the propery/field to the index mentioned above.

        /// <summary>
        /// Stores the metadata of a single field in a Choice.
        /// </summary>
        private class ChoiceMetaData : Asn1ConstraintedFieldMetadata
        {
            //Fields from base class:
            //Asn1Object outReference
            //MemberInfo MemberInfo
            //Asn1Tag AttachedTag
            //Asn1Constraint Constraint

            /// <summary>
            /// Stores the index defined for the choice.
            /// </summary>
            /// <remarks>
            /// This information will be obtained via the Asn1ChoiceElement attributes for the field.
            /// </remarks>
            public readonly long? AttachedIndex;

            /// <summary>
            /// Create a new instance of FieldMetaData with a given MemberInfo.
            /// </summary>
            /// <param name="outRef">Specifies the out reference of the field.</param>
            /// <param name="info"></param>
            /// <remarks>
            /// AttachedTag, Optional, Constraint will be set automatically by reflection.
            /// </remarks>
            public ChoiceMetaData(Asn1Object outRef, MemberInfo info)
                : base(outRef, info)
            {
                //Get Optional
                var attrs = MemberInfo.GetCustomAttributes(typeof(Asn1ChoiceElement), true);
                if (attrs == null || attrs.Length == 0)
                {
                    throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent + " No choice element in CHOICE is defined.");
                }
                AttachedIndex = ((Asn1ChoiceElement)attrs[0]).Index;
            }
        }

        //Stores the metadatas for all choices.
        private ChoiceMetaData[] metaDatas;

        /// <summary>
        /// Collects the metadata in the definition.
        /// </summary>
        private void CollectMetadata()
        {
            //Get the indices and choices
            definedAllowedIndices = new List<long?>();
            List<ChoiceMetaData> metaDataList = new List<ChoiceMetaData>();

            MemberInfo[] mis = GetType().GetMembers(
                BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance |
                BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            foreach (var mi in mis)
            {
                //Get Choice Index.
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
                    if (definedAllowedIndices.Contains(allowedIndex))
                    {
                        throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent +
                        " Duplicated indices.");
                    }
                    definedAllowedIndices.Add(allowedIndex);
                }
                //Get Choice.
                if (mi.IsDefined(typeof(Asn1ChoiceElement), true))
                {
                    if (mi.MemberType == MemberTypes.Field ||
                        mi.MemberType == MemberTypes.Property)
                    {
                        ChoiceMetaData metaData = new ChoiceMetaData(this, mi);
                        metaDataList.Add(metaData);
                    }
                    else
                    {
                        throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                            + " Asn1ChoiceElement property could only be used in properties or fields.");
                    }
                }
            }

            //Ensure the consistency of the definition.
            definedAllowedIndices.Sort();
            metaDataList = new List<ChoiceMetaData>(metaDataList.OrderBy(var => var.AttachedIndex));
            List<long?> indicesDefinedInChoices = metaDataList.ConvertAll(var => var.AttachedIndex);
            if (!definedAllowedIndices.SequenceEqual(indicesDefinedInChoices))
            {
                throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent +
                    " Stated Indices are different from the indices in choice elements' attributes.");
            }

            metaDatas = metaDataList.ToArray();
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
            : this(UndefinedIndex, null)
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
            if (index == UndefinedIndex)
            {
                SelectedChoice = UndefinedIndex;
                choiceIndexInFieldsMemberInfo = UndefinedIndex;
                return;
            }
            if (definedAllowedIndices.Contains(index))
            {
                if (HasExternalObjects && index == definedAllowedIndices[definedAllowedIndices.Count - 1])
                {
                    throw new NotImplementedException("Assigning value to external objects is not implemented.");
                }
                for (int i = 0; i < metaDatas.Length; i++)
                {
                    if (metaDatas[i].AttachedIndex == index)
                    {
                        SelectedChoice = index;
                        choiceIndexInFieldsMemberInfo = i;
                        //Store the data
                        metaDatas[i].ValueInOutObject = obj;
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
            if (choiceIndexInFieldsMemberInfo == UndefinedIndex)
            {
                return null;
            }

            return metaDatas[(int)choiceIndexInFieldsMemberInfo].ValueInOutObject;
        }

        #region overrode methods from System.Object

        /// <summary>
        /// Overrode method from System.Object.
        /// </summary>
        /// <param name="obj">The object to be compared.</param>
        /// <returns>True if obj has same data with this instance. False if not.</returns>
        public override bool Equals(object obj)
        {
            // If parameter is null or cannot be cast to Asn1Choice return false.
            Asn1Choice p = obj as Asn1Choice;
            if (p == null)
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
                Object[] attrs = GetType().GetCustomAttributes(typeof(Asn1Tag), true);
                if (attrs.Length == 0)
                {
                    return GetData().TopTag;
                }
                return (attrs[0] as Asn1Tag);
            }
        }

        /// <summary>
        /// Gets instances of all the choices in the structure.
        /// </summary>
        /// <returns>An array that contains the instances.</returns>
        private Asn1Object[] ChoiceTypeInstances
        {
            get
            {
                Asn1Object[] allChoices = new Asn1Object[metaDatas.Length];
                for (int i = 0; i < allChoices.Length; i++)
                {
                    allChoices[i] = metaDatas[i].NewInstance;
                }
                return allChoices;
            }
        }

        #region BER

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
            if (SelectedChoice == UndefinedIndex)
            {
                throw new Asn1EmptyDataException(ExceptionMessages.EmptyData);
            }

            int length = GetData().BerEncode(buffer, explicitTag);

            Asn1Tag contextTag = metaDatas[(int)choiceIndexInFieldsMemberInfo].AttachedTag;
            if (contextTag != null)
            {
                length += LengthBerEncode(buffer, length);
                length += TagBerEncode(buffer, contextTag);
            }

            Object[] attrs = GetType().GetCustomAttributes(typeof(Asn1Tag), true);
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
        /// <param name="explicitTag">Indicates whether the tags should be encoded explicitly. In our Test Suites, it will always be true.</param>
        /// <returns>The number of the bytes consumed in the buffer to decode this object.</returns>
        /// <exception cref="Asn1DecodingUnexpectedData">
        /// Thrown when the data in the buffer can not be properly decoded.
        /// </exception>
        public override int BerDecode(IAsn1DecodingBuffer buffer, bool explicitTag = true)
        {
            int length = 0;
            Object[] attrs = GetType().GetCustomAttributes(typeof(Asn1Tag), true);
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
            Asn1Object[] choiceInstances = ChoiceTypeInstances;
            for (int i = 0; i < metaDatas.Length; i++)
            {
                if (metaDatas[i].AttachedTag != null)
                {
                    if (topTag.Equals(metaDatas[i].AttachedTag))
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
            SetData(metaDatas[(int)decodingTargetIndex].AttachedIndex, choiceInstances[(int)decodingTargetIndex]);
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
            //Encode an index specifying the chosen alternative, Ref: X.691: 22
            Asn1Integer ai = new Asn1Integer(choiceIndexInFieldsMemberInfo, 0, definedAllowedIndices.Count - 1);
            ai.PerEncode(buffer);
            //Encode the chosen alternative
            Asn1Object obj = GetData();
            obj.PerEncode(buffer);
        }

        /// <summary>
        /// Decodes the content of the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a PER encoding result.</param>
        /// <param name="aligned">Indicating whether the PER decoding is aligned.</param>
        protected override void ValuePerDecode(IAsn1DecodingBuffer buffer, bool aligned = true)
        {
            //Decode the index specifying the chosen alternative, Ref: X.691: 22
            Asn1Integer ai = new Asn1Integer(null, 0, definedAllowedIndices.Count - 1);
            ai.PerDecode(buffer);
            //Decode the chosen alternative
            Asn1Object[] instances = ChoiceTypeInstances;
            Asn1Object obj = instances[(int)ai.Value];
            obj.PerDecode(buffer);
            SetData(definedAllowedIndices[(int)ai.Value], obj);
        }

        #endregion PER
    }
}
