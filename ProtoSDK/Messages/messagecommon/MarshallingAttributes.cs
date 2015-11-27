// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using IOP = System.Runtime.InteropServices;
using System.Reflection;

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    #region Attributes

    /// <summary>
    /// An attribute which indicates the length of a runtime value. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    // The following suppression is adopted because these fields are only used by reflection, there is no need to define accessors.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    public sealed class LengthAttribute : Attribute
    {
        // The following suppression is adopted because this field will be used by reflection.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal string expr;

        /// <summary>
        /// Constructs a length attribute by using the given length expression.
        /// </summary>
        /// <param name="expression">The length expression</param>
        public LengthAttribute(string expression)
        {
            this.expr = expression;
        }
    }

    /// <summary>
    /// An attribute which indicates the size of a runtime value. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    // The following suppression is adopted because these fields are only used by reflection, there is no need to define accessors.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    public sealed class SizeAttribute : Attribute
    {
        // The following suppression is adopted because this field will be used by reflection.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal string expr;

        /// <summary>
        /// Constructs a size attribute by using the given size expression.
        /// </summary>
        /// <param name="expression">The size expression.</param>
        public SizeAttribute(string expression)
        {
            this.expr = expression;
        }
    }

    /// <summary>
    /// An attribute which indicates the static size of a runtime value. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    // The following suppression is adopted because these fields are only used by reflection, there is no need to define accessors.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    public sealed class StaticSizeAttribute : Attribute
    {
        // The following suppression is adopted because this field will be used by reflection.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal int size;

        // The following suppression is adopted because this field will be used by reflection.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal StaticSizeMode mode;

        /// <summary>
        /// Constructs a static size attribute by using the given static size.
        /// </summary>
        /// <param name="size">The static size.</param>
        public StaticSizeAttribute(int size)
        {
            this.size = size;
            this.mode = StaticSizeMode.Bytes;
        }

        /// <summary>
        /// Constructs a static size attribute by using given static size and static size mode.
        /// </summary>
        /// <param name="size">The static size.</param>
        /// <param name="mode">The mode for calculating the size.</param>
        public StaticSizeAttribute(int size, StaticSizeMode mode)
        {
            this.size = size;
            this.mode = mode;
        }
    }

    /// <summary>
    /// An enumeration type which defines the mode for evaluating the static size expression.
    /// </summary>
    public enum StaticSizeMode
    {
        /// <summary>
        /// Indicates to evaluate static size of bytes.
        /// </summary>
        Bytes,

        /// <summary>
        /// Indicates to evaluate static size of elements.
        /// </summary>
        Elements
    }


    /// <summary>
    /// An attribute which indicates the switch expression of a runtime value. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    // The following suppression is adopted because these fields are only used by reflection, there is no need to define accessors.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    public sealed class SwitchAttribute : Attribute
    {
        // The following suppression is adopted because this field will be used by reflection.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal string expr;

        /// <summary>
        /// Constructs a switch attribute by using the given switch expression.
        /// </summary>
        /// <param name="expression">The switch expression</param>
        public SwitchAttribute(string expression)
        {
            this.expr = expression;
        }
    }

    /// <summary>
    /// An attribute which indicates the case of a union.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    // The following suppression is adopted because these fields are only used by reflection, there is no need to define accessors.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    public sealed class CaseAttribute : Attribute
    {
        // The following suppression is adopted because this field will be used by reflection.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal string expr;

        /// <summary>
        /// Constructs a case attribute by using the given case expression.
        /// </summary>
        /// <param name="expression">The case expression.</param>
        public CaseAttribute(string expression)
        {
            this.expr = expression;
        }
    }

    /// <summary>
    /// An attribute which indicates the default case of the union. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class CaseDefaultAttribute : Attribute
    {

        /// <summary>
        /// Constructs a case default attribute.
        /// </summary>
        public CaseDefaultAttribute()
        {
        }
    }

    /// <summary>
    /// An attribute which indicates the range expression of a runtime value. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    // The following suppression is adopted because these fields are only used by reflection, there is no need to define accessors.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    public sealed class RangeAttribute : Attribute
    {
        // The following suppression is adopted because this field will be used by reflection.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal string expr;

        /// <summary>
        /// Constructs a range attribute by using the given range expression.
        /// </summary>
        /// <param name="expression">The range expression.</param>
        public RangeAttribute(string expression)
        {
            this.expr = expression;
        }
    }

    /// <summary>
    /// An enum type which specifies the encoding type for string.
    /// </summary>
    public enum StringEncoding
    {
        /// <summary>
        /// Unicode encoding
        /// </summary>
        Unicode,

        /// <summary>
        /// ASCII encoding
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased")]
        ASCII,
    }

    /// <summary>
    /// An attribute which indicates the string property of a runtime value. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    // The following suppression is adopted because these fields are only used by reflection, there is no need to define accessors.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    public sealed class StringAttribute : Attribute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal StringEncoding EncodingType;

        /// <summary>
        /// Constructs a string attribute.
        /// </summary>
        public StringAttribute()
        {
            EncodingType = StringEncoding.Unicode;
        }

        /// <summary>
        /// Constructs a string attribute with specified encoding type.
        /// </summary>
        /// <param name="encodingType">The string encoding type</param>
        public StringAttribute(StringEncoding encodingType)
        {
            EncodingType = encodingType;
        }
    }

    /// <summary>
    /// An attribute which indicates the unique property of a runtime value. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    [Obsolete("HandleAttribute is obsolete")]
    public sealed class UniqueAttribute : Attribute
    {

        /// <summary>
        /// Constructs a unique attribute.
        /// </summary>
        public UniqueAttribute()
        {
        }
    }

    /// <summary>
    /// An attribute which indicates the handle property of a runtime value. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    [Obsolete("HandleAttribute is obsolete")]
    public sealed class HandleAttribute : Attribute
    {

        /// <summary>
        /// Constructs a handle attribute.
        /// </summary>
        public HandleAttribute()
        {
        }
    }

    /// <summary>
    /// An attribute which indicates the union property of a structure value. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
    // The following suppression is adopted because these fields are only used by reflection, there is no need to define accessors.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    public sealed class UnionAttribute : Attribute
    {
        // The following suppression is adopted because this field will be used by reflection.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal string switchTypeExpr;

        /// <summary>
        /// Constructs a union attribute by using the given switch type expression.
        /// </summary>
        /// <param name="switchTypeExpression">The switch type expression.</param>
        public UnionAttribute(string switchTypeExpression)
        {
            this.switchTypeExpr = switchTypeExpression;
        }
    }

    /// <summary>
    /// An attribute which indicates that a value should be marshaled by a pointer to the value. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class IndirectAttribute : Attribute
    {
        /// <summary>
        /// Constructs an indirect attribute.
        /// </summary>
        public IndirectAttribute()
        {
        }
    }

    /// <summary>
    /// An attribute which indicates a customized marshaler. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Struct | AttributeTargets.Class,
                        AllowMultiple = false)]
    // The following suppression is adopted because these fields are only used by reflection, there is no need to define accessors.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    public sealed class CustomMarshalerAttribute : Attribute
    {
        // The following suppression is adopted because this field will be used by reflection.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Type type;

        /// <summary>
        /// Constructs a custom marshaler attribute by using the given type.
        /// </summary>
        /// <param name="type">The type which is the custom marshaler for.</param>
        public CustomMarshalerAttribute(Type type)
        {
            this.type = type;
        }
    }

    /// <summary>
    /// An attribute which associates an identifier binding (type IdentifierBinding)
    /// with a parameter or a field. This attribute allows binding abstraction of values on the wire to integer
    /// identifiers automatically. Entities to which this attribute is attached must have integer type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = false)]
    // The following suppression is adopted because these fields are only used by reflection, there is no need to define accessors.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    public sealed class IdentifierBindingAttribute : Attribute
    {
        // The following suppression is adopted because this field will be used by reflection.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal readonly string Name;

        // The following suppression is adopted because this field will be used by reflection.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal readonly Type TargetType;

        /// <summary>
        /// Constructs an identifier binding attribute.
        /// </summary>
        /// <param name="name">A unique name for the binding. All attributes using the
        /// same name in a given attribute context share the same IdentifierBinding 
        /// instance in a given marshaller. 
        /// </param>
        /// <param name="targetType">The type to which the binding is performed. This is
        /// the type of the value as it appears on the wire.</param>
        public IdentifierBindingAttribute(string name, Type targetType)
        {
            this.Name = name;
            this.TargetType = targetType;
        }
    }

    /// <summary>
    /// An attribute which indicates that a value should be marshaled to an inlined array. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class InlineAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InlineAttribute()
        {
        }
    }

    /// <summary>
    /// An attribute which indicates a value should be marshaled to a bit or a bit array.
    /// The type of the field with bit attribute must be int or int[].
    /// The value of int 0 indicates bit 0, and the value of int 1 indicates bit 1.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class BitAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BitAttribute()
        {
        }
    }

    /// <summary>
    /// An attribute which specifies an method to determine whether a field should be marshaled/unmarshaled.
    /// </summary>
    // The following suppression is adopted because these fields are only used by reflection, there is no need to define accessors.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class MarshalingConditionAttribute : Attribute
    {
        private string methodName;

        /// <summary>
        /// Gets the method name.
        /// </summary>
        public string MethodName
        {
            get
            {
                return methodName;
            }
        }

        /// <summary>
        /// Constructs an instance of MarshalingConditionAttribute object.
        /// </summary>
        /// <param name="methodName">The method name</param>
        public MarshalingConditionAttribute(string methodName)
        {
            this.methodName = methodName;
        }
    }

    /// <summary>
    /// An enumeration type for indicating encoding rules
    /// </summary>
    public enum EncodingRule
    {
        /// <summary>
        /// Indicates a Basic Encoding Rules
        /// </summary>
        Ber,
        /// <summary>
        /// Indicates a Distinguished Encoding Rules
        /// </summary>
        Der,
        /// <summary>
        /// Indicates a Packed Encoding Rules
        /// </summary>
        Per,
    }

    /// <summary>
    /// An attribute used to indicate PER is either aligned or unaligned.
    /// If this attribute is not attached to the asn.1 type, aligned mode will be used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    public sealed class AlignedAttribute : Attribute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal bool aligned;

        /// <summary>
        /// Constructs an instance of AlignedAttribute object.
        /// </summary>
        /// <param name="aligned">The boolean value indicating PER is either aligned or unaligned.</param>
        public AlignedAttribute(bool aligned)
        {
            this.aligned = aligned;
        }
    }

    /// <summary>
    /// An attribute that indicates which ASN.1 encoding rule should be used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    public sealed class Asn1Attribute : Attribute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal EncodingRule rule;

        /// <summary>
        /// Constructs an instance of Asn1Attribute object.
        /// </summary>
        /// <param name="rule">The encoding rule to be used.</param>
        public Asn1Attribute(EncodingRule rule)
        {
            this.rule = rule;
        }
    }

    /// <summary>
    /// Indicates the endian type.
    /// </summary>
    public enum EndianType
    {
        /// <summary>
        /// Indicates the big endian type.
        /// </summary>
        BigEndian,

        /// <summary>
        /// Indicates the little endian type.
        /// </summary>
        LittleEndian,
    }

    /// <summary>
    /// An attribute that indicates the bytes order is little endian or big endian.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    public sealed class ByteOrderAttribute : Attribute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal EndianType endian;

        /// <summary>
        /// Constructs an instance of ByteOrderAttribute object.
        /// </summary>
        /// <param name="endian">Endian type of the byte order.</param>
        public ByteOrderAttribute(EndianType endian)
        {
            this.endian = endian;
        }
    }

    #endregion
}
