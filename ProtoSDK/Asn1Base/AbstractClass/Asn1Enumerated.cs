// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Contains some common procedures and properties for ENUMERATED.
    /// </summary>
    /// <remarks>
    /// All ENUMERATED ASN.1 type should be derived from this class.
    /// </remarks>
    [Asn1Tag(Asn1TagType.Universal, Asn1TagValue.Enumerated)]
    public abstract class Asn1Enumerated : Asn1Integer
    {
        //Ref. X.690: 8.4
        //The encoding of an enumerated value shall be that of the integer value with which it is associated.
        //Therefore Asn1Integer is its base class.

        //In derived classes, there should be some public const fields/properties corresponding to the enumerated elements in ASN.1 definition. 
        //These members should have the Asn1EnumeratedElements attribute.
        //For other members, they should not have this attribute.

        /// <summary>
        /// Stores all the above public const values.
        /// </summary>
        /// <remarks>
        /// Doesn't contain the undefinedValue;
        /// </remarks>
        private List<long?> allowedValues = null;

        //Constructors
        private static readonly long? undefinedValue = null;

        /// <summary>
        /// Initializes a new instance of the Asn1Enumerated class with an undefined value.
        /// </summary>
        protected Asn1Enumerated()
            : this(undefinedValue)
        {

        }

        /// <summary>
        /// Initializes a new instance of the Asn1Enumerated class with a given value. 
        /// </summary>
        /// <param name="val"></param>
        protected Asn1Enumerated(long? val)
            : base(val)
        {
            //Get all the allowed values by checking the Asn1EnumeratedElement attribute on each member.

            allowedValues = new List<long?>();

            MemberInfo[] mis = this.GetType().GetMembers(
                BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            foreach (var mi in mis)
            {
                if (mi.IsDefined(typeof(Asn1EnumeratedElement), true))
                {
                    object o = null;
                    if (mi.MemberType == MemberTypes.Property)
                    {
                        PropertyInfo pi = this.GetType().GetProperty(mi.Name);
                        o = pi.GetValue(this, null);
                    }
                    else if (mi.MemberType == MemberTypes.Field)
                    {
                        FieldInfo fi = this.GetType().GetField(mi.Name);
                        o = fi.GetValue(this);
                    }
                    else
                    {
                        //Unreachable. Ensured by the AttributeUsage of Asn1EnumeratedElement.
                        throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                            + " Asn1Field property could only be used in properties or fields.");
                    }

                    if (o == null)
                    {
                        throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                            + " " + mi.Name + " in class " + this.GetType().Name + " can't be null.");
                    }
                    long allowedValue = Convert.ToInt64(o);
                    if (allowedValues.Contains(allowedValue))
                    {
                        throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                            + " Deplicated indices for " + mi.Name + " in class " + this.GetType().Name + ".");
                    }
                    allowedValues.Add(allowedValue);
                }
            }

            if (allowedValues.Count == 0)
            {
                throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent 
                    + String.Format(" There must be at least one enumaration for {0}.", GetType()));
            }
            allowedValues.Sort();

            Constraints = new Asn1IntegerBound() { Min = 0, Max = allowedValues.Count - 1 };
        }

        /// <summary>
        /// Ensures the associated data is in allowedValues.
        /// </summary>
        /// <returns>Return true if it is in allowedValues. Return false if neither.</returns>
        protected sealed override bool VerifyConstraints()
        {
            return (this.Value == undefinedValue || allowedValues.Contains(this.Value));
        }

        //BER encoding & decoding are implemented in the base class

        #region PER

        /// <summary>
        /// Encodes the content of the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer to which the encoding result will be written.</param>
        protected override void ValuePerEncode(IAsn1PerEncodingBuffer buffer)
        {
            int i = 0;
            for (; i < allowedValues.Count; i++)
            {
                if (allowedValues[i] == Value)
                {
                    break;
                }
            }
            if (i == allowedValues.Count)
            {
                throw new Asn1ConstraintsNotSatisfied(ExceptionMessages.ConstraintsNotSatisfied + " Invalid enumeration.");
            }

            Asn1Integer ai = new Asn1Integer(i, Constraints.Min, Constraints.Max);
            ai.PerEncode(buffer);
        }

        /// <summary>
        /// Decodes the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a PER encoding result.</param>
        /// <param name="aligned">Indicating whether the PER decoding is aligned.</param>
        protected override void ValuePerDecode(IAsn1DecodingBuffer buffer, bool aligned = true)
        {
            Asn1Integer ai = new Asn1Integer(null, Constraints.Min, Constraints.Max);
            ai.PerDecode(buffer, aligned);
            Value = allowedValues[(int)ai.Value];
        }

        #endregion PER

    }
}
