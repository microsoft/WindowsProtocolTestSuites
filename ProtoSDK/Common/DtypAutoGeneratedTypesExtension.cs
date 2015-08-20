// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.Dtyp
{

    /// <summary>
    /// The ACCESS_ALLOWED_OBJECT_ACE structure defines an ACE that controls 
    /// allowed access to an object, a property set, or property. The ACE 
    /// contains a set of access rights, a GUID that identifies the type of object, 
    /// and a SID that identifies the trustee to whom the system will grant access. 
    /// The ACE also contains a GUID and a set of flags that control inheritance of 
    /// the ACE by child objects.
    /// </summary>
    public partial struct _ACCESS_ALLOWED_OBJECT_ACE
    {
        /// <summary>
        /// IsObjectTypePresent
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        internal static bool IsObjectTypePresent(MarshalingType marshalingType, object value)
        {
            return (((_ACCESS_ALLOWED_OBJECT_ACE)value).Flags
                & ACCESS_OBJECT_ACE_Flags.ACE_OBJECT_TYPE_PRESENT) != 0;
        }


        /// <summary>
        /// IsInheritedObjectTypePresent
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        internal static bool IsInheritedObjectTypePresent(MarshalingType marshalingType, object value)
        {
            return (((_ACCESS_ALLOWED_OBJECT_ACE)value).Flags
                & ACCESS_OBJECT_ACE_Flags.ACE_INHERITED_OBJECT_TYPE_PRESENT) != 0;
        }
    }


    /// <summary>
    /// The _ACCESS_DENIED_OBJECT_ACE structure defines an ACE that controls 
    /// denied access to an object, a property set, or property. The ACE 
    /// contains a set of access rights, a GUID that identifies the type of object, 
    /// and a SID that identifies the trustee to whom the system will grant access. 
    /// The ACE also contains a GUID and a set of flags that control inheritance of 
    /// the ACE by child objects.
    /// </summary>
    public partial struct _ACCESS_DENIED_OBJECT_ACE
    {
        /// <summary>
        /// IsObjectTypePresent
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        internal static bool IsObjectTypePresent(MarshalingType marshalingType, object value)
        {
            return (((_ACCESS_DENIED_OBJECT_ACE)value).Flags
                & ACCESS_OBJECT_ACE_Flags.ACE_OBJECT_TYPE_PRESENT) != 0;
        }


        /// <summary>
        /// IsInheritedObjectTypePresent
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        internal static bool IsInheritedObjectTypePresent(MarshalingType marshalingType, object value)
        {
            return (((_ACCESS_DENIED_OBJECT_ACE)value).Flags
                & ACCESS_OBJECT_ACE_Flags.ACE_INHERITED_OBJECT_TYPE_PRESENT) != 0;
        }
    }


    /// <summary>
    /// The ACCESS_ALLOWED_CALLBACK_ACE structure defines an ACE for the DACL 
    /// that controls access to an object. An access-allowed ACE allows access 
    /// to an object for a specific trustee identified by a SID.
    /// </summary>
    public partial struct _ACCESS_ALLOWED_CALLBACK_ACE
    {
        /// <summary>
        /// Customized length calculator of ApplicationData, called by Channel.
        /// This method's name is written in ApplicationData's Size attribute. 
        /// </summary>
        /// <param name="context">Channel's context.</param>
        /// <returns>ApplicationData's length.</returns>
        // this method is called by Channel.
        internal static int CalculateApplicationDataLength(IEvaluationContext context)
        {
            _ACE_HEADER header = (_ACE_HEADER)context.Variables["Header"];
            _SID sid = (_SID)context.Variables["Sid"];
            return DtypUtility.CalculateApplicationDataLength(header, sid);
        }
    }


    /// <summary>
    /// The ACCESS_DENIED_CALLBACK_ACE structure defines an ACE for the DACL 
    /// that controls access to an object. An access-denied ACE denies access 
    /// to an object for a specific trustee identified by a SID.
    /// </summary>
    public partial struct _ACCESS_DENIED_CALLBACK_ACE
    {
        /// <summary>
        /// Customized length calculator of ApplicationData, called by Channel.
        /// This method's name is written in ApplicationData's Size attribute. 
        /// </summary>
        /// <param name="context">Channel's context.</param>
        /// <returns>ApplicationData's length.</returns>
        // this method is called by Channel.
        internal static int CalculateApplicationDataLength(IEvaluationContext context)
        {
            _ACE_HEADER header = (_ACE_HEADER)context.Variables["Header"];
            _SID sid = (_SID)context.Variables["Sid"];
            return DtypUtility.CalculateApplicationDataLength(header, sid);
        }
    }


    /// <summary>
    /// The ACCESS_ALLOWED_CALLBACK_OBJECT_ACE structure defines an ACE that 
    /// controls allowed access to an object, property set, or property. 
    /// The ACE contains a set of user rights, a GUID that identifies the type 
    /// of object, and a SID that identifies the trustee to whom the system 
    /// will grant access. The ACE also contains a GUID and a set of flags that 
    /// control inheritance of the ACE by child objects.
    /// </summary>
    public partial struct _ACCESS_ALLOWED_CALLBACK_OBJECT_ACE
    {
        /// <summary>
        /// IsObjectTypePresent
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        internal static bool IsObjectTypePresent(MarshalingType marshalingType, object value)
        {
            return (((_ACCESS_ALLOWED_CALLBACK_OBJECT_ACE)value).Flags
                & ACCESS_OBJECT_ACE_Flags.ACE_OBJECT_TYPE_PRESENT) != 0;
        }


        /// <summary>
        /// IsInheritedObjectTypePresent
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        internal static bool IsInheritedObjectTypePresent(MarshalingType marshalingType, object value)
        {
            return (((_ACCESS_ALLOWED_CALLBACK_OBJECT_ACE)value).Flags
                & ACCESS_OBJECT_ACE_Flags.ACE_INHERITED_OBJECT_TYPE_PRESENT) != 0;
        }


        /// <summary>
        /// Customized length calculator of ApplicationData, called by Channel.
        /// This method's name is written in ApplicationData's Size attribute. 
        /// </summary>
        /// <param name="context">Channel's context.</param>
        /// <returns>ApplicationData's length.</returns>
        // this method is called by Channel.
        internal static int CalculateApplicationDataLength(IEvaluationContext context)
        {
            _ACE_HEADER header = (_ACE_HEADER)context.Variables["Header"];
            _SID sid = (_SID)context.Variables["Sid"];
            return DtypUtility.CalculateApplicationDataLength(header, sid);
        }
    }


    /// <summary>
    /// The ACCESS_DENIED_CALLBACK_OBJECT_ACE structure defines an ACE 
    /// that controls denied access to an object, a property set, or property. 
    /// The ACE contains a set of user rights, a GUID that identifies the 
    /// type of object, and a SID that identifies the trustee to whom the 
    /// system will deny access. The ACE also contains a GUID and a set of 
    /// flags that control inheritance of the ACE by child objects.
    /// </summary>
    public partial struct _ACCESS_DENIED_CALLBACK_OBJECT_ACE
    {
        /// <summary>
        /// IsObjectTypePresent
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        internal static bool IsObjectTypePresent(MarshalingType marshalingType, object value)
        {
            return (((_ACCESS_DENIED_CALLBACK_OBJECT_ACE)value).Flags
                & ACCESS_OBJECT_ACE_Flags.ACE_OBJECT_TYPE_PRESENT) != 0;
        }


        /// <summary>
        /// IsInheritedObjectTypePresent
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        internal static bool IsInheritedObjectTypePresent(MarshalingType marshalingType, object value)
        {
            return (((_ACCESS_DENIED_CALLBACK_OBJECT_ACE)value).Flags
                & ACCESS_OBJECT_ACE_Flags.ACE_INHERITED_OBJECT_TYPE_PRESENT) != 0;
        }


        /// <summary>
        /// Customized length calculator of ApplicationData, called by Channel.
        /// This method's name is written in ApplicationData's Size attribute. 
        /// </summary>
        /// <param name="context">Channel's context.</param>
        /// <returns>ApplicationData's length.</returns>
        // this method is called by Channel.
        internal static int CalculateApplicationDataLength(IEvaluationContext context)
        {
            _ACE_HEADER header = (_ACE_HEADER)context.Variables["Header"];
            _SID sid = (_SID)context.Variables["Sid"];
            return DtypUtility.CalculateApplicationDataLength(header, sid);
        }
    }


    /// <summary>
    /// The SYSTEM_AUDIT_OBJECT_ACE structure defines an access control entry (ACE)
    /// for a system access control list (SACL). The ACE can audit access to an object
    /// or subobjects such as property sets or properties. The ACE contains a set of 
    /// access rights, a GUID that identifies the type of object or subobject, and a 
    /// security identifier (SID) that identifies the trustee for whom the system will 
    /// audit access. The ACE also contains a GUID and a set of flags that control 
    /// inheritance of the ACE by child objects.
    /// </summary>
    public partial struct _SYSTEM_AUDIT_OBJECT_ACE
    {
        /// <summary>
        /// IsObjectTypePresent
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        internal static bool IsObjectTypePresent(MarshalingType marshalingType, object value)
        {
            return (((_SYSTEM_AUDIT_OBJECT_ACE)value).Flags
                & ACCESS_OBJECT_ACE_Flags.ACE_OBJECT_TYPE_PRESENT) != 0;
        }


        /// <summary>
        /// IsInheritedObjectTypePresent
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        internal static bool IsInheritedObjectTypePresent(MarshalingType marshalingType, object value)
        {
            return (((_SYSTEM_AUDIT_OBJECT_ACE)value).Flags
                & ACCESS_OBJECT_ACE_Flags.ACE_INHERITED_OBJECT_TYPE_PRESENT) != 0;
        }
    }


    /// <summary>
    /// The SYSTEM_AUDIT_CALLBACK_ACE structure defines an ACE for the SACL that 
    /// specifies what types of access cause system-level notifications. 
    /// A system-audit ACE causes an audit message to be logged when a specified 
    /// trustee attempts to gain access to an object. The trustee is identified by a SID.
    /// </summary>
    public partial struct _SYSTEM_AUDIT_CALLBACK_ACE
    {
        /// <summary>
        /// Customized length calculator of ApplicationData, called by Channel.
        /// This method's name is written in ApplicationData's Size attribute. 
        /// </summary>
        /// <param name="context">Channel's context.</param>
        /// <returns>ApplicationData's length.</returns>
        // this method is called by Channel.
        internal static int CalculateApplicationDataLength(IEvaluationContext context)
        {
            _ACE_HEADER header = (_ACE_HEADER)context.Variables["Header"];
            _SID sid = (_SID)context.Variables["Sid"];
            return DtypUtility.CalculateApplicationDataLength(header, sid);
        }
    }


    /// <summary>
    /// The SYSTEM_AUDIT_CALLBACK_OBJECT_ACE structure defines an ACE for a SACL. 
    /// The ACE can audit access to an object or subobjects, such as property 
    /// sets or properties. The ACE contains a set of user rights, a GUID that 
    /// identifies the type of object or subobject, and a SID that identifies the 
    /// trustee for whom the system will audit access. The ACE also contains a 
    /// GUID and a set of flags that control inheritance of the ACE by child objects.
    /// </summary>
    public partial struct _SYSTEM_AUDIT_CALLBACK_OBJECT_ACE
    {
        /// <summary>
        /// IsObjectTypePresent
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        internal static bool IsObjectTypePresent(MarshalingType marshalingType, object value)
        {
            return (((_SYSTEM_AUDIT_CALLBACK_OBJECT_ACE)value).Flags
                & ACCESS_OBJECT_ACE_Flags.ACE_OBJECT_TYPE_PRESENT) != 0;
        }


        /// <summary>
        /// IsInheritedObjectTypePresent
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        internal static bool IsInheritedObjectTypePresent(MarshalingType marshalingType, object value)
        {
            return (((_SYSTEM_AUDIT_CALLBACK_OBJECT_ACE)value).Flags
                & ACCESS_OBJECT_ACE_Flags.ACE_INHERITED_OBJECT_TYPE_PRESENT) != 0;
        }


        /// <summary>
        /// Customized length calculator of ApplicationData, called by Channel.
        /// This method's name is written in ApplicationData's Size attribute. 
        /// </summary>
        /// <param name="context">Channel's context.</param>
        /// <returns>ApplicationData's length.</returns>
        // this method is called by Channel.
        internal static int CalculateApplicationDataLength(IEvaluationContext context)
        {
            _ACE_HEADER header = (_ACE_HEADER)context.Variables["Header"];
            _SID sid = (_SID)context.Variables["Sid"];
            return DtypUtility.CalculateApplicationDataLength(header, sid);
        }
    }
}
