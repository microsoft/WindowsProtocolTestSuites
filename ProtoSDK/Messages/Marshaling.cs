// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using IOP = System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Runtime.Marshaling;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    #region Marshaler

    internal class MarshalerHelper
    {
        internal static void ReverseByteOrder(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            for (int i = 0; i < value.Length / 2; i++)
            {
                byte temp = value[i];
                value[i] = value[value.Length - i - 1];
                value[value.Length - i - 1] = temp;
            }
        }

        internal static bool IsBigEndian(MarshalingDescriptor desc)
        {
            EndianType endian;
            if (MarshalingDescriptor.TryGetAttribute<EndianType>(
                desc.Attributes,
                typeof(ByteOrderAttribute),
                "endian",
                out endian))
            {
                if (endian == EndianType.BigEndian)
                    return true;
            }

            return false;
        }

        internal static short AdjustByteOrder(short value, MarshalingDescriptor desc)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (MarshalerHelper.IsBigEndian(desc))
            {
                MarshalerHelper.ReverseByteOrder(bytes);
            }

            return BitConverter.ToInt16(bytes, 0);
        }

        internal static int AdjustByteOrder(int value, MarshalingDescriptor desc)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (MarshalerHelper.IsBigEndian(desc))
            {
                MarshalerHelper.ReverseByteOrder(bytes);
            }

            return BitConverter.ToInt32(bytes, 0);
        }

        internal static long AdjustByteOrder(long value, MarshalingDescriptor desc)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (MarshalerHelper.IsBigEndian(desc))
            {
                MarshalerHelper.ReverseByteOrder(bytes);
            }

            return BitConverter.ToInt64(bytes, 0);
        }

        internal static float AdjustByteOrder(float value, MarshalingDescriptor desc)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (MarshalerHelper.IsBigEndian(desc))
            {
                MarshalerHelper.ReverseByteOrder(bytes);
            }

            return BitConverter.ToSingle(bytes, 0);
        }

        internal static double AdjustByteOrder(double value, MarshalingDescriptor desc)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            if (MarshalerHelper.IsBigEndian(desc))
            {
                MarshalerHelper.ReverseByteOrder(bytes);
            }

            return BitConverter.ToDouble(bytes, 0);
        }

        internal static void CheckNullableField(FieldInfo field, Type type)
        {
            if (field != null
                && field.FieldType.Name == "Nullable`1"
                && field.GetCustomAttributes(typeof(IndirectAttribute), false).Length == 0)
            {
                throw new InvalidOperationException(
                    String.Format(
                    CultureInfo.InvariantCulture,
                    "nullable field without [Indirect] cannot be marshaled: {0}.{1}",
                    type.Name, field.Name
                    ));
            }
        }
    }

    /// <summary>
    /// Marshal and Unmarshal
    /// </summary>
    public enum MarshalingType
    {
        /// <summary>
        /// Indicates marshal the message.
        /// </summary>
        Marshal,

        /// <summary>
        /// Indicates unmarshal the message.
        /// </summary>
        Unmarshal
    }

    /// <summary>
    /// A structure which describes a marshaling descriptor.
    /// It consists of a type and a custom attribute provider.
    /// </summary>
    public struct MarshalingDescriptor
    {
        /// <summary>
        /// The type.
        /// </summary>
        private readonly Type type;

        /// <summary>
        /// Gets the type.
        /// </summary>
        public Type Type
        {
            get { return type; }
        }


        /// <summary>
        /// The custom attribute provider.
        /// </summary>
        private readonly ICustomAttributeProvider attributes;

        /// <summary>
        /// Gets the custom attribute provider.
        /// </summary>
        public ICustomAttributeProvider Attributes
        {
            get { return attributes; }
        }

        /// <summary>
        /// The container type which contains the type in this marshaling descriptor.
        /// </summary>
        private readonly Type containerType;

        /// <summary>
        /// Gets the container type.
        /// </summary>
        public Type ContainerType
        {
            get { return containerType; }
        }

        /// <summary>
        /// Constructs a marshaling descriptor.
        /// </summary>
        /// <param name="type">The type for the marshaling descriptor.</param>
        /// <param name="attributes">The custom attribute provider.</param>
        public MarshalingDescriptor(Type type, ICustomAttributeProvider attributes)
            : this(type, attributes, null)
        {
        }

        /// <summary>
        /// Constructs a marshaling descriptor.
        /// </summary>
        /// <param name="type">The type for the marshaling descriptor.</param>
        /// <param name="attributes">The custom attribute provider.</param>
        /// <param name="containerType">The container type.</param>
        public MarshalingDescriptor(Type type, ICustomAttributeProvider attributes, Type containerType)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.Name == "Nullable`1")
            {
                IsNullable = true;
                type = type.GetGenericArguments()[0];
            }
            else
            {
                IsNullable = false;
            }
            this.type = type;
            this.attributes = attributes;
            this.containerType = containerType;
        }

        /// <summary>
        /// Constructs a marshaling descriptor by using the EmptyAttributeProvider.
        /// </summary>
        /// <param name="type">The type for the marshaling descriptor.</param>
        public MarshalingDescriptor(Type type)
            : this(type, emptyAttributeProvider, null)
        {
        }

        /// <summary>
        /// Gets a readable representation of the marshaling descriptor.
        /// </summary>
        /// <returns>Returns readable representation of the marshaling descriptor as string.</returns>
        public override string ToString()
        {
            if (Attributes is ParameterInfo)
                return string.Format(CultureInfo.InvariantCulture, "parameter '{0}', type '{1}'", Attributes, Type);
            else if (Attributes is FieldInfo)
                return string.Format(CultureInfo.InvariantCulture, "field '{0}', type '{1}'", Attributes, Type);
            else
                return string.Format(CultureInfo.InvariantCulture, "type '{0}'", Type);
        }

        #region Dealing with attributes

        /// <summary>
        /// A helper function for checking whether an attribute is defined.
        /// </summary>
        /// <param name="provider">The custom attribute provider.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <returns>Returns true if the type has the attribute.</returns>
        public static bool HasAttribute(ICustomAttributeProvider provider, Type expectedType)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            object[] attrs = provider.GetCustomAttributes(expectedType, false);
            return attrs.Length > 0;
        }

        /// <summary>
        /// A helper function for checking whether an attribute is defined and delivers a field value if so.
        /// </summary>
        /// <typeparam name="T">Type template for field value</typeparam>
        /// <param name="provider">The customer attribute provider.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="fieldValue">The field value.</param>
        /// <returns>Returns true if gets the field value successfully.</returns>
        public static bool TryGetAttribute<T>(
            ICustomAttributeProvider provider,
            Type expectedType,
            string fieldName,
            out T fieldValue)
        {
            fieldValue = default(T);
            object[] attrs = provider.GetCustomAttributes(expectedType, false);
            if (attrs.Length > 0)
            {
                FieldInfo field = expectedType.GetField(fieldName,
                                                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                object value = field.GetValue(attrs[0]);
                if (!(value is T))
                    return false;
                fieldValue = (T)value;
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion


        #region Empty attribute provider

        internal static readonly ICustomAttributeProvider emptyAttributeProvider =
             new EmptyAttributeProvider();


        class EmptyAttributeProvider : ICustomAttributeProvider
        {
            public object[] GetCustomAttributes(bool inherit)
            {
                return new object[0];
            }

            public object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                return new object[0];
            }

            public bool IsDefined(Type attributeType, bool inherit)
            {
                return false;
            }

        }

        #endregion

        /// <summary>
        /// Indicates the type is nullable.
        /// </summary>
        internal bool IsNullable;
    }

    internal class CustomAttributeFilter : ICustomAttributeProvider
    {
        ICustomAttributeProvider provider;

        List<Type> expectedAttributes = new List<Type>();

        internal CustomAttributeFilter(ICustomAttributeProvider provider, params Type[] expectedAttributes)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            this.provider = provider;

            if (expectedAttributes != null)
            {
                this.expectedAttributes.AddRange(expectedAttributes);
            }
        }

        #region ICustomAttributeProvider Members

        public object[] GetCustomAttributes(bool inherit)
        {
            object[] attrs = provider.GetCustomAttributes(inherit);

            return FilterAttributes(attrs);
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            object[] attrs = provider.GetCustomAttributes(attributeType, inherit);

            return FilterAttributes(attrs);
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            bool result = provider.IsDefined(attributeType, inherit);

            if (result && !expectedAttributes.Contains(attributeType))
            {
                result = false;
            }

            return result;
        }

        private object[] FilterAttributes(object[] attrs)
        {
            if (attrs != null)
            {
                List<object> attrsList = new List<object>();
                foreach (object attr in attrs)
                {
                    if (expectedAttributes.Contains(attr.GetType()))
                    {
                        attrsList.Add(attr);
                    }
                }
                attrs = attrsList.ToArray();
            }

            return attrs;
        }

        internal static CustomAttributeFilter GetArrayElementFilter(ICustomAttributeProvider provider)
        {
            return new CustomAttributeFilter(provider, typeof(SwitchAttribute), typeof(ByteOrderAttribute));
        }

        internal static CustomAttributeFilter GetEnumElementFilter(ICustomAttributeProvider provider)
        {
            return new CustomAttributeFilter(provider, typeof(ByteOrderAttribute));
        }

        #endregion
    }

    /// <summary>
    /// An interface which describes a type marshaler.
    /// </summary>
    public interface ITypeMarshaler
    {
        /// <summary>
        /// Gets the size of the marshaled value in the native environment. 
        /// If the size depends on the value and the value is unknown (null), 
        /// the marshaler should return negative value to indicate this.
        /// </summary>
        /// <param name="marshaler">The marshaler.</param>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <param name="value">The marshaled value.</param>
        /// <returns>The size of the marshaled value.</returns>
        int GetSize(Marshaler marshaler, MarshalingDescriptor descriptor, object value);

        /// <summary>
        /// Gets the alignment of the marshaled value in the native environment.
        /// </summary>
        /// <param name="marshaler">The marshaler.</param>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <returns>The alignment</returns>
        int GetAlignment(Marshaler marshaler, MarshalingDescriptor descriptor);

        /// <summary>
        /// Gets the native type representation, if any.
        /// </summary>
        /// <param name="marshaler">The marshaler.</param>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <returns>The native type representation.</returns>
        Type GetNativeType(Marshaler marshaler, MarshalingDescriptor descriptor);

        /// <summary>
        /// Marshals the given value into its native representation.
        /// </summary>
        /// <param name="marshaler">The marshaler.</param>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <param name="value">The value to be marshaled.</param>
        void Marshal(Marshaler marshaler, MarshalingDescriptor descriptor, object value);

        /// <summary>
        /// Unmarshals a value from the native representation.
        /// </summary>
        /// <param name="marshaler">The marshaler.</param>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <returns>The unmarshaled value.</returns>
        object Unmarshal(Marshaler marshaler, MarshalingDescriptor descriptor);

    }

    /// <summary>
    /// An interface which describes a generic type marshaler. 
    /// It can be instantiated for given type arguments.
    /// </summary>
    public interface IGenericTypeMarshaler
    {
        /// <summary>
        /// Instantiates the generic marshaler and delivers a type marshaler.
        /// </summary>
        /// <param name="baseType">The instance type for the generic type marshaler.</param>
        /// <param name="elementMarshalers">The array which contains the type marshalers for the given generic type.</param>
        /// <returns>The type marshaler.</returns>
        ITypeMarshaler Instantiate(Type baseType, params ITypeMarshaler[] elementMarshalers);
    }

    /// <summary>
    /// An interface which represents a contiguous region of bytes.
    /// This interface defines a set of operations to read and write in these bytes.
    /// </summary>
    public interface IRegion
    {
        /// <summary>
        /// Writes a 8 bit value.
        /// </summary>
        /// <param name="value">The value to be written</param>
        void WriteByte(byte value);

        /// <summary>
        /// Writes a 16 bit value.
        /// </summary>
        /// <param name="value">The value to be written</param>
        void WriteInt16(short value);

        /// <summary>
        /// Writes a 32 bit value.
        /// </summary>
        /// <param name="value">The value to be written</param>
        void WriteInt32(int value);

        /// <summary>
        /// Writes a 64 bit value.
        /// </summary>
        /// <param name="value">The value to be written</param>
        void WriteInt64(long value);

        /// <summary>
        /// Writes a 32/64 bit value.
        /// </summary>
        /// <param name="value">The value to be written</param>
        void WriteIntPtr(IntPtr value);


        /// <summary>
        /// Reads a 8 bit value.
        /// </summary>
        /// <returns>The value</returns>
        byte ReadByte();

        /// <summary>
        /// Reads a 16 bit value.
        /// </summary>
        /// <returns>The value</returns>
        short ReadInt16();

        /// <summary>
        /// Reads a 32 bit value.
        /// </summary>
        /// <returns>The value</returns>
        int ReadInt32();

        /// <summary>
        /// Reads a 64 bit value.
        /// </summary>
        /// <returns>The value</returns>
        long ReadInt64();

        /// <summary>
        /// Reads a 32/64 bit value.
        /// </summary>
        /// <returns>The value</returns>
        IntPtr ReadIntPtr();

        /// <summary>
        /// Gets the number of bytes left in the region, or -1 if that is unknown.
        /// </summary>
        int SpaceLeft { get; }

        /// <summary>
        /// Gets the number of bytes written/read so far to the region.
        /// </summary>
        int Offset { get; set; }

        /// <summary>
        /// Gets the native memory which is associated with this region, or IntPtr.Zero, if it has no associated native memory.
        /// </summary>
        IntPtr NativeMemory { get; }

        /// <summary>
        /// Resets the region to its initial offset. 
        /// This method may not be supported by all regions. It returns true on success only.
        /// </summary>
        /// <returns>Returns true if it resets successfully.</returns>
        bool TryReset();

        /// <summary>
        /// Indicates whether check left space when do marshaling/unmarshaling
        /// </summary>
        bool UseSpaceChecking { get; set; }
    }

    /// <summary>
    /// A class which represents a marshaling configuration.
    /// </summary>
    public abstract class MarshalingConfiguration
    {
        Dictionary<Type, IGenericTypeMarshaler> genericMarshalers =
           new Dictionary<Type, IGenericTypeMarshaler>();

        Dictionary<Type, IGenericTypeMarshaler> genericMarshalersInline =
           new Dictionary<Type, IGenericTypeMarshaler>();

        Dictionary<Type, ITypeMarshaler> marshalers =
            new Dictionary<Type, ITypeMarshaler>();

        IGenericTypeMarshaler arrayMarshaler;
        IGenericTypeMarshaler arrayMarshalerInline;
        IGenericTypeMarshaler indirectMarshaler;
        ITypeMarshaler unionMarshaler;
        ITypeMarshaler structMarshaler;
        ITypeMarshaler bitMarshaler;

        ITypeMarshaler stringMarshaler;
        ITypeMarshaler stringMarshalerInline;
        ITypeMarshaler stringASCIIMarshaler;
        ITypeMarshaler stringASCIIMarshalerInline;

        Dictionary<Type, ITypeMarshaler> derivedMarshalers =
            new Dictionary<Type, ITypeMarshaler>();

        Dictionary<Type, ITypeMarshaler> derivedIndirectMarshalers =
            new Dictionary<Type, ITypeMarshaler>();

        Dictionary<string, ITypeMarshaler> derivedBindingMarshalers =
            new Dictionary<string, ITypeMarshaler>();
        Dictionary<string, Type> bindingTypes =
            new Dictionary<string, Type>();

        Dictionary<EncodingRule, ITypeMarshaler> asn1Marshalers =
            new Dictionary<EncodingRule, ITypeMarshaler>();

        #region Marshaler registration

        /// <summary>
        /// Registers a marshaler for the given type.
        /// </summary>
        /// <param name="type">The type to be marshaled.</param>
        /// <param name="typeMarshaler">The type marshaler corresponding to the given type.</param>
        public void RegisterMarshaler(Type type, ITypeMarshaler typeMarshaler)
        {
            marshalers[type] = typeMarshaler;
            ClearCache();
        }

        /// <summary>
        /// Registers a generic marshaler for the given generic type.
        /// </summary>
        /// <param name="genericType">The generic type to be marshaled.</param>
        /// <param name="genericTypeMarshaler">The marshaler corresponding to the given generic type.</param>
        public void RegisterGenericMarshaler(Type genericType, IGenericTypeMarshaler genericTypeMarshaler)
        {
            genericMarshalers[genericType] = genericTypeMarshaler;
            ClearCache();
        }

        /// <summary>
        /// Registers an inline generic marshaler for the given generic type.
        /// </summary>
        /// <param name="genericType">The generic type to be marshaled.</param>
        /// <param name="genericTypeMarshaler">The marshaler corresponding to the given generic type.</param>
        public void RegisterGenericMarshalerInline(Type genericType, IGenericTypeMarshaler genericTypeMarshaler)
        {
            genericMarshalersInline[genericType] = genericTypeMarshaler;
            ClearCache();
        }

        /// <summary>
        /// Registers a generic marshaler for array types.
        /// </summary>
        /// <param name="genericTypeMarshaler">The marshaler for the generic type.</param>
        public void RegisterArrayMarshaler(IGenericTypeMarshaler genericTypeMarshaler)
        {
            arrayMarshaler = genericTypeMarshaler;
            ClearCache();
        }

        /// <summary>
        /// Registers an inline generic marshaler for array types.
        /// </summary>
        /// <param name="genericTypeMarshaler">The marshaler for the generic type.</param>
        public void RegisterArrayMarshalerInline(IGenericTypeMarshaler genericTypeMarshaler)
        {
            arrayMarshalerInline = genericTypeMarshaler;
            ClearCache();
        }

        /// <summary>
        /// Registers a generic marshaler for the elements with Indirect attribute.
        /// </summary>
        /// <param name="genericTypeMarshaler">The marshaler for the generic type.</param>
        public void RegisterIndirectMarshaler(IGenericTypeMarshaler genericTypeMarshaler)
        {
            indirectMarshaler = genericTypeMarshaler;
            ClearCache();
        }

        /// <summary>
        /// Registers a struct marshaler.
        /// </summary>
        /// <param name="typeMarshaler">The type marshaler.</param>
        public void RegisterStructMarshaler(ITypeMarshaler typeMarshaler)
        {
            structMarshaler = typeMarshaler;
            ClearCache();
        }

        /// <summary>
        /// Registers an array marshaler.
        /// </summary>
        /// <param name="typeMarshaler">The type marshaler.</param>
        public void RegisterUnionMarshaler(ITypeMarshaler typeMarshaler)
        {
            unionMarshaler = typeMarshaler;
            ClearCache();
        }

        /// <summary>
        /// Registers a bit marshaler.
        /// </summary>
        /// <param name="typeMarshaler">The type marshaler</param>
        public void RegisterBitMarshaler(ITypeMarshaler typeMarshaler)
        {
            bitMarshaler = typeMarshaler;
            ClearCache();
        }

        /// <summary>
        /// Registers an ASN1 marshaler
        /// </summary>
        /// <param name="rule">The encoding rule</param>
        /// <param name="typeMarshaler">The type marshaler</param>
        public void RegisterAsn1Marshaler(EncodingRule rule, ITypeMarshaler typeMarshaler)
        {
            asn1Marshalers[rule] = typeMarshaler;
            ClearCache();
        }

        /// <summary>
        /// Registers a string marshaler
        /// </summary>
        /// <param name="typeMarshaler">The type marshaler</param>
        public void RegisterStringMarshaler(ITypeMarshaler typeMarshaler)
        {
            stringMarshaler = typeMarshaler;
            ClearCache();
        }

        /// <summary>
        /// Registers an inline string marshaler
        /// </summary>
        /// <param name="typeMarshaler">The type marshaler</param>
        public void RegisterStringMarshalerInline(ITypeMarshaler typeMarshaler)
        {
            stringMarshalerInline = typeMarshaler;
            ClearCache();
        }

        /// <summary>
        /// Registers an ASCII string marshaler
        /// </summary>
        /// <param name="typeMarshaler">The type marshaler</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased")]
        public void RegisterStringASCIIMarshaler(ITypeMarshaler typeMarshaler)
        {
            stringASCIIMarshaler = typeMarshaler;
            ClearCache();
        }

        /// <summary>
        /// Registers an inline ASCII string marshaler
        /// </summary>
        /// <param name="typeMarshaler">The type marshaler</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased")]
        public void RegisterStringASCIIMarshalerInline(ITypeMarshaler typeMarshaler)
        {
            stringASCIIMarshalerInline = typeMarshaler;
            ClearCache();
        }

        void ClearCache()
        {
            derivedMarshalers.Clear();
            derivedIndirectMarshalers.Clear();
        }

        #endregion

        #region Lookup marshalers

        /// <summary>
        /// Looks up marshaler for given marshaling descriptor.
        /// </summary>
        /// <param name="marshaler">The expected marshaler.</param>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <returns>The type marshaler.</returns>
        public ITypeMarshaler GetMarshaler(Marshaler marshaler, MarshalingDescriptor descriptor)
        {
            return GetMarshaler(marshaler, descriptor, true, true);
        }

        /// <summary>
        /// Looks up marshaler for given marshaling descriptor, with or without processing indirect attribute and identifier binding attribute. 
        /// </summary>
        /// <param name="marshaler">The expected marshaler.</param>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <param name="processIndirect">Indicates if processing Indirect attribute.</param>
        /// <param name="processBinding">Indicates if processing IdentifierBinding attribute.</param>
        /// <returns>The type marshaler.</returns>
        /// <remarks>Instead of overriding this method, considers registering your marshalers in the constructor
        /// of a derived class.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        protected virtual ITypeMarshaler GetMarshaler(Marshaler marshaler, MarshalingDescriptor descriptor,
                                                      bool processIndirect, bool processBinding)
        {
            // check [Indirect] marshaler
            ITypeMarshaler msr;
            if (processIndirect && descriptor.Attributes.IsDefined(typeof(IndirectAttribute), false))
            {
                if (!this.derivedIndirectMarshalers.TryGetValue(descriptor.Type, out msr))
                {
                    msr = GetMarshaler(marshaler, descriptor, false, processBinding);
                    msr = indirectMarshaler.Instantiate(descriptor.Type, msr);
                    derivedIndirectMarshalers[descriptor.Type] = msr;
                }
                return msr;
            }

            // check [IdentifierBinding] marshaler
            if (processBinding && descriptor.Attributes.IsDefined(typeof(IdentifierBindingAttribute), false))
            {
                string name;
                Type targetType;
                if (!MarshalingDescriptor.TryGetAttribute<string>(descriptor.Attributes,
                                                    typeof(IdentifierBindingAttribute), "Name",
                            out name))
                    throw new InvalidOperationException("unexpected inconsistent attribute: " + descriptor);
                if (!MarshalingDescriptor.TryGetAttribute<Type>(descriptor.Attributes,
                                                    typeof(IdentifierBindingAttribute), "TargetType",
                            out targetType))
                    throw new InvalidOperationException("unexpected inconsistent attribute: " + descriptor);
                if (!derivedBindingMarshalers.TryGetValue(name, out msr))
                {

                    bindingTypes[name] = targetType;
                    msr = GetMarshaler(marshaler, descriptor, processIndirect, false);
                    derivedBindingMarshalers[name] = msr = new BindingMarshaler(name, msr);
                }
                if (descriptor.Type != typeof(int))
                    marshaler.TestAssumeFail(descriptor, "identifier binding '{0}': source type must be 'int'", name);
                if (targetType != bindingTypes[name])
                    marshaler.TestAssumeFail(descriptor, "identifier binding '{0}': previous target type '{1}' mismatch",
                                                    name, bindingTypes[name]);
                return msr;
            }

            // try [CustomMarshaler] attribute
            object ctypeObj;
            if (MarshalingDescriptor.TryGetAttribute(descriptor.Attributes, typeof(CustomMarshalerAttribute), "type", out ctypeObj) ||
                MarshalingDescriptor.TryGetAttribute(descriptor.Type, typeof(CustomMarshalerAttribute), "type", out ctypeObj))
            {
                Type ctype = (Type)ctypeObj;
                if (!typeof(ITypeMarshaler).IsAssignableFrom(ctype))
                    marshaler.TestAssumeFail(descriptor,
                                              "custom marshaler '{0}' does not implement 'ITypeMarshaler'",
                                              ctype.FullName);
                msr = Activator.CreateInstance(ctype) as ITypeMarshaler;
                return msr;
            }

            // try [Asn1BER], [Asn1DER] and [Asn1PER] Asn.1 marshalers
            if (TryGetAsn1Marshaler(marshaler, descriptor, out msr))
            {
                return msr;
            }

            // try bit marshaler
            if (descriptor.Attributes.IsDefined(typeof(BitAttribute), false))
            {
                if ((descriptor.Type.IsArray && descriptor.Type.GetElementType() == typeof(int))
                    || descriptor.Type == typeof(int))
                {
                    if (bitMarshaler == null)
                    {
                        marshaler.TestAssumeFail(descriptor,
                                              "current marshaling configuration does not support marshaling the type with [Bit] attribute");
                    }

                    msr = bitMarshaler;
                    return msr;
                }
                else
                {
                    marshaler.TestAssumeFail(descriptor,
                                              "the type with [Bit] attribute must be int or int[].");
                }
            }

            // try array marshaler
            if (descriptor.Type.IsArray)
            {
                if (descriptor.Attributes.IsDefined(typeof(InlineAttribute), false))
                {
                    if (arrayMarshalerInline == null)
                    {
                        marshaler.TestAssumeFail(descriptor,
                                              "current marshaling configuration does not support marshaling array with [Inline] attribute");
                    }

                    msr = arrayMarshalerInline.Instantiate(descriptor.Type,
                                GetMarshaler(marshaler, new MarshalingDescriptor(descriptor.Type.GetElementType())));
                }
                else
                {
                    msr = arrayMarshaler.Instantiate(descriptor.Type,
                                GetMarshaler(marshaler, new MarshalingDescriptor(descriptor.Type.GetElementType())));
                }

                return msr;
            }

            // try string marshalers
            if (descriptor.Type == typeof(string))
            {
                StringEncoding encodingType;
                if (!MarshalingDescriptor.TryGetAttribute<StringEncoding>(
                    descriptor.Attributes, typeof(StringAttribute), "EncodingType", out encodingType))
                {
                    encodingType = StringEncoding.Unicode;
                }

                if (descriptor.Attributes.IsDefined(typeof(InlineAttribute), false))
                {
                    msr = encodingType == StringEncoding.Unicode
                        ? stringMarshalerInline : stringASCIIMarshalerInline;
                }
                else
                {
                    msr = encodingType == StringEncoding.Unicode
                        ? stringMarshaler : stringASCIIMarshaler;
                }

                if (msr == null)
                    marshaler.TestAssumeFail(descriptor,
                        "current marshaling configuration does not support marshaling string with [Inline] attribute");
                return msr;
            }

            // try generic marshalers
            IGenericTypeMarshaler gmsr;
            if (descriptor.Type.IsGenericType)
            {
                bool hasGenericMarshaler;
                if (descriptor.Attributes.IsDefined(typeof(InlineAttribute), false))
                {
                    hasGenericMarshaler =
                        genericMarshalersInline.TryGetValue(descriptor.Type.GetGenericTypeDefinition(), out gmsr);

                    if (!hasGenericMarshaler)
                    {
                        marshaler.TestAssumeFail(descriptor,
                                              "current marshaling configuration does not support marshaling the generic type with [Inline] attribute");
                    }
                }
                else
                {
                    hasGenericMarshaler =
                        genericMarshalers.TryGetValue(descriptor.Type.GetGenericTypeDefinition(), out gmsr);
                }

                if (hasGenericMarshaler)
                {
                    Type[] genericArgs = descriptor.Type.GetGenericArguments();
                    ITypeMarshaler[] argMsrs = new ITypeMarshaler[genericArgs.Length];
                    for (int i = 0; i < argMsrs.Length; i++)
                    {
                        argMsrs[i] = GetMarshaler(marshaler, new MarshalingDescriptor(genericArgs[i]));
                    }
                    msr = gmsr.Instantiate(descriptor.Type, argMsrs);
                    derivedMarshalers[descriptor.Type] = msr;
                    return msr;
                }
            }


            // try registered marshalers or derived marshalers
            if (marshalers.TryGetValue(descriptor.Type, out msr) ||
                derivedMarshalers.TryGetValue(descriptor.Type, out msr))
            {
                return msr;
            }

            // finally, try struct and union marshalers
            if (descriptor.Type.IsValueType && !descriptor.Type.IsPrimitive)
            {
                if (MarshalingDescriptor.HasAttribute(descriptor.Type, typeof(UnionAttribute)))
                    msr = unionMarshaler;
                else
                    msr = structMarshaler;
                derivedMarshalers[descriptor.Type] = msr;
                return msr;
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "cannot find marshaler for type '{0}'", descriptor.Type));
        }

        private bool TryGetAsn1Marshaler(Marshaler marshaler,
                                MarshalingDescriptor descriptor,
                                out ITypeMarshaler typeMarshaler)
        {
            typeMarshaler = null;
            EncodingRule rule;
            if (MarshalingDescriptor.TryGetAttribute<EncodingRule>(descriptor.Attributes, typeof(Asn1Attribute), "rule", out rule)
                || MarshalingDescriptor.TryGetAttribute<EncodingRule>(descriptor.Type, typeof(Asn1Attribute), "rule", out rule))
            {
                if (asn1Marshalers[rule] != null)
                {
                    typeMarshaler = asn1Marshalers[rule];
                }
                else
                {
                    marshaler.TestAssumeFail(
                            descriptor,
                            "current marshaling configuration does not support marshaling asn.1 type with encoding rule '{0}'",
                            rule.ToString());
                }

                return true;
            }

            return false;
        }

        #endregion

        public int Alignment
        {
            get;
            set;
        }

        public int IntPtrSize
        {
            get;
            set;
        }
    }

    /// <summary>
    /// A marshaling configuration which works for native code bindings, like RPC calls. 
    /// </summary>
    public class NativeMarshalingConfiguration : MarshalingConfiguration
    {
        /// <summary>
        /// Constructs the native marshaling configuration.
        /// </summary>
        protected NativeMarshalingConfiguration()
        {
            RegisterMarshaler(typeof(byte), new ByteMarshaler());
            RegisterMarshaler(typeof(sbyte), new ByteMarshaler());
            RegisterMarshaler(typeof(short), new Int16Marshaler());
            RegisterMarshaler(typeof(ushort), new Int16Marshaler());
            RegisterMarshaler(typeof(int), new Int32Marshaler());
            RegisterMarshaler(typeof(uint), new Int32Marshaler());
            RegisterMarshaler(typeof(long), new Int64Marshaler());
            RegisterMarshaler(typeof(ulong), new Int64Marshaler());
            RegisterMarshaler(typeof(IntPtr), new IntPtrMarshaler());
            RegisterMarshaler(typeof(float), new SingleMarshaler());
            RegisterMarshaler(typeof(double), new DoubleMarshaler());
            RegisterMarshaler(typeof(bool), new BooleanMarshaler());
            RegisterMarshaler(typeof(char), new CharMarshaler());
            RegisterMarshaler(typeof(Guid), new GuidMarshaler());

            RegisterStringMarshaler(new StringUTF16Marshaler());
            RegisterStringMarshalerInline(new StringUTF16MarshalerInline());

            RegisterStringASCIIMarshaler(new StringASCIIMarshaler());
            RegisterStringASCIIMarshalerInline(new StringASCIIMarshalerInline());

            RegisterStructMarshaler(new StructMarshaler());
            RegisterUnionMarshaler(new UnionMarshaler());

            RegisterArrayMarshaler(new GenericArrayMarshaler());
            RegisterArrayMarshalerInline(new GenericArrayMarshalerInline());

            RegisterIndirectMarshaler(new GenericIndirectMarshaler());

            Alignment = IntPtr.Size;
            IntPtrSize = IntPtr.Size;
        }

        /// <summary>
        /// Gets the default native marshaling configuration
        /// </summary>
        public static MarshalingConfiguration Configuration
        {
            get
            {
                //if (configuration == null)
                //{
                //    configuration = new NativeMarshalingConfiguration();
                //}
                //return configuration;
                return new NativeMarshalingConfiguration();
            }
        }
    }

    /// <summary>
    /// A marshaling configuration which works for block protocols that exchange C-like structs. 
    /// </summary>
    public class BlockMarshalingConfiguration : MarshalingConfiguration
    {
        /// <summary>
        /// Constructs the block marshaling configuration.
        /// </summary>
        protected BlockMarshalingConfiguration()
        {
            RegisterMarshaler(typeof(byte), new ByteMarshaler());
            RegisterMarshaler(typeof(sbyte), new ByteMarshaler());
            RegisterMarshaler(typeof(short), new Int16Marshaler());
            RegisterMarshaler(typeof(ushort), new Int16Marshaler());
            RegisterMarshaler(typeof(int), new Int32Marshaler());
            RegisterMarshaler(typeof(uint), new Int32Marshaler());
            RegisterMarshaler(typeof(long), new Int64Marshaler());
            RegisterMarshaler(typeof(ulong), new Int64Marshaler());
            RegisterMarshaler(typeof(IntPtr), new IntPtrMarshaler());
            RegisterMarshaler(typeof(float), new SingleMarshaler());
            RegisterMarshaler(typeof(double), new DoubleMarshaler());
            RegisterMarshaler(typeof(bool), new BooleanMarshaler());
            RegisterMarshaler(typeof(char), new CharMarshaler());

            RegisterMarshaler(typeof(Guid), new GuidMarshaler());
            RegisterStringMarshaler(new StringUTF16MarshalerInline());
            RegisterStringASCIIMarshaler(new StringASCIIMarshalerInline());

            RegisterStructMarshaler(new StructMarshalerInline());

            RegisterArrayMarshaler(new GenericArrayMarshalerInline());
            RegisterArrayMarshalerInline(new GenericArrayMarshalerInline());

            RegisterBitMarshaler(new BitMarshaler());

            RegisterAsn1Marshaler(EncodingRule.Ber, new Asn1BerMarshaler());
            RegisterAsn1Marshaler(EncodingRule.Der, new Asn1DerMarshaler());
            RegisterAsn1Marshaler(EncodingRule.Per, new Asn1PerMarshaler());

            Alignment = IntPtr.Size;
            IntPtrSize = IntPtr.Size;
        }

        /// <summary>
        /// Gets the default block marshaling configuration
        /// </summary>
        public static MarshalingConfiguration Configuration
        {
            get
            {
                //if (configuration == null)
                //{
                //    configuration = new BlockMarshalingConfiguration();
                //}
                //return configuration;
                return new BlockMarshalingConfiguration();
            }
        }
    }

    #endregion

    #region Primitive Type Marshalers

    internal class ByteMarshaler : ITypeMarshaler
    {
        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            return 1;
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return 1;
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return typeof(byte);
        }

        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            if (value is sbyte)
                marshaler.WriteByte((byte)(sbyte)value);
            else if (value is byte)
                marshaler.WriteByte((byte)value);
            else
                throw new InvalidCastException();

        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            byte r = marshaler.ReadByte();
            if (desc.Type == typeof(byte))
                return r;
            else if (desc.Type == typeof(sbyte))
                return (sbyte)r;
            else
                throw new InvalidCastException();
        }
    }

    internal class Int16Marshaler : ITypeMarshaler
    {
        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            return 2;
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return 2;
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return typeof(short);
        }

        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            short v;
            if (value is short)
                v = MarshalerHelper.AdjustByteOrder((short)value, desc);
            else if (value is ushort)
                v = MarshalerHelper.AdjustByteOrder((short)(ushort)value, desc);
            else
                throw new InvalidCastException();

            marshaler.WriteInt16(v);
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            short r = marshaler.ReadInt16();

            r = MarshalerHelper.AdjustByteOrder(r, desc);

            if (desc.Type == typeof(short))
                return r;
            else if (desc.Type == typeof(ushort))
                return (ushort)r;
            else
                throw new InvalidCastException();
        }
    }

    internal class Int32Marshaler : ITypeMarshaler
    {

        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            return 4;
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return 4;
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return typeof(int);
        }


        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            int v;
            if (value is int)
                v = MarshalerHelper.AdjustByteOrder((int)value, desc);
            else if (value is uint)
                v = MarshalerHelper.AdjustByteOrder((int)(uint)value, desc);
            else
                throw new InvalidCastException();

            marshaler.WriteInt32(v);
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            int r = marshaler.ReadInt32();

            r = MarshalerHelper.AdjustByteOrder(r, desc);

            if (desc.Type == typeof(int))
                return r;
            else if (desc.Type == typeof(uint))
                return (uint)r;
            else
                throw new InvalidCastException();
        }
    }

    internal class Int64Marshaler : ITypeMarshaler
    {

        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            return 8;
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return 8;
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return typeof(long);
        }


        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            long v;
            if (value is long)
                v = MarshalerHelper.AdjustByteOrder((long)value, desc);
            else if (value is ulong)
                v = MarshalerHelper.AdjustByteOrder((long)(ulong)value, desc);
            else
                throw new InvalidCastException();

            marshaler.WriteInt64(v);
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            long r = marshaler.ReadInt64();

            r = MarshalerHelper.AdjustByteOrder(r, desc);

            if (desc.Type == typeof(long))
                return r;
            else if (desc.Type == typeof(ulong))
                return (ulong)r;
            else
                throw new InvalidCastException();
        }
    }

    internal class IntPtrMarshaler : ITypeMarshaler
    {

        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            return IntPtr.Size;
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return IntPtr.Size;
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return typeof(IntPtr);
        }


        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            marshaler.WriteIntPtr((IntPtr)value);
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return marshaler.ReadIntPtr();
        }
    }

    internal class SingleMarshaler : ITypeMarshaler
    {
        private const int SIZE = 4;
        private const int ALIGNMENT = 4;

        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            return SIZE;
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return ALIGNMENT;
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return typeof(float);
        }

        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            float v = MarshalerHelper.AdjustByteOrder((float)value, desc);
            marshaler.WriteStructure(v, SIZE);
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            float v = (float)marshaler.ReadStructure(typeof(float), SIZE);
            return MarshalerHelper.AdjustByteOrder(v, desc);
        }
    }

    internal class DoubleMarshaler : ITypeMarshaler
    {
        private const int SIZE = 8;
        private const int ALIGNMENT = 8;

        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            return SIZE;
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return ALIGNMENT;
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return typeof(double);
        }

        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            double v = MarshalerHelper.AdjustByteOrder((double)value, desc);
            marshaler.WriteStructure(v, SIZE);
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            double v = (double)marshaler.ReadStructure(typeof(double), SIZE);
            return MarshalerHelper.AdjustByteOrder(v, desc);
        }
    }

    internal class BooleanMarshaler : ITypeMarshaler
    {
        private const int SIZE = 4;
        private const int ALIGNMENT = 4;

        private int GetSize(MarshalingDescriptor desc, int defaultValue)
        {
            int size;
            if (!MarshalingDescriptor.TryGetAttribute<int>(
                desc.Attributes, typeof(StaticSizeAttribute), "size", out size))
            {
                size = defaultValue;
            }
            if (size != SIZE && size != 1)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "Invalid static size '{0}' for bool in structure '{1}'", size, desc.Type.Name));
            }
            return size;
        }

        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            return GetSize(desc, SIZE);
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return GetSize(desc, ALIGNMENT);
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return typeof(bool);
        }

        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            int size = GetSize(desc, SIZE);
            if (size == SIZE)
            {
                int v = Convert.ToInt32((bool)value);
                v = MarshalerHelper.AdjustByteOrder(v, desc);
                marshaler.WriteInt32(v);
            }
            else //if (size == 1)
            {
                byte v = Convert.ToByte((bool)value);
                marshaler.WriteByte(v);
            }
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            int size = GetSize(desc, SIZE);
            if (size == SIZE)
            {
                int v = marshaler.ReadInt32();
                v = MarshalerHelper.AdjustByteOrder(v, desc);
                return Convert.ToBoolean(v);
            }
            else //if (size == 1)
            {
                byte v = marshaler.ReadByte();
                return Convert.ToBoolean(v);
            }
        }
    }

    internal class CharMarshaler : ITypeMarshaler
    {
        private const int SIZE = 2;
        private const int ALIGNMENT = 2;

        private int GetSize(MarshalingDescriptor desc, int defaultValue)
        {
            int size;
            if (!MarshalingDescriptor.TryGetAttribute<int>(
                desc.Attributes, typeof(StaticSizeAttribute), "size", out size))
            {
                size = defaultValue;
            }
            if (size != SIZE && size != 1)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "Invalid static size '{0}' for char in structure '{1}'", size, desc.Type.Name));
            }
            return size;
        }

        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            return GetSize(desc, SIZE);
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return GetSize(desc, ALIGNMENT);
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return typeof(char);
        }

        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            int size = GetSize(desc, SIZE);
            if (size == SIZE)
            {
                short v = Convert.ToInt16((char)value);
                v = MarshalerHelper.AdjustByteOrder(v, desc);
                marshaler.WriteInt16(v);
            }
            else //if (size == 1)
            {
                byte v = Convert.ToByte((char)value);
                marshaler.WriteByte(v);
            }
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            int size = GetSize(desc, SIZE);
            if (size == SIZE)
            {
                short v = marshaler.ReadInt16();
                v = MarshalerHelper.AdjustByteOrder(v, desc);
                return Convert.ToChar(v);
            }
            else //if (size == 1)
            {
                byte v = marshaler.ReadByte();
                return Convert.ToChar(v);
            }
        }
    }

    #endregion

    #region String Marshalers

    internal class StringUTF16Marshaler : ITypeMarshaler
    {

        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            return marshaler.GetIntPtrSize();
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return marshaler.GetAlignment();
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return typeof(IntPtr);
        }


        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            string s = (string)value;
            IntPtr p = IOP.Marshal.StringToHGlobalUni(s);
            marshaler.MarkMemoryForDispose(p);
            marshaler.WriteIntPtr(p);
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            IntPtr p = marshaler.ReadIntPtr();
            marshaler.MarkForeignMemoryForDispose(p);

            string s = IOP.Marshal.PtrToStringUni(p);
            return s;
        }
    }

    internal class StringASCIIMarshaler : ITypeMarshaler
    {

        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            return marshaler.GetIntPtrSize();
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return marshaler.GetAlignment();
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return typeof(IntPtr);
        }


        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            string s = (string)value;
            IntPtr p = IOP.Marshal.StringToHGlobalAnsi(s);
            marshaler.MarkMemoryForDispose(p);
            marshaler.WriteIntPtr(p);
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            IntPtr p = marshaler.ReadIntPtr();
            marshaler.MarkForeignMemoryForDispose(p);

            string s = IOP.Marshal.PtrToStringAnsi(p);
            return s;
        }
    }

    internal class StringUTF16MarshalerInline : ITypeMarshaler
    {

        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            if (value == null)
            {
                int size;
                int length;
                marshaler.GetAdvocatedSize(desc, out size, out length);
                // The size of Unicode char is 2, so that the total size is "size * 2".
                return size * 2;
            }
            else
            {
                string s = (string)value;
                return Encoding.Unicode.GetByteCount(s) + 2; // FIXME: configure zero termination? 
            }
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return 2; // FIXME: alignment for inline case?
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return null;
        }


        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            string s = (string)value;
            byte[] bytes;
            if (MarshalerHelper.IsBigEndian(desc))
            {
                bytes = Encoding.BigEndianUnicode.GetBytes(s);
            }
            else
            {
                bytes = Encoding.Unicode.GetBytes(s);
            }
            for (int i = 0; i < bytes.Length; i++)
                marshaler.WriteByte(bytes[i]);
            if (bytes.Length % 2 == 1)
                marshaler.WriteByte(0);
            marshaler.WriteByte(0);
            marshaler.WriteByte(0);
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            List<byte> bytes = new List<byte>();
            byte b;
            do
            {
                b = marshaler.ReadByte();
                if (b == 0 && bytes.Count > 0 && bytes.Count % 2 == 1 && bytes[bytes.Count - 1] == 0)
                {
                    // double zero
                    bytes.RemoveAt(bytes.Count - 1);
                    break;
                }
                bytes.Add(b);
            } while (true);
            string s;
            if (MarshalerHelper.IsBigEndian(desc))
            {
                s = Encoding.BigEndianUnicode.GetString(bytes.ToArray());
            }
            else
            {
                s = Encoding.Unicode.GetString(bytes.ToArray());
            }
            return s;
        }
    }

    internal class StringASCIIMarshalerInline : ITypeMarshaler
    {

        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            if (value == null)
            {
                int size;
                int length;
                marshaler.GetAdvocatedSize(desc, out size, out length);
                return size;
            }
            else
            {
                string s = (string)value;
                return Encoding.ASCII.GetByteCount(s) + 1; // FIXME: configure zero termination? 
            }
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return 1; // FIXME: alignment for inline case?
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return null;
        }


        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            string s = (string)value;
            byte[] bytes;
            bytes = Encoding.ASCII.GetBytes(s);
            for (int i = 0; i < bytes.Length; i++)
                marshaler.WriteByte(bytes[i]);
            marshaler.WriteByte(0);
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            List<byte> bytes = new List<byte>();
            byte b;
            do
            {
                b = marshaler.ReadByte();
                if (b == 0)
                {
                    break;
                }
                bytes.Add(b);
            } while (true);
            string s = Encoding.ASCII.GetString(bytes.ToArray());
            return s;
        }
    }

    #endregion

    #region Struct marshaler

    internal class StructMarshaler : ITypeMarshaler
    {
        struct StructInfo
        {
            internal int alignment;
            internal FieldInfo[] fields;
        }

        Dictionary<MarshalingDescriptor, StructInfo> infos = new Dictionary<MarshalingDescriptor, StructInfo>();

        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            if (value == null)
                value = Activator.CreateInstance(desc.Type);

            StructInfo info = GetStructInfo(marshaler, desc);
            int requiredSize = 0;
            marshaler.EnterContext();
            marshaler.DefineSymbols(info.fields, value);

            foreach (FieldInfo field in info.fields)
            {
                MarshalingDescriptor fdesc = new MarshalingDescriptor(field.FieldType, field);
                int align = marshaler.GetAlignment(fdesc);
                requiredSize = Marshaler.Pad(requiredSize, align);
                object fv = field.GetValue(value);

                int s = marshaler.GetSize(fdesc, fv);
                if (s < 0)
                    marshaler.TestAssumeFail(fdesc, "required size is unknown");
                requiredSize += s;
            }
            requiredSize = Marshaler.Pad(requiredSize, info.alignment);
            marshaler.ExitContext();
            return requiredSize;
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            StructInfo info = GetStructInfo(marshaler, desc);
            return info.alignment;
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return desc.Type;
        }

        StructInfo GetStructInfo(Marshaler marshaler, MarshalingDescriptor desc)
        {
            StructInfo info;
            if (!infos.TryGetValue(desc, out info)) // CHECKME: hash also over marshaler object?
            {
                FieldInfo[] fields = desc.Type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                int maxAlign = 0;
                foreach (FieldInfo field in fields)
                {
                    MarshalerHelper.CheckNullableField(field, desc.Type);

                    MarshalingDescriptor fdesc = new MarshalingDescriptor(field.FieldType, field);
                    int align = marshaler.GetAlignment(fdesc);
                    maxAlign = align > maxAlign ? align : maxAlign;
                }
                info.alignment = maxAlign;
                info.fields = fields;
                infos[desc] = info;
            }
            return info;
        }

        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            StructInfo info = GetStructInfo(marshaler, desc);

            marshaler.EnterContext();
            marshaler.DefineSymbols(info.fields, value);
            marshaler.AlignWrite(marshaler.GetAlignment(desc));

            foreach (FieldInfo field in info.fields)
            {
                object fieldValue = field.GetValue(value);
                MarshalingDescriptor fdesc = new MarshalingDescriptor(field.FieldType, field);
                marshaler.AlignWrite(marshaler.GetAlignment(fdesc));

                if (marshaler.GetNativeType(fdesc) == typeof(IntPtr)
                    && fieldValue == null)
                {
                    marshaler.WriteIntPtr(IntPtr.Zero);
                }
                else
                {
                    marshaler.Marshal(new MarshalingDescriptor(field.FieldType, field), fieldValue);
                }
            }

            marshaler.AlignWrite(marshaler.GetAlignment(desc));
            marshaler.ExitContext();
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            StructInfo info = GetStructInfo(marshaler, desc);
            object value = Activator.CreateInstance(desc.Type);

            if (marshaler.IsProbingUnmarshaling)
            {
                marshaler.Skip(GetSize(marshaler, desc, value));
                return value;
            }

            marshaler.EnterContext();

            // Save the value of marshaler.UseSpaceChecking
            bool useSpaceChecking = marshaler.UseSpaceChecking;

            marshaler.DefineSymbols(info.fields, value);
            marshaler.AlignRead(marshaler.GetAlignment(desc));
            int offset = marshaler.RegionOffset;
            marshaler.IsProbingUnmarshaling = true;
            for (int pass = 0; pass < 2; pass++)
            {
                marshaler.RegionOffset = offset;
                foreach (FieldInfo field in info.fields)
                {
                    // If an field is a comformant array, disable the space checking,
                    // Because the region size is nondetermined before unmarshaling, 
                    // the left space cannot be checked correctly.
                    if (MarshalingDescriptor.HasAttribute(field, typeof(InlineAttribute))
                        && MarshalingDescriptor.HasAttribute(field, typeof(SizeAttribute)))
                    {
                        marshaler.UseSpaceChecking = false;
                    }

                    MarshalingDescriptor fdesc = new MarshalingDescriptor(field.FieldType, field);
                    marshaler.AlignRead(marshaler.GetAlignment(fdesc));

                    object fieldValue = marshaler.Unmarshal(fdesc);

                    field.SetValue(value, fieldValue);

                    if (marshaler.GetNativeType(fdesc) == typeof(IntPtr))
                    {
                        marshaler.DefinePointerSymbol(field.Name, fieldValue);
                    }
                    else
                    {
                        marshaler.DefineSymbol(field.Name, fieldValue);
                    }
                }
                marshaler.AlignRead(marshaler.GetAlignment(desc));
                marshaler.IsProbingUnmarshaling = false;
            }

            // Restore the value of marshaler.UseSpaceChecking
            marshaler.UseSpaceChecking = useSpaceChecking;

            marshaler.ExitContext();

            return value;
        }
    }

    internal class StructMarshalerInline : ITypeMarshaler
    {

        struct StructInfo
        {
            internal int alignment;
            internal FieldInfo[] fields;
        }

        Dictionary<MarshalingDescriptor, StructInfo> infos = new Dictionary<MarshalingDescriptor, StructInfo>();

        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            if (value == null)
                return -1;

            StructInfo info = GetStructInfo(marshaler, desc);
            int requiredSize = 0;
            int bitCount = 0;

            marshaler.EnterContext();
            foreach (FieldInfo field in info.fields)
            {
                MarshalingDescriptor fdesc = new MarshalingDescriptor(field.FieldType, field, desc.Type);
                int align = marshaler.GetAlignment(fdesc);
                requiredSize = Marshaler.Pad(requiredSize, align);
                object fv = field.GetValue(value);
                int s = marshaler.GetSize(fdesc, fv);
                if (s < 0)
                    marshaler.TestAssumeFail(fdesc, "required size is unknown");
                requiredSize += s;

                if (fdesc.Attributes.IsDefined(typeof(BitAttribute), false))
                {
                    bitCount += GetBitCount(marshaler, fdesc);
                }

                marshaler.DefineSymbol(field.Name, fv);
            }

            if (bitCount % 8 != 0)
            {
                marshaler.TestAssumeFail(desc, "invalid bit alignment");
            }

            requiredSize += bitCount / 8;
            marshaler.ExitContext();
            return requiredSize;
        }

        protected static int GetBitCount(Marshaler marshaler, MarshalingDescriptor desc)
        {
            int size, length;
            marshaler.GetAdvocatedSize(desc, out size, out length);
            return size;
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            StructInfo info = GetStructInfo(marshaler, desc);
            return info.alignment;
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return null;
        }

        StructInfo GetStructInfo(Marshaler marshaler, MarshalingDescriptor desc)
        {
            StructInfo info;
            if (!infos.TryGetValue(desc, out info)) // CHECKME: hash also over marshaler object?
            {
                FieldInfo[] fields = desc.Type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                int maxAlign = 0;
                foreach (FieldInfo field in fields)
                {
                    MarshalingDescriptor fdesc = new MarshalingDescriptor(field.FieldType, field, desc.Type);
                    int align = marshaler.GetAlignment(fdesc);
                    maxAlign = align > maxAlign ? align : maxAlign;
                }
                info.alignment = maxAlign;
                info.fields = fields;
                infos[desc] = info;
            }
            return info;
        }

        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            StructInfo info = GetStructInfo(marshaler, desc);
            marshaler.EnterContext();
            foreach (FieldInfo field in info.fields)
            {
                MarshalingDescriptor fdesc;
                if (!desc.Type.IsEnum)
                {
                    fdesc = new MarshalingDescriptor(field.FieldType, field, desc.Type);
                }
                else
                {
                    fdesc = new MarshalingDescriptor(
                    field.FieldType,
                    CustomAttributeFilter.GetEnumElementFilter(desc.Attributes));
                }
                object fieldValue = field.GetValue(value);
                if (CheckMarshalingCondition(field, desc, MarshalingType.Marshal, value))
                {
                    marshaler.Marshal(fdesc, fieldValue);
                }
                marshaler.DefineSymbol(field.Name, fieldValue);
            }
            marshaler.ExitContext();
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            StructInfo info = GetStructInfo(marshaler, desc);

            object value = Activator.CreateInstance(desc.Type);
            marshaler.EnterContext();
            foreach (FieldInfo field in info.fields)
            {
                MarshalingDescriptor fdesc;
                if (!desc.Type.IsEnum)
                {
                    fdesc = new MarshalingDescriptor(field.FieldType, field, desc.Type);
                }
                else
                {
                    fdesc = new MarshalingDescriptor(
                    field.FieldType,
                    CustomAttributeFilter.GetEnumElementFilter(desc.Attributes));
                }
                if (CheckMarshalingCondition(field, desc, MarshalingType.Unmarshal, value))
                {
                    object fieldValue = marshaler.Unmarshal(fdesc);
                    field.SetValue(value, fieldValue);
                    marshaler.DefineSymbol(field.Name, fieldValue);
                }
            }
            marshaler.ExitContext();
            return value;
        }

        private static bool CheckMarshalingCondition(FieldInfo field,
                                MarshalingDescriptor desc,
                                MarshalingType marshalingType,
                                object value)
        {
            object[] attrs = field.GetCustomAttributes(typeof(MarshalingConditionAttribute), false);
            if (attrs.Length == 0)
                return true;

            string methodName = ((MarshalingConditionAttribute)(attrs[0])).MethodName;

            MethodInfo method =
                desc.Type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (method == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot find method '{0}' in structure '{1}'", methodName, desc.Type.Name));
            }

            return (bool)method.Invoke(null, new object[] { marshalingType, value });
        }
    }

    #endregion

    #region Union Marshaler
    internal class UnionMarshaler : ITypeMarshaler
    {
        struct UnionInfo
        {
            internal string switchExpr;
            internal int size;
            internal int alignment;
            internal Dictionary<int, FieldInfo[]> caseFields;
            internal FieldInfo[] defaultFields;
        }

        Dictionary<MarshalingDescriptor, UnionInfo> infos = new Dictionary<MarshalingDescriptor, UnionInfo>();

        UnionInfo GetUnionInfo(Marshaler marshaler, MarshalingDescriptor desc)
        {
            UnionInfo info;
            if (!infos.TryGetValue(desc, out info)) // CHECKME: hash also over marshaler object?
            {
                info = new UnionInfo();
                info.caseFields = new Dictionary<int, FieldInfo[]>();

                if (!MarshalingDescriptor.TryGetAttribute<string>(desc.Attributes, typeof(SwitchAttribute), "expr",
                                                out info.switchExpr))
                    marshaler.TestAssumeFail(desc, "expected switch attribute at location of union");

                // pass 1: calculate union members
                FieldInfo[] fields = desc.Type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                List<FieldInfo> currentFields = null;
                string currentCaseExpr = null;
                int i = 0;
                int n = fields.Length;
                while (i <= n) // note we are running behind last element
                {
                    FieldInfo field;
                    bool isCase, isDefault, isEnd;
                    string caseExpr;
                    if (i == n)
                    {
                        isCase = isDefault = false;
                        field = null;
                        isEnd = true;
                        caseExpr = null;
                    }
                    else
                    {
                        isEnd = false;
                        field = fields[i];
                        isCase = MarshalingDescriptor.TryGetAttribute<string>(field,
                                                        typeof(CaseAttribute), "expr", out caseExpr);
                        isDefault = !isCase && MarshalingDescriptor.HasAttribute(field, typeof(CaseDefaultAttribute));
                    }

                    MarshalerHelper.CheckNullableField(field, desc.Type);

                    if (isCase || isDefault || isEnd)
                    {
                        // commit current member
                        if (currentFields != null)
                        {
                            if (currentCaseExpr != null)
                            {
                                IList<int?> caseValues = marshaler.EvaluateConstant(currentCaseExpr);
                                FieldInfo[] fieldArray = currentFields.ToArray();
                                foreach (int? caseValue in caseValues)
                                {
                                    if (caseValue.HasValue)
                                    {
                                        if (info.caseFields.ContainsKey(caseValue.Value))
                                            marshaler.TestAssumeFail(desc, "duplicate case label '{0}'",
                                                                        caseValue.Value);
                                        info.caseFields[caseValue.Value] = fieldArray;
                                    }
                                }
                            }
                            else
                            {
                                if (info.defaultFields != null)
                                    marshaler.TestAssumeFail(desc, "duplicate default label");
                                info.defaultFields = currentFields.ToArray();
                            }
                        }
                    }
                    if (isCase || isDefault)
                    {
                        // setup for next union member
                        currentFields = new List<FieldInfo>();
                        currentFields.Add(field);
                        if (isCase)
                            currentCaseExpr = caseExpr;
                        else
                            currentCaseExpr = null;
                    }
                    i++;
                }

                // pass 2: calculate size and alignment
                foreach (FieldInfo[] caseFields in info.caseFields.Values)
                    CalcSize(marshaler, ref info, caseFields);
                if (info.defaultFields != null)
                    CalcSize(marshaler, ref info, info.defaultFields);
                info.size = Marshaler.Pad(info.size, info.alignment);
                infos[desc] = info;
            }
            return info;
        }

        static void CalcSize(Marshaler marshaler, ref UnionInfo info, FieldInfo[] fields)
        {
            int size = 0;
            bool first = true;
            foreach (FieldInfo field in fields)
            {
                MarshalingDescriptor desc = new MarshalingDescriptor(field.FieldType, field);
                int fsize = marshaler.GetSize(desc, null);
                if (fsize < 0)
                    marshaler.TestAssumeFail(desc, "required size unknown");
                int falign = marshaler.GetAlignment(desc);
                size += fsize;
                if (first && falign > info.alignment)
                    info.alignment = falign;
            }
            if (size > info.size)
                info.size = size;
        }


        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            UnionInfo info = GetUnionInfo(marshaler, desc);
            return info.size;
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            UnionInfo info = GetUnionInfo(marshaler, desc);
            return info.alignment;
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return null;
        }


        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            UnionInfo info = GetUnionInfo(marshaler, desc);

            int switchValue = marshaler.Evaluate(info.switchExpr);
            FieldInfo[] fields;
            if (!info.caseFields.TryGetValue(switchValue, out fields))
            {
                if (info.defaultFields != null)
                    fields = info.defaultFields;
                else
                    marshaler.TestAssertFail(desc, "no case of union matches switch value '{0}', and no default is given",
                                                switchValue);
            }

            int maxFieldSize = 0;
            marshaler.EnterContext();
            marshaler.DefineSymbols(fields, value);
            foreach (FieldInfo field in fields)
            {
                MarshalingDescriptor fdesc = new MarshalingDescriptor(field.FieldType, field);
                object fieldValue = field.GetValue(value);

                int fieldSize = marshaler.GetSize(fieldValue);
                if (maxFieldSize < fieldSize)
                {
                    maxFieldSize = fieldSize;
                }

                marshaler.AlignWrite(marshaler.GetAlignment(fdesc));
                marshaler.Marshal(fdesc, fieldValue);
            }

            marshaler.Clear(info.size - maxFieldSize);

            marshaler.ExitContext();
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            UnionInfo info = GetUnionInfo(marshaler, desc);

            object value = Activator.CreateInstance(desc.Type);

            if (marshaler.IsProbingUnmarshaling)
            {
                marshaler.Skip(info.size);
                return value;
            }

            int switchValue = marshaler.Evaluate(info.switchExpr);
            FieldInfo[] fields;
            if (!info.caseFields.TryGetValue(switchValue, out fields))
            {
                if (info.defaultFields != null)
                    fields = info.defaultFields;
                else
                    marshaler.TestAssertFail(desc, "no case of union matches switch value '{0}', and no default is given",
                                                switchValue);
            }

            int maxFieldSize = 0;
            marshaler.EnterContext();
            marshaler.DefineSymbols(fields, value);
            int offset = marshaler.RegionOffset;
            marshaler.IsProbingUnmarshaling = true;
            for (int pass = 0; pass < 2; pass++)
            {
                marshaler.RegionOffset = offset;
                foreach (FieldInfo field in fields)
                {
                    MarshalingDescriptor fdesc = new MarshalingDescriptor(field.FieldType, field);
                    marshaler.AlignRead(marshaler.GetAlignment(fdesc));
                    object fieldValue = marshaler.Unmarshal(fdesc);

                    // get the size of field with the help of its marshaling descriptor
                    int fieldSize = marshaler.GetSize(fdesc, fieldValue);
                    if (maxFieldSize < fieldSize)
                    {
                        maxFieldSize = fieldSize;
                    }

                    field.SetValue(value, fieldValue);
                    marshaler.DefineSymbol(field.Name, fieldValue);
                }

                marshaler.IsProbingUnmarshaling = false;
            }

            marshaler.Skip(info.size - maxFieldSize);

            marshaler.ExitContext();
            return value;
        }

    }

    #endregion

    #region Indirect Marshalers

    internal class GenericIndirectMarshaler : IGenericTypeMarshaler
    {

        public ITypeMarshaler Instantiate(Type baseType, params ITypeMarshaler[] elementMarshalers)
        {
            return new IndirectMarshaler(elementMarshalers[0]);
        }

        class IndirectMarshaler : ITypeMarshaler
        {
            ITypeMarshaler innerTypeMarshaler;

            internal IndirectMarshaler(ITypeMarshaler msr)
            {
                this.innerTypeMarshaler = msr;
            }

            public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
            {
                return marshaler.GetIntPtrSize();
            }

            public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
            {
                return marshaler.GetAlignment();
            }

            public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
            {
                return typeof(IntPtr);
            }

            public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
            {
                int regionSize = innerTypeMarshaler.GetSize(marshaler, desc, value);

                if (regionSize < 0)
                    marshaler.TestAssumeFail(desc, "required size is unknown");
                //Generic Indrection is always used as field of a struct, struct marshaller will add padding to each field by alignment
                int align = this.GetAlignment(marshaler, desc);
                while (regionSize % align != 0)
                    regionSize++;
                IRegion region = marshaler.AllocateRegion(regionSize);
                marshaler.WriteIntPtr(region.NativeMemory);
                marshaler.EnterRegion(region);
                innerTypeMarshaler.Marshal(marshaler, desc, value);
                marshaler.ExitRegion();
            }

            public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
            {
                int regionSize = innerTypeMarshaler.GetSize(marshaler, desc, null);

                if (regionSize < 0)
                    marshaler.TestAssumeFail(desc, "required size is unknown");
                IntPtr region = marshaler.ReadIntPtr();
                if (region != IntPtr.Zero)
                {
                    marshaler.MarkForeignMemoryForDispose(region);

                    marshaler.EnterRegion(marshaler.MakeRegion(region, regionSize));
                    // use the marshaling descriptor of the pointed type to do unmarshal
                    var innerDesc = new MarshalingDescriptor(desc.Type);
                    object value = innerTypeMarshaler.Unmarshal(marshaler, innerDesc);
                    marshaler.ExitRegion();
                    return value;
                }
                else
                {
                    if (desc.IsNullable)
                    {
                        return null;
                    }
                    else
                    {
                        return Activator.CreateInstance(desc.Type);
                    }
                }
            }

        }
    }

    #endregion

    #region Array Marshalers

    internal class GenericArrayMarshaler : IGenericTypeMarshaler
    {
        public ITypeMarshaler Instantiate(Type baseType, params ITypeMarshaler[] elementMarshalers)
        {
            return new ArrayMarshaler(elementMarshalers[0]);
        }


        class ArrayMarshaler : ITypeMarshaler
        {
            ITypeMarshaler innerTypeMarshaler;

            internal ArrayMarshaler(ITypeMarshaler msr)
            {
                this.innerTypeMarshaler = msr;
            }

            public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
            {
                return marshaler.GetIntPtrSize();
            }

            public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
            {
                return marshaler.GetAlignment();
            }

            public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
            {
                return typeof(IntPtr);
            }

            public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
            {
                if (value == null)
                {
                    marshaler.EnterArrayLevel();
                    marshaler.WriteIntPtr(IntPtr.Zero);
                    marshaler.ExitArrayLevel();
                }
                else
                {
                    int size, length;

                    GetCurrentSizeAndLength(marshaler, desc, out size, out length);

                    MarshalingDescriptor innerDesc = new MarshalingDescriptor(
                        desc.Type.GetElementType(),
                        CustomAttributeFilter.GetArrayElementFilter(desc.Attributes));

                    Array arr = (Array)value;

                    if (arr.Length > size)
                        marshaler.TestAssertFail(desc,
                                    "array is larger than the advocated size");

                    int innerAlign = marshaler.GetAlignment(innerDesc);

                    int regionSize = 0;

                    for (int i = 0; i < length; i++)
                    {
                        int align = innerAlign - 1;
                        regionSize = (regionSize + align) & ~align;

                        int elementSize = innerTypeMarshaler.GetSize(marshaler, innerDesc, arr.GetValue(i));
                        if (elementSize <= 0)
                            marshaler.TestAssumeFail(innerDesc, "required size is unknown or invalid");

                        regionSize += elementSize;
                    }

                    IRegion region = marshaler.AllocateRegion(regionSize);
                    marshaler.WriteIntPtr(region.NativeMemory);
                    marshaler.EnterRegion(region);

                    marshaler.EnterArrayLevel();
                    for (int i = 0; i < length; i++)
                    {
                        marshaler.AlignWrite(innerAlign);
                        innerTypeMarshaler.Marshal(marshaler, innerDesc, arr.GetValue(i));
                    }

                    marshaler.ExitArrayLevel();

                    // skip remaining entries
                    marshaler.Clear(regionSize - region.Offset);
                    marshaler.ExitRegion();
                }
            }

            public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
            {
                MarshalingDescriptor innerDesc = new MarshalingDescriptor(
                    desc.Type.GetElementType(),
                    CustomAttributeFilter.GetArrayElementFilter(desc.Attributes));

                int elementSize = innerTypeMarshaler.GetSize(marshaler, innerDesc, null);
                if (elementSize <= 0)
                    marshaler.TestAssumeFail(innerDesc, "required size is unknown or invalid");

                int size, length;

                GetCurrentSizeAndLength(marshaler, desc, out size, out length);

                int regionSize = size * elementSize;
                IntPtr region = marshaler.ReadIntPtr();

                marshaler.MarkForeignMemoryForDispose(region);

                Array arr;
                if (region != IntPtr.Zero)
                {
                    marshaler.EnterRegion(marshaler.MakeRegion(region, regionSize));
                    arr = Array.CreateInstance(innerDesc.Type, length);
                    int innerAlign = marshaler.GetAlignment(innerDesc);

                    marshaler.EnterArrayLevel();
                    for (int i = 0; i < length; i++)
                    {
                        marshaler.AlignRead(innerAlign);
                        arr.SetValue(innerTypeMarshaler.Unmarshal(marshaler, innerDesc), i);
                    }
                    marshaler.ExitArrayLevel();

                    marshaler.Skip((size - length) * elementSize);
                    marshaler.ExitRegion();
                }
                else
                {
                    marshaler.EnterArrayLevel();
                    arr = null;
                    marshaler.ExitArrayLevel();
                }

                return arr;
            }

            // This method would not be marked as static because this is an instance based class
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
            public void GetCurrentSizeAndLength(Marshaler marshaler, MarshalingDescriptor desc, out int size, out int length)
            {
                IList<int> sizes;
                IList<int> lengths;
                int arrayLevel = marshaler.ArrayLevel;

                if (!marshaler.ArrayContextAvailable)
                {
                    marshaler.GetMultipleAdvocatedSizes(desc, out sizes, out lengths);
                    marshaler.ArraySizes = sizes;
                    marshaler.ArrayLengths = lengths;
                }
                else
                {
                    sizes = marshaler.ArraySizes;
                    lengths = marshaler.ArrayLengths;
                }

                size = sizes[arrayLevel];
                length = lengths[arrayLevel];
            }
        }
    }

    internal class GenericArrayMarshalerInline : IGenericTypeMarshaler
    {
        public ITypeMarshaler Instantiate(Type baseType, params ITypeMarshaler[] elementMarshalers)
        {
            return new ArrayMarshalerInline(elementMarshalers[0]);
        }


        class ArrayMarshalerInline : ITypeMarshaler
        {
            ITypeMarshaler innerTypeMarshaler;

            internal ArrayMarshalerInline(ITypeMarshaler msr)
            {
                this.innerTypeMarshaler = msr;
            }

            private int GetSizeRecursion(Marshaler marshaler, MarshalingDescriptor desc, Array arr, IList<int> lengths, int rank, int[] indices)
            {
                int Size = 0;

                if (rank == 0)
                {
                    MarshalingDescriptor innerDesc = new MarshalingDescriptor(
                        desc.Type.GetElementType(),
                        CustomAttributeFilter.GetArrayElementFilter(desc.Attributes));

                    Size += innerTypeMarshaler.GetSize(marshaler, innerDesc, arr.GetValue(indices));
                    return Size;
                }

                List<int> list = new List<int>(indices);
                list.Add(0);
                int[] tempIndices = list.ToArray();
                for (int i = 0; i < lengths[rank - 1]; i++)
                {
                    tempIndices[rank - 1] = i;
                    Size += GetSizeRecursion(marshaler, desc, arr, lengths, rank - 1, tempIndices);
                }

                return Size;
            }

            public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
            {
                MarshalingDescriptor innerDesc = new MarshalingDescriptor(
                    desc.Type.GetElementType(),
                    CustomAttributeFilter.GetArrayElementFilter(desc.Attributes));

                int elementSize = innerTypeMarshaler.GetSize(marshaler, innerDesc, Activator.CreateInstance(innerDesc.Type));
                if (elementSize <= 0)
                    marshaler.TestAssumeFail(innerDesc, "required size is unknown or invalid");

                IList<int> sizes;
                IList<int> lengths;
                marshaler.GetMultipleAdvocatedSizes(desc, out sizes, out lengths);

                if (sizes.Count == 0)
                {
                    marshaler.TestAssertFail(innerDesc, "size expression is invalid");
                }

                if (value != null && sizes.Count != ((Array)value).Rank)
                {
                    marshaler.TestAssertFail(desc,
                                "the number of values in size expression is not consistent with the actual array dimension");
                }

                int totalSize = 1;
                Array arr = (Array)value;

                if (arr != null && innerDesc.Type != typeof(byte))
                {
                    for (int rank = 0; rank < arr.Rank; rank++)
                    {
                        if (arr.GetLength(rank) > sizes[rank])
                            marshaler.TestAssertFail(desc,
                                    "array is larger than the advocated size");
                    }

                    int[] indicies = new int[0];
                    elementSize += GetSizeRecursion(marshaler, desc, arr, lengths, arr.Rank, indicies);
                }
                else
                {
                    foreach (int size in sizes)
                    {
                        totalSize *= size;
                    }
                }

                return totalSize * elementSize;
            }

            public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
            {
                MarshalingDescriptor innerDesc = new MarshalingDescriptor(
                    desc.Type.GetElementType(),
                    CustomAttributeFilter.GetArrayElementFilter(desc.Attributes));

                return marshaler.GetAlignment(innerDesc);
            }

            public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
            {
                return null;
            }

            public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
            {
                if (value == null)
                    marshaler.TestAssertFail(desc, "array value cannot be null");

                MarshalingDescriptor innerDesc = new MarshalingDescriptor(
                    desc.Type.GetElementType(),
                    CustomAttributeFilter.GetArrayElementFilter(desc.Attributes));

                int elementSize = innerTypeMarshaler.GetSize(marshaler, innerDesc, Activator.CreateInstance(innerDesc.Type));
                if (elementSize <= 0)
                    marshaler.TestAssumeFail(innerDesc, "required size is unknown or invalid");

                IList<int> sizes;
                IList<int> lengths;
                marshaler.GetMultipleAdvocatedSizes(desc, out sizes, out lengths);

                Array arr = (Array)value;

                if (sizes.Count != arr.Rank)
                {
                    marshaler.TestAssertFail(desc,
                                "the number of values in size expression is not consistent with the actual array dimension");
                }

                for (int rank = 0; rank < arr.Rank; rank++)
                {
                    if (arr.GetLength(rank) > sizes[rank])
                        marshaler.TestAssertFail(desc,
                                "array is larger than the advocated size");
                }

                if (arr.Rank == 1)
                {
                    for (int i = 0; i < lengths[0]; i++)
                    {
                        innerTypeMarshaler.Marshal(marshaler, innerDesc, arr.GetValue(i));
                    }
                    // skip remaining entries
                    marshaler.Clear((sizes[0] - lengths[0]) * elementSize);
                }
                else if (arr.Rank == 2)
                {
                    for (int i = 0; i < sizes[0]; i++)
                    {
                        for (int j = 0; j < sizes[1]; j++)
                        {
                            int[] indices = { i, j };
                            innerTypeMarshaler.Marshal(marshaler, innerDesc, arr.GetValue(indices));
                        }
                    }
                }
                else if (arr.Rank == 3)
                {
                    for (int i = 0; i < sizes[0]; i++)
                    {
                        for (int j = 0; j < sizes[1]; j++)
                        {
                            for (int k = 0; k < sizes[2]; k++)
                            {
                                int[] indices = { i, j, k };
                                innerTypeMarshaler.Marshal(marshaler, innerDesc, arr.GetValue(indices));
                            }
                        }
                    }
                }
                else
                {
                    marshaler.TestAssertFail(desc,
                                "do not support four dimension array and above");
                }
            }

            public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
            {
                MarshalingDescriptor innerDesc = new MarshalingDescriptor(
                    desc.Type.GetElementType(),
                    CustomAttributeFilter.GetArrayElementFilter(desc.Attributes));

                int elementSize = innerTypeMarshaler.GetSize(marshaler, innerDesc, Activator.CreateInstance(innerDesc.Type));
                if (elementSize <= 0)
                    marshaler.TestAssumeFail(innerDesc, "required size is unknown or invalid");
                IList<int> sizes;
                IList<int> lengths;
                marshaler.GetMultipleAdvocatedSizes(desc, out sizes, out lengths);

                int[] arrayIndices = new int[sizes.Count];
                for (int i = 0; i < arrayIndices.Length; i++)
                {
                    arrayIndices[i] = sizes[i];
                }
                Array arr = Array.CreateInstance(innerDesc.Type, arrayIndices);

                if (sizes.Count != arr.Rank)
                {
                    marshaler.TestAssertFail(desc,
                                "the number of values in size expression is not consistent with the actual array dimension");
                }

                if (arr.Rank == 1)
                {
                    for (int i = 0; i < lengths[0]; i++)
                    {
                        arr.SetValue(innerTypeMarshaler.Unmarshal(marshaler, innerDesc), i);
                    }
                    marshaler.Skip((sizes[0] - lengths[0]) * elementSize);
                }
                else if (arr.Rank == 2)
                {
                    for (int i = 0; i < sizes[0]; i++)
                    {
                        for (int j = 0; j < sizes[1]; j++)
                        {
                            int[] indices = { i, j };
                            arr.SetValue(innerTypeMarshaler.Unmarshal(marshaler, innerDesc), indices);
                        }
                    }
                }
                else if (arr.Rank == 3)
                {
                    for (int i = 0; i < sizes[0]; i++)
                    {
                        for (int j = 0; j < sizes[1]; j++)
                        {
                            for (int k = 0; k < sizes[2]; k++)
                            {
                                int[] indices = { i, j, k };
                                arr.SetValue(innerTypeMarshaler.Unmarshal(marshaler, innerDesc), indices);
                            }
                        }
                    }
                }
                else
                {
                    marshaler.TestAssertFail(desc,
                                "do not support four dimension array and above");
                }

                return arr;
            }

        }
    }

    #endregion

    #region Sequence Marshaler

    internal class GenericSequenceMarshaler : IGenericTypeMarshaler
    {
        public ITypeMarshaler Instantiate(Type baseType, params ITypeMarshaler[] elementMarshalers)
        {
            return new SequenceMarshaler(elementMarshalers[0]);
        }


        class SequenceMarshaler : ITypeMarshaler
        {
            ITypeMarshaler innerTypeMarshaler;

            internal SequenceMarshaler(ITypeMarshaler msr)
            {
                this.innerTypeMarshaler = msr;
            }

            public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
            {
                return marshaler.GetIntPtrSize();
            }

            public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
            {
                return marshaler.GetAlignment();//4;
            }

            public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
            {
                return typeof(IntPtr);
            }

            public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
            {
                if (value == null)
                {
                    marshaler.EnterSequenceLevel();
                    marshaler.WriteIntPtr(IntPtr.Zero);
                    marshaler.ExitSequenceLevel();
                }
                else
                {
                    int size, length;

                    GetCurrentSizeAndLength(marshaler, desc, out size, out length);

                    Type elementType = desc.Type.GetGenericArguments()[0];
                    MarshalingDescriptor innerDesc = new MarshalingDescriptor(
                        elementType,
                        CustomAttributeFilter.GetArrayElementFilter(desc.Attributes));

                    MethodInfo getCount = desc.Type.GetMethod("get_Count");
                    MethodInfo getItem = desc.Type.GetMethod("get_Item");
                    object[] noArgs = new object[0];

                    int count = (int)getCount.Invoke(value, noArgs);

                    if (count > size)
                        marshaler.TestAssertFail(desc,
                                    "sequence is larger than the advocated size");

                    int innerAlign = marshaler.GetAlignment(innerDesc);

                    int regionSize = 0;

                    for (int i = 0; i < count; i++)
                    {
                        if (i >= length)
                            break;

                        int align = innerAlign - 1;
                        regionSize = (regionSize + align) & ~align;

                        object elem = getItem.Invoke(value, new object[] { i });
                        int elementSize = innerTypeMarshaler.GetSize(marshaler, innerDesc, elem);
                        if (elementSize <= 0)
                            marshaler.TestAssumeFail(innerDesc, "required size is unknown or invalid");

                        regionSize += elementSize;
                    }

                    IRegion region = marshaler.AllocateRegion(regionSize);
                    marshaler.WriteIntPtr(region.NativeMemory);
                    marshaler.EnterRegion(region);

                    marshaler.EnterSequenceLevel();
                    for (int i = 0; i < count; i++)
                    {
                        if (i >= length)
                            break;
                        marshaler.AlignWrite(innerAlign);
                        object elem = getItem.Invoke(value, new object[] { i });
                        innerTypeMarshaler.Marshal(marshaler, innerDesc, elem);
                    }
                    marshaler.ExitSequenceLevel();

                    // skip remaining entries
                    marshaler.Clear(regionSize - region.Offset);
                    marshaler.ExitRegion();
                }
            }

            public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
            {
                Type elementType = desc.Type.GetGenericArguments()[0];
                MarshalingDescriptor innerDesc = new MarshalingDescriptor(
                    elementType,
                    CustomAttributeFilter.GetArrayElementFilter(desc.Attributes));

                int elementSize = innerTypeMarshaler.GetSize(marshaler, innerDesc, null);
                if (elementSize <= 0)
                    marshaler.TestAssumeFail(innerDesc, "required size is unknown or invalid");

                int size, length;

                GetCurrentSizeAndLength(marshaler, desc, out size, out length);

                int regionSize = size * elementSize;
                IntPtr region = marshaler.ReadIntPtr();
                marshaler.MarkForeignMemoryForDispose(region);

                object seq;
                if (region != IntPtr.Zero)
                {
                    seq = Activator.CreateInstance(desc.Type);
                    MethodBase addMethod = desc.Type.GetMethod("Add");
                    marshaler.EnterRegion(marshaler.MakeRegion(region, regionSize));
                    marshaler.EnterSequenceLevel();
                    int innerAlign = marshaler.GetAlignment(innerDesc);
                    for (int i = 0; i < length; i++)
                    {
                        marshaler.AlignRead(innerAlign);
                        seq = addMethod.Invoke(seq, new object[]{
                                    innerTypeMarshaler.Unmarshal(marshaler, innerDesc)});
                    }
                    marshaler.ExitSequenceLevel();

                    marshaler.Skip((size - length) * elementSize);
                    marshaler.ExitRegion();
                }
                else
                {
                    marshaler.EnterSequenceLevel();
                    seq = null;
                    marshaler.ExitSequenceLevel();
                }

                return seq;
            }

            // This method would not be marked as static because this is an instance based class
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
            public void GetCurrentSizeAndLength(Marshaler marshaler, MarshalingDescriptor desc, out int size, out int length)
            {
                IList<int> sizes;
                IList<int> lengths;
                int sequenceLevel = marshaler.SequenceLevel;

                if (!marshaler.SequenceContextAvailable)
                {
                    marshaler.GetMultipleAdvocatedSizes(desc, out sizes, out lengths);
                    marshaler.SequenceSizes = sizes;
                    marshaler.SequenceLengths = lengths;
                }
                else
                {
                    sizes = marshaler.SequenceSizes;
                    lengths = marshaler.SequenceLengths;
                }

                size = sizes[sequenceLevel];
                length = lengths[sequenceLevel];
            }
        }
    }


    internal class GenericSequenceMarshalerInline : IGenericTypeMarshaler
    {
        public ITypeMarshaler Instantiate(Type baseType, params ITypeMarshaler[] elementMarshalers)
        {
            return new SequenceMarshalerInline(elementMarshalers[0]);
        }


        class SequenceMarshalerInline : ITypeMarshaler
        {
            ITypeMarshaler innerTypeMarshaler;

            internal SequenceMarshalerInline(ITypeMarshaler msr)
            {
                this.innerTypeMarshaler = msr;
            }

            public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
            {
                MarshalingDescriptor innerDesc = new MarshalingDescriptor(
                    desc.Type.GetGenericArguments()[0],
                    CustomAttributeFilter.GetArrayElementFilter(desc.Attributes));

                int elementSize = innerTypeMarshaler.GetSize(marshaler, innerDesc, Activator.CreateInstance(innerDesc.Type));
                if (elementSize <= 0)
                    return -1;
                int size, length;
                marshaler.GetAdvocatedSize(desc, out size, out length);
                return size * elementSize;
            }

            public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
            {
                MarshalingDescriptor innerDesc = new MarshalingDescriptor(
                    desc.Type.GetGenericArguments()[0],
                    CustomAttributeFilter.GetArrayElementFilter(desc.Attributes));

                return marshaler.GetAlignment(innerDesc);
            }

            public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
            {
                return null;
            }


            public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
            {
                if (value == null)
                    marshaler.TestAssertFail(desc, "sequence cannot be null");

                Type elementType = desc.Type.GetGenericArguments()[0];
                MarshalingDescriptor innerDesc = new MarshalingDescriptor(
                    elementType,
                    CustomAttributeFilter.GetArrayElementFilter(desc.Attributes));

                int size, length;
                marshaler.GetAdvocatedSize(desc, out size, out length);

                MethodInfo getCount = desc.Type.GetMethod("get_Count");
                MethodInfo getItem = desc.Type.GetMethod("get_Item");
                object[] noArgs = new object[0];

                int count = (int)getCount.Invoke(value, noArgs);

                if (count > size)
                    marshaler.TestAssertFail(desc,
                                "sequence is larger than the advocated size");

                for (int i = 0; i < count; i++)
                {
                    if (i >= length)
                        break;
                    object elem = getItem.Invoke(value, new object[] { i });
                    innerTypeMarshaler.Marshal(marshaler, innerDesc, elem);
                }
                // skip remaining entries
                if (size != length)
                {
                    int elementSize = innerTypeMarshaler.GetSize(marshaler, innerDesc, Activator.CreateInstance(innerDesc.Type));
                    if (elementSize <= 0)
                        marshaler.TestAssumeFail(innerDesc, "required size is unknown or invalid");

                    marshaler.Clear((size - length) * elementSize);
                }
            }

            public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
            {
                Type elementType = desc.Type.GetGenericArguments()[0];
                MarshalingDescriptor innerDesc = new MarshalingDescriptor(
                    elementType,
                    CustomAttributeFilter.GetArrayElementFilter(desc.Attributes));

                int size, length;
                marshaler.GetAdvocatedSize(desc, out size, out length);

                object seq = Activator.CreateInstance(desc.Type);
                MethodBase addMethod = desc.Type.GetMethod("Add");
                for (int i = 0; i < length; i++)
                {
                    seq = addMethod.Invoke(seq, new object[]{
                                    innerTypeMarshaler.Unmarshal(marshaler, innerDesc)});
                }
                if (size != length)
                {
                    int elementSize = innerTypeMarshaler.GetSize(marshaler, innerDesc, Activator.CreateInstance(innerDesc.Type));
                    if (elementSize <= 0)
                        marshaler.TestAssumeFail(innerDesc, "required size is unknown or invalid");
                    marshaler.Skip((size - length) * elementSize);
                }
                return seq;
            }

        }
    }

    #endregion

    #region Binding Marshaler

    class BindingMarshaler : ITypeMarshaler
    {
        string name;
        ITypeMarshaler innerMarshaler;

        internal BindingMarshaler(string name, ITypeMarshaler innerMarshaler)
        {
            this.name = name;
            this.innerMarshaler = innerMarshaler;
        }


        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            return innerMarshaler.GetSize(marshaler, desc, value);
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return innerMarshaler.GetAlignment(marshaler, desc);
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return innerMarshaler.GetNativeType(marshaler, desc);
        }

        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            IdentifierBinding<object> binding = marshaler.GetBinding(name);
            object target = binding.GetTarget((int)value);
            innerMarshaler.Marshal(marshaler, desc, target);
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            IdentifierBinding<object> binding = marshaler.GetBinding(name);
            object target = innerMarshaler.Unmarshal(marshaler, desc);
            if (!binding.IsTargetBound(target))
            {
                // create new binding
                int id = binding.GetUnusedIdentifier();
                binding.Bind(id, target);
                return id;
            }
            else
                return binding.GetIdentifier(target);
        }

    }

    #endregion

    #region Guid marshaling

    internal class GuidMarshaler : ITypeMarshaler
    {

        public int GetSize(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            return 16;
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor desc)
        {
            //GUID definition in native code:
            //typedef struct _GUID {
            //    unsigned long  Data1;
            //    unsigned short Data2;
            //    unsigned short Data3;
            //    unsigned char  Data4[ 8 ];
            //} GUID;
            //The max field is 4 bytes
            return 4;
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor desc)
        {
            return null;
        }


        public void Marshal(Marshaler marshaler, MarshalingDescriptor desc, object value)
        {
            Guid g = (Guid)value;
            foreach (byte b in g.ToByteArray())
                marshaler.WriteByte(b);
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor desc)
        {
            byte[] bytes = new byte[16];
            for (int i = 0; i < 16; i++)
                bytes[i] = marshaler.ReadByte();

            return new Guid(bytes);
        }
    }


    #endregion

    #region Bit Marshaler

    internal class BitMarshaler : ITypeMarshaler
    {
        #region ITypeMarshaler Members

        public int GetSize(Marshaler marshaler, MarshalingDescriptor descriptor, object value)
        {
            // bit type doesn't have a byte-based size, so return zero.
            return 0;
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor descriptor)
        {
            // bit type doesn't have a byte-based alignment, so return zero.
            return 0;
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor descriptor)
        {
            return null;
        }

        public void Marshal(Marshaler marshaler, MarshalingDescriptor descriptor, object value)
        {
            if (descriptor.Type == typeof(int))
            {
                int bit = (int)value;
                CheckBit(marshaler, descriptor, bit);
                marshaler.WriteBit(bit);
            }
            else
            {
                foreach (int bit in (int[])value)
                {
                    CheckBit(marshaler, descriptor, bit);
                    marshaler.WriteBit(bit);
                }
            }
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor descriptor)
        {
            if (descriptor.Type == typeof(int))
            {
                int bit = marshaler.ReadBit();
                CheckBit(marshaler, descriptor, bit);
                return bit;
            }
            else
            {
                int size, length;
                marshaler.GetAdvocatedSize(descriptor, out size, out length);
                Array array = Array.CreateInstance(descriptor.Type.GetElementType(), length);
                for (int i = 0; i < length; i++)
                {
                    int bit = marshaler.ReadBit();
                    CheckBit(marshaler, descriptor, bit);
                    array.SetValue(bit, i);
                }

                return array;
            }
        }

        // Instance based class should not have static method.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected void CheckBit(Marshaler marshaler, MarshalingDescriptor descriptor, int bit)
        {
            if (bit != 0 && bit != 1)
            {
                marshaler.TestAssumeFail(descriptor, "bit value must be 0 or 1.");
            }
        }
        #endregion
    }

    #endregion

    #region Asn.1 Marshaler

    /// <summary>
    /// A stream that provides a wrapper for Marshaler so that Asn.1 decode buffer can get bytes from Marshaler.
    /// </summary>
    internal class Asn1BufferedStream : Stream
    {
        private Marshaler marshaler;
        private List<byte> bytesBuffer = new List<byte>();
        private long position;

        public Asn1BufferedStream(Marshaler marshaler)
        {
            this.marshaler = marshaler;
        }
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get
            {
                return bytesBuffer.Count;
            }
        }

        public override long Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", "'offset' must be positive");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "'count' must be positive");
            }

            if ((buffer.Length - offset) < count)
            {
                throw new ArgumentException("The length of the buffer is not large enough to hold the incoming data");
            }

            // Read bytes from marshaler if the expected bytes are not in the buffer.
            int size = (int)position + count;
            AppendBytes(size - bytesBuffer.Count);

            for (int i = 0; i < count; i++)
            {
                buffer[offset + i] = bytesBuffer[(int)position + i];
            }
            position += count;

            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    if (offset < 0L)
                    {
                        throw new IOException("'offset' must not be negative when SeekOrigin is SeekOrigin.Begin");
                    }
                    AppendBytes(offset - Length);
                    position = offset;
                    break;

                case SeekOrigin.Current:
                    if (offset + position < 0)
                    {
                        throw new IOException("expected position is out of range");
                    }
                    long num = position + offset - Length;
                    AppendBytes(num);
                    position += (int)offset;
                    break;

                case SeekOrigin.End:
                    long len = Length;
                    if (Length + offset < 0)
                    {
                        throw new IOException("expected position is out of range");
                    }
                    AppendBytes(offset);
                    position = len + offset;
                    break;

                default:
                    throw new ArgumentException("seek origin is invalid");
            }
            return position;

        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        protected void AppendBytes(long num)
        {
            for (long i = 0; i < num; i++)
            {
                byte b = marshaler.ReadByte();
                bytesBuffer.Add(b);
            }
        }
    }

    #region BER marshaler
    /// <summary>
    /// A marshaler used to marshal Asn.1 type by BER.
    /// </summary>
    internal class Asn1BerMarshaler : ITypeMarshaler
    {
        public int GetSize(Marshaler marshaler, MarshalingDescriptor descriptor, object value)
        {
            if (marshaler == null)
            {
                throw new ArgumentNullException("marshaler");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Asn1Object asnValue = value as Asn1Object;
            if (asnValue == null)
            {
                throw new ArgumentException("The type of Asn.1 value must be derived from 'Asn1Object'");
            }

            Asn1BerEncodingBuffer encodeBuffer = CreateEncodeBuffer();
            asnValue.BerEncode(encodeBuffer, true);

            return encodeBuffer.Data.Length;
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor descriptor)
        {
            return 1;
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor descriptor)
        {
            return null;
        }

        public void Marshal(Marshaler marshaler, MarshalingDescriptor descriptor, object value)
        {
            if (marshaler == null)
            {
                throw new ArgumentNullException("marshaler");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Asn1Object asnValue = value as Asn1Object;
            if (asnValue == null)
            {
                throw new ArgumentException("The type of Asn.1 value must be derived from 'Asn1Type'");
            }

            Asn1BerEncodingBuffer encodeBuffer = CreateEncodeBuffer();
            asnValue.BerEncode(encodeBuffer, true);

            foreach (byte b in encodeBuffer.Data)
            {
                marshaler.WriteByte(b);
            }
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor descriptor)
        {
            if (marshaler == null)
            {
                throw new ArgumentNullException("marshaler");
            }

            Asn1Object value = (Asn1Object)Activator.CreateInstance(descriptor.Type);
            Asn1BufferedStream stream = new Asn1BufferedStream(marshaler);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            Asn1DecodingBuffer decodeBuffer = CreateDecodeBuffer(bytes);
            value.BerDecode(decodeBuffer);

            return value;
        }

        protected virtual Asn1BerEncodingBuffer CreateEncodeBuffer()
        {
            return new Asn1BerEncodingBuffer();
        }

        protected virtual Asn1DecodingBuffer CreateDecodeBuffer(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            return new Asn1DecodingBuffer(bytes);
        }
    }
    #endregion

    #region DER marshaler
    /// <summary>
    /// A marshaler used to marshal Asn.1 type by DER.
    /// </summary>
    internal class Asn1DerMarshaler : Asn1BerMarshaler
    {
        //DER is the same as BER 
    }
    #endregion

    #region PER marshaler
    /// <summary>
    /// A marshaler used to marshal Asn.1 type by PER.
    /// </summary>
    internal class Asn1PerMarshaler : ITypeMarshaler
    {
        public int GetSize(Marshaler marshaler, MarshalingDescriptor descriptor, object value)
        {
            if (marshaler == null)
            {
                throw new ArgumentNullException("marshaler");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Asn1Object asnValue = value as Asn1Object;
            if (asnValue == null)
            {
                throw new ArgumentException("The type of Asn.1 value must be derived from 'Asn1Type'");
            }

            Asn1PerEncodingBuffer encodeBuffer = new Asn1PerEncodingBuffer(true);
            asnValue.PerEncode(encodeBuffer);

            return encodeBuffer.ByteArrayData.Length;
        }

        public int GetAlignment(Marshaler marshaler, MarshalingDescriptor descriptor)
        {
            return 1;
        }

        public Type GetNativeType(Marshaler marshaler, MarshalingDescriptor descriptor)
        {
            return null;
        }

        public void Marshal(Marshaler marshaler, MarshalingDescriptor descriptor, object value)
        {
            if (marshaler == null)
            {
                throw new ArgumentNullException("marshaler");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Asn1Object asnValue = value as Asn1Object;
            if (asnValue == null)
            {
                throw new ArgumentException("The type of Asn.1 value must be derived from 'Asn1Type'");
            }

            Asn1PerEncodingBuffer encodeBuffer = new Asn1PerEncodingBuffer(IsAligned(descriptor));
            asnValue.PerEncode(encodeBuffer);

            foreach (byte b in encodeBuffer.ByteArrayData)
            {
                marshaler.WriteByte(b);
            }
        }

        public object Unmarshal(Marshaler marshaler, MarshalingDescriptor descriptor)
        {
            Asn1Object value = (Asn1Object)Activator.CreateInstance(descriptor.Type);
            Asn1BufferedStream stream = new Asn1BufferedStream(marshaler);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            Asn1DecodingBuffer decodeBuffer = new Asn1DecodingBuffer(bytes);
            value.PerDecode(decodeBuffer, IsAligned(descriptor));
            return value;
        }

        protected static bool IsAligned(MarshalingDescriptor descriptor)
        {
            bool isAligned;

            if (MarshalingDescriptor.TryGetAttribute<bool>(descriptor.Attributes, typeof(AlignedAttribute), "aligned", out isAligned)
                || MarshalingDescriptor.TryGetAttribute<bool>(descriptor.Type, typeof(AlignedAttribute), "aligned", out isAligned))
            {
                return isAligned;
            }
            else
            {
                return true;
            }
        }
    }
    #endregion

    #endregion
}
