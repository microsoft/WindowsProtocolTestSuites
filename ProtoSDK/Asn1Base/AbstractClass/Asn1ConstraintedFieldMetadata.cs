// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents a object which stores the meta data of a constrainted field. 
    /// </summary>
    /// <remarks>
    /// For fields like INTEGER and OCTET STRING in SEQUENCE and CHOICE, they may have constraints.
    /// </remarks>
    abstract class Asn1ConstraintedFieldMetadata
    {
        /// <summary>
        /// Stores the reference of the outer class.
        /// </summary>
        protected Asn1Object OutReference;

        /// <summary>
        /// Stores the metadata of the type of the field in the C# definition.
        /// </summary>
        public MemberInfo MemberInfo;

        /// <summary>
        /// Stores the tag of the field.
        /// </summary>
        /// <remarks>
        /// This information will be obtained via the Asn1Tag attributes for the field.
        /// </remarks>
        public Asn1Tag AttachedTag;

        /// <summary>
        /// Stores the constraints of the field.
        /// </summary>
        public Asn1Constraint Constraint;

        /// <summary>
        /// Create a new instance of FieldMetaData with a given MemberInfo.
        /// </summary>
        /// <param name="outRef">Indicates the reference of the out object that hold this constraint.</param>
        /// <param name="info"></param>
        /// <remarks>
        /// AttachedTag, Constraint will be set automatically by reflection.
        /// </remarks>
        protected Asn1ConstraintedFieldMetadata(Asn1Object outRef, MemberInfo info)
        {
            OutReference = outRef;
            MemberInfo = info;

            //Get AttachedTag
            AttachedTag = null;
            var attrs = MemberInfo.GetCustomAttributes(typeof(Asn1Tag), true);
            if (attrs.Length != 0)
            {
                //Only the first tag is valid.
                Asn1Tag tag = attrs[0] as Asn1Tag;
                if (tag.TagType != Asn1TagType.Context && tag.TagType != Asn1TagType.Application)
                {
                    throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                        + " Only Context-Specific tag and Application tag are allowed.");
                }
                AttachedTag = tag;
            }

            //Get Constraint
            Constraint = GetConstraints();
        }

        /// <summary>
        /// Checks whether a type is inherited from another type.
        /// </summary>
        /// <param name="toCheck">The type to be checked.</param>
        /// <param name="baseType"></param>
        /// <returns>Returns true if the to be checked type is inherited from baseType, false if not.</returns>
        private static bool IsInherit(Type toCheck, Type baseType)
        {
            while (toCheck != null)
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (baseType == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        /// <summary>
        /// Checks whether this member is inherited from the given type.
        /// </summary>
        /// <param name="checkType"></param>
        /// <returns></returns>
        private bool IsInheritedFrom(Type checkType)
        {
            if (MemberInfo.MemberType == MemberTypes.Property &&
                IsInherit((MemberInfo as PropertyInfo).PropertyType, checkType)
                ||
                MemberInfo.MemberType == MemberTypes.Field &&
                IsInherit((MemberInfo as FieldInfo).FieldType, checkType))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the constraint defined for the field by reflection.
        /// </summary>
        /// <returns></returns>
        private Asn1Constraint GetConstraints()
        {
            Asn1Constraint Constraint = null;
            Object[] attrs;
            if (IsInheritedFrom(typeof(Asn1Integer)))
            {
                attrs = MemberInfo.GetCustomAttributes(typeof(Asn1IntegerBound), true);
                if (attrs.Length > 0)
                {
                    Constraint = attrs[0] as Asn1IntegerBound;
                }
            }
            else if (IsInheritedFrom(typeof(Asn1String)))
            {
                attrs = MemberInfo.GetCustomAttributes(typeof(Asn1StringConstraint), true);
                if (attrs.Length > 0)
                {
                    Constraint = attrs[0] as Asn1StringConstraint;
                }
            }
            else if (IsInheritedFrom(typeof (Asn1HomogeneousComposition<>)))
            {
                attrs = MemberInfo.GetCustomAttributes(typeof (Asn1SizeConstraint), true);
                if (attrs.Length > 0)
                {
                    Constraint = attrs[0] as Asn1SizeConstraint;
                }
            }
            return Constraint;
        }

        /// <summary>
        /// Set the Constraint stored in this instance to a specific object.
        /// </summary>
        /// <param name="obj"></param>
        private void SetObjectConstraint(Asn1Object obj)
        {
            if (Constraint != null)
            {
                if (IsInheritedFrom(typeof(Asn1Integer)))
                {
                    (obj as Asn1Integer).Constraints = Constraint as Asn1IntegerBound;
                }
                else if (IsInheritedFrom(typeof(Asn1String)))
                {
                    (obj as Asn1OctetString).Constraint = Constraint as Asn1StringConstraint;
                }
            }
        }

        /// <summary>
        /// Gets or sets the stored value of the member in outReference.
        /// </summary>
        public Asn1Object ValueInOutObject
        {
            get
            {
                Asn1Object obj;
                if (MemberInfo.MemberType == MemberTypes.Property)
                {
                    obj = (MemberInfo as PropertyInfo).GetValue(OutReference, null) as Asn1Object;
                }
                else if (MemberInfo.MemberType == MemberTypes.Field)
                {
                    obj = (MemberInfo as FieldInfo).GetValue(OutReference) as Asn1Object;
                }
                else
                {
                    throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                        + " Asn1Field property could only be used in properties or fields.");
                }
                SetObjectConstraint(obj);
                return obj;
            }
            set
            {
                if (MemberInfo.MemberType == MemberTypes.Property)
                {
                    (MemberInfo as PropertyInfo).SetValue(OutReference, value, null);
                }
                else if (MemberInfo.MemberType == MemberTypes.Field)
                {
                    (MemberInfo as FieldInfo).SetValue(OutReference, value);
                }
                else
                {
                    throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                        + " Asn1Field property could only be used in properties or fields.");
                }
            }
        }

        /// <summary>
        /// Get a new instance of the member.
        /// </summary>
        public Asn1Object NewInstance
        {
            get
            {
                Asn1Object obj = null;
                if (MemberInfo.MemberType == MemberTypes.Property)
                {
                    obj = Activator.CreateInstance((MemberInfo as PropertyInfo).PropertyType) as Asn1Object;
                }
                else if (MemberInfo.MemberType == MemberTypes.Field)
                {
                    obj = Activator.CreateInstance((MemberInfo as FieldInfo).FieldType) as Asn1Object;
                }
                else
                {
                    //Unreachable. Ensured by the AttributeUsage of Asn1Field..
                    throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                        + " Asn1Field property could only be used in properties or fields.");
                }
                if (obj == null)
                {
                    throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent
                        + " Can't create instance for member " + MemberInfo.Name
                        + " in class " + GetType().Name + ".");
                }
                SetObjectConstraint(obj);
                return obj;
            }
        }
    }
}
