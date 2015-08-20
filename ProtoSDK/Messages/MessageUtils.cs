// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Runtime.Marshaling;
using System.Globalization;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages
{
    /// <summary>
    /// A class which provides various utilities for dealing with message data structures, 
    /// like initialization, validation, and so on. 
    /// This class can be customized by extending it and adding methods for particular message types.
    /// </summary>
    public class MessageUtils : IDisposable
    {
        Marshaler marshaler;
        IRuntimeHost host;
        bool rejectNullValue;
        bool disableValidation;
        IDictionary<string, object> symbolStore;

        Dictionary<Type, List<MethodInfo>> initializers = new Dictionary<Type, List<MethodInfo>>();
        Dictionary<Type, List<MethodInfo>> validators = new Dictionary<Type, List<MethodInfo>>();

        private object messageUtilsLock = new object();
        private const int BASE_HEX = 16;
        private const int BASE_BIN = 2;
        private const int BASE_DEC = 10;
        private const string BASE_HEX_PREFIX = "0x";
        private const string BASE_BIN_PREFIX = "0b";
        private const int PARAMETER_NUM = 2;

        /// <summary>
        /// Constructs an instance of this class by using passed test site and default marshaling
        /// configuration for block protocols.
        /// </summary>
        /// <param name="host">The message runtime host</param>
        public MessageUtils(IRuntimeHost host)
            : this(host, BlockMarshalingConfiguration.Configuration)
        {
        }

        /// <summary>
        /// Enum use for indicating the type of message, block or rpc.
        /// </summary>
        public enum MessageType
        {
            /// <summary>
            /// Indicates a Block message.
            /// </summary>
            Block,

            /// <summary>
            /// Indicates a Rpc message.
            /// </summary>
            Rpc
        }

        /// <summary>
        /// Constructs an instance of this class by using passed test site and given marshaling configuration.
        /// </summary>
        /// <param name="host">The message runtime host</param>
        /// <param name="config">The marshaling configuration</param>
        public MessageUtils(IRuntimeHost host, MarshalingConfiguration config)
        {
            this.marshaler = new Marshaler(host, config);
            this.host = host;
            disableValidation = IsDisableValidation();
            InitMethodTable(initializers, typeof(InitializerAttribute), true);
            InitMethodTable(validators, typeof(ValidatorAttribute), false);
        }

        /// <summary>
        /// Constructs an instance of this class by using passed test site and message type.
        /// </summary>
        /// <param name="host">The message runtime host</param>
        /// <param name="type">The message type, rpc or block</param>
        public MessageUtils(IRuntimeHost host, MessageType type)
        {
            if (type == MessageType.Block)
            {
                this.marshaler = new Marshaler(host, BlockMarshalingConfiguration.Configuration);
            }
            else if (type == MessageType.Rpc)
            {
                this.marshaler = new Marshaler(host, NativeMarshalingConfiguration.Configuration);
            }
            else
            {
                throw new NotSupportedException("Current constructor does not support this message type, need fix");
            }
            this.host = host;
            disableValidation = IsDisableValidation();
            InitMethodTable(initializers, typeof(InitializerAttribute), true);
            InitMethodTable(validators, typeof(ValidatorAttribute), false);
        }

        /// <summary>
        /// Returns message runtime host which is associated with this class.
        /// </summary>
        public IRuntimeHost RuntimeHost
        {
            get
            {
                return host;
            }
        }

        /// <summary>
        /// Indicates whether report error when the value to be checked is null.
        /// </summary>
        public bool RejectNullValue
        {
            get { return rejectNullValue; }
            set { rejectNullValue = value; }
        }

        /// <summary>
        /// Returns the size (in bytes) of the given message value.
        /// </summary>
        /// <param name="value">The value which size is to be calculated.</param>
        /// <returns>The size (in bytes) of the given value</returns>
        public int GetSize(object value)
        {
            lock (messageUtilsLock)
            {
                return marshaler.GetSize(value);
            }
        }

        /// <summary>
        /// Creates a value of type T and calling <see cref="Initialize"/> on it.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>The initialized value.</returns>
        public T Create<T>()
        {
            T value = default(T);
            Initialize(ref value);
            return value;
        }

        /// <summary>
        /// Initializes a value of type T which uses defaults derived from types and attributes.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to be initialized.</param>
        /// <returns>The initialized value.</returns>
        /// <remarks>
        /// This method initializes a value of the given type as follows:
        /// <list type="bullet">
        ///   <item>
        ///     Fields of primitive types keep their current value (which is the standard
        ///     default value if the initialized value is freshly created)
        ///   </item>
        ///   <item>
        ///     Fields of enumeration types obtain the first value in the enumeration order.
        ///   </item>
        ///   <item>
        ///     Fields of arrays or sequences with size attributes are initialized to that size,
        ///     with elements having their standard default values. Any array or sequence
        ///     with size attribute has a non-null value after initialization.
        ///   </item>
        ///   <item>
        ///     After the above steps, the method calls any initializer method 
        ///     in the receiver's type which is tagged with the <see cref="InitializerAttribute"/> and
        ///     which has the parameter type <code> ref T</code>. Those methods can provide custom 
        ///     initialization code specific to the type T.
        ///   </item>
        /// </list>
        /// </remarks>
        // Suppress this fxcop warning to prevent the change of the method.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
        public void Initialize<T>(ref T value)
        {
            lock (messageUtilsLock)
            {
                object obj = (object)value;
                Type t = typeof(T);
                marshaler.EnterContext();
                foreach (FieldInfo field in
                                t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    Type ft = field.FieldType;
                    if (ft.IsEnum)
                    {
                        bool exclusive = !MarshalingDescriptor.HasAttribute(ft, typeof(FlagsAttribute));

                        if (exclusive)
                        {
                            FieldInfo[] df =
                                ft.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                            if (df != null && df.Length > 0)
                            {
                                field.SetValue(obj, df[0].GetRawConstantValue());
                            }
                        }
                    }
                    else if (ft.IsArray)
                    {
                        int size, length;
                        marshaler.GetAdvocatedSize(new MarshalingDescriptor(ft, field), out size, out length);
                        if (size >= 0)
                        {
                            if (ft.IsArray)
                            {
                                field.SetValue(obj, Array.CreateInstance(ft.GetElementType(), size));
                            }
                            else
                            {
                                field.SetValue(obj, CreateSequence(ft.GetGenericArguments()[0], size));
                            }
                        }
                    }
                    marshaler.DefineSymbol(field.Name, field.GetValue(obj));
                }
                marshaler.ExitContext();

                List<MethodInfo> ms;
                if (initializers.TryGetValue(t, out (ms)))
                {
                    foreach (MethodInfo m in ms)
                    {
                        object[] args = new object[] { obj };
                        m.Invoke(this, args);
                        obj = args[0];
                    }
                }

                value = (T)obj;
            }
        }

        /// <summary>
        /// Validates a value of type T. 
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to be validated.</param>
        /// <remarks>
        /// This method validates an instance of the given type
        /// as follows:
        /// <list type="bullet">
        ///   <item>
        ///     Fields with enumeration type are checked whether their value is in the domain
        ///     of the enumeration.
        ///   </item>
        ///   <item>
        ///     Arrays or sequences are checked whether they are non-null, and whether size and length
        ///     attribute are consistent.
        ///   </item>
        ///   <item>
        ///     After the above steps, the method calls any validator method 
        ///     in the receiver's type which is tagged with the <see cref="ValidatorAttribute"/> and
        ///     which has the parameter type <code>ref T</code>. Those methods can provide custom 
        ///     validation code specific to the type T.
        ///   </item>
        /// </list>
        /// </remarks>
        public void Validate<T>(T value)
        {
            Validate((object)value);
        }

        /// <summary>
        /// Validates the value of the object 
        /// </summary>
        /// <param name="value">The object value to be validated</param>
        /// <param name="provider">The custom attribute provider</param>
        /// <param name="symbolStore">The dictionary of symbols</param>
        /// <remarks>
        /// This method validates an instance of the given type
        /// as follows:
        /// <list type="bullet">
        ///   <item>
        ///     Fields with enumeration type are checked whether their value is in the domain
        ///     of the enumeration.
        ///   </item>
        ///   <item>
        ///     Arrays or sequences are checked whether they are non-null, and whether size and length
        ///     attribute are consistent.
        ///   </item>
        ///   <item>
        ///     After the above steps, the method calls any validator method 
        ///     in the receiver's type which is tagged with the <see cref="ValidatorAttribute"/> and
        ///     which has the parameter type <code>ref T</code>. Those methods can provide custom 
        ///     validation code specific to the type T.
        ///   </item>
        /// </list>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.MSInternal", "CA1803:AvoidCostlyCallsWherePossible")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public void Validate(object value, ICustomAttributeProvider provider, IDictionary<string, object> symbolStore)
        {
            lock (messageUtilsLock)
            {
                this.symbolStore = symbolStore;

                if (DisableValidation)
                {
                    return;
                }

                if (value == null)
                {
                    if (rejectNullValue)
                        host.Assert(false, "value to be checked must not be null");
                    else
                        return;
                }

                Type type = value.GetType();
                if (type.IsArray)
                {
                    Array array = (Array)value;
                    foreach (object element in array)
                    {
                        ValidateValue(element, provider);
                    }
                }
                else
                {
                    ValidateValue(value, provider);
                }
            }
        }

        /// <summary>
        /// Validates the value of the object.
        /// </summary>
        /// <param name="value">The object to be validated.</param>
        public void Validate(object value)
        {
            Validate(value, null, null);
        }

        private void ValidateValue(object value, ICustomAttributeProvider provider)
        {
            if (value == null)
            {
                if (rejectNullValue)
                    host.Assert(false, "value to be checked must not be null");
                else
                    return;
            }

            CheckEnum(value);

            object obj = (object)value;
            Type t = value.GetType();

            marshaler.EnterContext();
            FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            marshaler.DefineSymbols(fields, obj);
            if (symbolStore != null)
            {
                foreach (var symbolEntry in symbolStore)
                {
                    marshaler.DefineSymbol(symbolEntry.Key, symbolEntry.Value);
                }
            }
            foreach (FieldInfo field in fields)
            {
                if (field.GetValue(obj) == null)
                {
                    if (rejectNullValue)
                        host.Assert(false, "value of field '{0}' in the type '{1}' must not be null", field.Name, t.Name);
                    else
                        return;
                }

                Type ft = field.FieldType;

                if (ft.IsArray)
                {
                    int size, length;

                    IList<int> sizes, lengths;
                    marshaler.GetMultipleAdvocatedSizes(new MarshalingDescriptor(ft, field, t), out sizes, out lengths);

                    size = sizes[0];
                    length = lengths[0];

                    object fv = field.GetValue(obj);
                    if (fv != null)
                    {
                        int actualSize;
                        if (ft.IsArray)
                            actualSize = ((Array)fv).GetLength(0);
                        else
                            actualSize = GetSequenceLength(ft, fv);
                        if (actualSize > size)
                            host.Assert(false,
                                "validating value of type '{0}': advocated size of field '{1} is smaller than the actual size (advocated == {2}, actual == {3}",
                                t, field, size, actualSize);
                    }
                }
            }

            CheckPossibleValue(value, provider);

            marshaler.ExitContext();

            List<MethodInfo> ms;
            if (validators.TryGetValue(t, out (ms)))
            {
                foreach (MethodInfo m in ms)
                {
                    object[] args = new object[] { obj };
                    try
                    {
                        m.Invoke(this, args);
                    }
                    catch (TargetInvocationException e)
                    {
                        throw e.InnerException;
                    }
                    if (m.GetParameters()[0].ParameterType.IsByRef)
                        obj = args[0];
                }
            }
        }

        /// <summary>
        /// Checks if the value is one of the enum values.
        /// </summary>
        /// <returns>Indicates if the value is in enum.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.MSInternal", "CA1803:AvoidCostlyCallsWherePossible")]
        private bool CheckEnum(object value)
        {
            object obj = (object)value;
            Type t = value.GetType();

            if (t.IsEnum)
            {
                if (!CheckValueByEnum(value, t))
                {
                    if (host != null)
                    {
                        host.Assert(false,
                            "validating value of type '{0}': value of field '{1}' must be in domain of its type",
                            t, value.ToString());
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckPossibleValue(object value, ICustomAttributeProvider provider)
        {
            if (value == null)
            {
                if (rejectNullValue)
                    host.Assert(false, "value to be checked must not be null");
                else
                    return;
            }

            Type type = value.GetType();

            if (type.Name == "Guid")
            {
                return;
            }

            if (IsStruct(type))
            {
                FieldInfo[] myFields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo fieldInfo in myFields)
                {
                    Object valueOfField = fieldInfo.GetValue(value);

                    if (IsStruct(fieldInfo.FieldType))
                        CheckPossibleValue(valueOfField, fieldInfo);
                    else if (IsUnion(fieldInfo.FieldType))
                    {
                        CheckPossibleValueInUnion(valueOfField, fieldInfo);
                    }

                    if (HasPossibleValue(fieldInfo))
                    {
                        CheckFieldValue(valueOfField, fieldInfo);
                    }
                }

                LogRequirement(value);
            }
            else if (IsUnion(type))
            {
                CheckPossibleValueInUnion(value, provider);
            }
        }

        private void CheckPossibleValueInUnion(object value, ICustomAttributeProvider provider)
        {
            string switchExpr;
            if (provider == null || !MarshalingDescriptor.TryGetAttribute<string>(
                provider, typeof(SwitchAttribute), "expr",
                out switchExpr))
            {
                throw new InvalidOperationException(String.Format(
                    CultureInfo.InvariantCulture, 
                    "cannot find switch attribute for union '{0}'",
                    value.GetType().Name));
            }

            Type type = value.GetType();
            var fieldsToCheck = new List<FieldInfo>();
            var defaultFields = new List<FieldInfo>();
            foreach (var field in type.GetFields())
            {
                string caseExpr;


                if (MarshalingDescriptor.TryGetAttribute<string>(field,
                    typeof(CaseAttribute), "expr", out caseExpr))
                {
                    IList<int?> caseValues = marshaler.EvaluateConstant(caseExpr);

                    int switchValue = marshaler.Evaluate(switchExpr);

                    if (caseValues.Contains(switchValue))
                    {
                        fieldsToCheck.Add(field);
                    }
                }
                else if (field.GetCustomAttributes(typeof(CaseDefaultAttribute), false).Length != 0)
                {
                    defaultFields.Add(field);
                }
            }

            if (fieldsToCheck.Count == 0)
            {
                fieldsToCheck.AddRange(defaultFields);
            }

            foreach (var field in fieldsToCheck)
            {
                object fieldValue = field.GetValue(value);
                if (IsStruct(field.GetType()))
                    CheckPossibleValue(fieldValue, field);
                else if (HasPossibleValue(field))
                {
                    CheckFieldValue(fieldValue, field);
                }
            }
        }

        private static bool IsStruct(Type type)
        {
            return !type.IsPrimitive && type.IsValueType && !type.IsEnum
                && !IsUnion(type);
        }

        private static bool IsUnion(Type type)
        {
            return type.GetCustomAttributes(typeof(UnionAttribute), false).Length > 0;
        }

        private static bool HasPossibleValue(FieldInfo fieldInfo)
        {
            return fieldInfo.IsDefined(typeof(PossibleValueAttribute), false)
                    || fieldInfo.IsDefined(typeof(PossibleValueRangeAttribute), false)
                    || fieldInfo.FieldType.IsEnum;
        }

        private void CheckFieldValue(Object valueOfField, FieldInfo fieldInfo)
        {
            Type type = valueOfField.GetType();
            bool checkSucceed = false;

            if (type.IsEnum)
            {
                checkSucceed = CheckValueByEnum(valueOfField, type);
            }

            PossibleValueRangeAttribute[] possibleValueRangeAttributes =
                        (PossibleValueRangeAttribute[])Attribute.GetCustomAttributes((MemberInfo)fieldInfo, typeof(PossibleValueRangeAttribute));

            foreach (PossibleValueRangeAttribute possibleValueRangeAttribute in possibleValueRangeAttributes)
            {
                if (checkSucceed)
                    break;

                checkSucceed = CheckValueByRange(valueOfField, possibleValueRangeAttribute);
            }

            PossibleValueAttribute possibleValueAttribute =
                        (PossibleValueAttribute)Attribute.GetCustomAttribute((MemberInfo)fieldInfo, typeof(PossibleValueAttribute));

            if (!checkSucceed && possibleValueAttribute != null)
            {
                checkSucceed = CheckValueByEnum(valueOfField, possibleValueAttribute.EnumType);
            }

            if (!checkSucceed)
            {
                if (host != null)
                {
                    host.Assert(false, "Validating value of type '{0}': value '{1}' of field '{2}.{3}' must be in domain of its type",
                         type, valueOfField.ToString(), fieldInfo.DeclaringType.ToString(), fieldInfo.Name);
                }
            }
            else if (fieldInfo.IsDefined(typeof(RequirementAttribute), false))
            {
                LogRequirement(fieldInfo);
            }
        }

        private void LogRequirement(FieldInfo fieldInfo)
        {
            RequirementAttribute[] requirementAttributes =
                (RequirementAttribute[])fieldInfo.GetCustomAttributes(typeof(RequirementAttribute), false);
            foreach (RequirementAttribute requirementAttribute in requirementAttributes)
            {
                if (host != null)
                {
                    host.CaptureRequirement(requirementAttribute.ProtocolDocName,
                        requirementAttribute.RequirementID, requirementAttribute.Description);
                }
            }
        }

        private void LogRequirement(object value)
        {

            RequirementAttribute requirementAttribute = (RequirementAttribute)GetAttributeFrom(value, typeof(RequirementAttribute).Name);
            if (requirementAttribute != null && host != null)
            {
                host.CaptureRequirement(requirementAttribute.ProtocolDocName, requirementAttribute.RequirementID,
                    requirementAttribute.Description);
            }
        }

        private static Attribute GetAttributeFrom(object value, String inAttribName)
        {
            Type type = value.GetType();
            Object[] getAttributes = type.GetCustomAttributes(true);

            foreach (Object attribute in getAttributes)
            {
                Attribute getAttribute = (Attribute)attribute;
                String getAttribName = ((MemberInfo)(((Attribute)getAttribute).TypeId)).Name;
                if (getAttribName == inAttribName)
                {
                    return getAttribute;
                }
            }

            return null;
        }

        /// <summary>
        /// Check if the given value is defined in the specified enumeration.
        /// </summary>
        /// <param name="value">The given value</param>
        /// <param name="type">The specified enumeration</param>
        /// <returns>True for yes; False for no.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.MSInternal", "CA1803:AvoidCostlyCallsWherePossible")]
        public bool CheckValueByEnum(Object value, Type type)
        {
            object obj = (object)value;
            Type t = type;

            bool exclusive = !MarshalingDescriptor.HasAttribute(t, typeof(FlagsAttribute));

            if (exclusive)
            {
                try
                {
                    if (!Enum.IsDefined(t, obj))
                    {
                        return false;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    if (host != null)
                        host.Assert(false, ex.ToString());
                }
            }
            else
            {
                object x = value;

                foreach (object v in Enum.GetValues(t))
                {
                    x = ExcludeBits(x, v);
                }

                if (!HasNoBits(x))
                {
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        /// Check if the given value is within the possible value range.
        /// </summary>
        /// <param name="inValue">The given value</param>
        /// <param name="possibleValueAttribute">The possible value range</param>
        /// <returns>True for yes; False for no.</returns>
        public static bool CheckValueByRange(object inValue, PossibleValueRangeAttribute possibleValueAttribute)
        {
            String typeName = inValue.GetType().Name;
            bool checkResult;
            switch (typeName)
            {
                case "Char":
                    checkResult = CheckRange<Char>(inValue, possibleValueAttribute);
                    break;
                case "Byte":
                    checkResult = CheckRange<Byte>(inValue, possibleValueAttribute);
                    break;
                case "SByte":
                    checkResult = CheckRange<SByte>(inValue, possibleValueAttribute);
                    break;
                case "Int16":
                    checkResult = CheckRange<Int16>(inValue, possibleValueAttribute);
                    break;
                case "UInt16":
                    checkResult = CheckRange<UInt16>(inValue, possibleValueAttribute);
                    break;
                case "Int32":
                    checkResult = CheckRange<Int32>(inValue, possibleValueAttribute);
                    break;
                case "UInt32":
                    checkResult = CheckRange<UInt32>(inValue, possibleValueAttribute);
                    break;
                case "Int64":
                    checkResult = CheckRange<Int64>(inValue, possibleValueAttribute);
                    break;
                case "UInt64":
                    checkResult = CheckRange<UInt64>(inValue, possibleValueAttribute);
                    break;
                case "Double":
                    checkResult = CheckRange<Double>(inValue, possibleValueAttribute);
                    break;
                case "Single":
                    checkResult = CheckRange<Single>(inValue, possibleValueAttribute);
                    break;
                case "Decimal":
                    checkResult = CheckRange<Decimal>(inValue, possibleValueAttribute);
                    break;
                default:
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "{0} data type is not supported", typeName));
            }

            return checkResult;
        }

        private static bool CheckRange<T>(object inValue, PossibleValueRangeAttribute possibleValue)
                           where T : IComparable<T>
        {
            try
            {
                T maxValue = (T)GetValueFromString(typeof(T), possibleValue.MaxValue);
                T minValue = (T)GetValueFromString(typeof(T), possibleValue.MinValue);
                T checkingValue = (T)inValue;

                if (maxValue.CompareTo(minValue) < 0)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "max value: {0} could not be less than min value: {1}", maxValue.ToString(), minValue.ToString()));
                }

                if ((checkingValue.CompareTo(maxValue) <= 0 && checkingValue.CompareTo(minValue) >= 0))
                {
                    return true;
                }
            }
            catch (FormatException ex)
            {

                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Format exception happens when convert {0} or {1} to {2}, information: {3}",
                             possibleValue.MinValue, possibleValue.MaxValue,
                             inValue.GetType().Name, ex.ToString()));
            }
            catch (OverflowException ex)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "{0} or {1} overflowed as {2}, information: {3}", possibleValue.MinValue,
                        possibleValue.MaxValue, inValue.GetType().Name, ex.ToString()));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Error happens when check value range, information: {0}", ex.ToString()));
            }

            return false;
        }

        static object GetValueFromString(Type dataType, string inputValue)
        {
            inputValue = inputValue.Trim().ToLower(CultureInfo.InvariantCulture);

            if (dataType == typeof(Double))
            {
                return (object)Double.Parse(inputValue, CultureInfo.InvariantCulture);
            }

            if (dataType == typeof(Single))
            {
                return (object)Single.Parse(inputValue, CultureInfo.InvariantCulture);
            }

            if (dataType == typeof(Decimal))
            {
                return (object)Decimal.Parse(inputValue, CultureInfo.InvariantCulture);
            }

            bool isChar = false;
            if (dataType == typeof(Char))
            {
                dataType = typeof(UInt16);
                isChar = true;
            }

            Type[] parameterTypes = new Type[PARAMETER_NUM];
            parameterTypes[0] = typeof(string);
            parameterTypes[1] = typeof(int);
            object[] parameters = new object[PARAMETER_NUM];

            string methodName = "To" + dataType.Name;
            Type typeConvert = typeof(Convert);
            MethodInfo convertMethod = typeConvert.GetMethod(methodName, parameterTypes);

            if (inputValue.StartsWith(BASE_HEX_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                parameters[0] = (object)inputValue.Substring(BASE_HEX_PREFIX.Length);
                parameters[1] = (object)(BASE_HEX);
            }
            else if (inputValue.StartsWith(BASE_BIN_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                parameters[0] = (object)inputValue.Substring(BASE_BIN_PREFIX.Length);
                parameters[1] = (object)(BASE_BIN);
            }
            else
            {
                parameters[0] = (object)inputValue;
                parameters[1] = (object)(BASE_DEC);
            }

            if (isChar)
            {
                UInt16 returnValue = (UInt16)convertMethod.Invoke(null, parameters);
                return (char)returnValue;
            }

            return convertMethod.Invoke(null, parameters);

        }

        /// <summary>
        /// Provides a printed presentation of the message value passed.
        /// </summary>
        /// <param name="value">The value which content is to be dumped as a string.</param>
        /// <returns>The string presentation of the value</returns>
        public string ToString(object value)
        {
            if (value == null)
                return "(null)";

            Type t = value.GetType();
            MethodInfo toString = t.GetMethod("ToString", new Type[0]);
            if (toString != null && toString.DeclaringType != typeof(object)
                                 && toString.DeclaringType != typeof(ValueType)
                                 && !t.IsEnum)
                return value.ToString();

            StringBuilder b = new StringBuilder();

            if (t.IsArray)
            {
                Array avalue = (Array)value;
                b.Append(t.GetElementType().Name);
                b.Append("[]{");
                bool first = true;
                int max = 12;
                foreach (object elem in avalue)
                {
                    if (first)
                        first = false;
                    else
                        b.Append(",");
                    if (max-- == 0)
                    {
                        b.Append("..");
                        break;
                    }
                    b.Append(ToString(elem));
                }
                b.Append("}");
            }
            else if (t.IsEnum)
            {
                b.Append(value.ToString());
                try
                {
                    object uvalue = Convert.ChangeType(value, Enum.GetUnderlyingType(t), CultureInfo.InvariantCulture);
                    b.AppendFormat("({0})", uvalue);
                }
                catch (ArgumentNullException)
                { }
                catch (InvalidCastException)
                { }
                catch (ArgumentOutOfRangeException)
                { }
                catch (FormatException)
                { }
            }
            else
            {
                string sname = t.Name;
                for (; ; )
                {
                    int i = sname.IndexOfAny(new char[] { '.', '<' });
                    if (i < 0) break;
                    if (sname[i] == '.')
                        sname = sname.Substring(i + 1);
                    else
                        break;
                }
                b.Append(sname);
                b.Append("(");
                bool first = true;
                foreach (FieldInfo field in
                    t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (first)
                        first = false;
                    else
                        b.Append(",");
                    b.Append(field.Name);
                    b.Append("=");
                    b.Append(ToString(field.GetValue(value)));
                }
                b.Append(")");
            }
            return b.ToString();
        }

        #region Helpers

        static object CreateSequence(Type seqType, int size)
        {
            Type elemType = seqType.GetGenericArguments()[0];
            ConstructorInfo ctor = seqType.GetConstructor(
                                    new Type[]{ typeof(IEnumerable<int>)
                                                    .GetGenericTypeDefinition()
                                                    .MakeGenericType(elemType) });
            return ctor.Invoke(new object[] { Array.CreateInstance(elemType, size) });
        }

        static int GetSequenceLength(Type seqType, object seqValue)
        {
            MethodInfo count = seqType.GetMethod("get_Count");
            return (int)count.Invoke(seqValue, null);
        }

        void InitMethodTable(Dictionary<Type, List<MethodInfo>> table, Type attribute, bool byRef)
        {
            foreach (MethodInfo info in
                this.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (MarshalingDescriptor.HasAttribute(info, attribute))
                {
                    ParameterInfo[] ps = info.GetParameters();
                    if (ps.Length == 1)
                    {
                        Type pt = ps[0].ParameterType;
                        if (!byRef || byRef && pt.IsByRef && !ps[0].IsOut)
                        {
                            if (byRef)
                                pt = pt.GetElementType();
                            List<MethodInfo> ms;
                            if (!table.TryGetValue(pt, out ms))
                                table[pt] = ms = new List<MethodInfo>();
                            ms.Add(info);
                        }
                    }
                }
            }
        }

        static object ExcludeBits(object target, object enumValue)
        {
            ulong targetVal = (ulong)Convert.ChangeType(target, TypeCode.UInt64, CultureInfo.InvariantCulture);
            ulong enumVal = (ulong)Convert.ChangeType(enumValue, TypeCode.UInt64, CultureInfo.InvariantCulture);

            return target = ~enumVal & targetVal;
        }

        static bool HasNoBits(object target)
        {
            ulong targetVal = (ulong)Convert.ChangeType(target, TypeCode.UInt64, CultureInfo.InvariantCulture);

            return targetVal == 0 ? true : false;
        }

        #endregion

        /// <summary>
        /// Implements <see cref="IDisposable.Dispose"/>
        /// </summary>
        [SecurityPermission(SecurityAction.Demand)]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes this object
        /// </summary>
        [SecurityPermission(SecurityAction.Demand)]
        ~MessageUtils()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        /// <remarks>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If the parameter 'disposing' equals true, the method is called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If the parameter 'disposing' equals false, the method is called by the 
        /// runtime from the inside of the finalizer and you should not refer to 
        /// other objects. Therefore, only unmanaged resources can be disposed.
        /// </remarks>
        /// <param name="disposing">Indicates if Dispose is called by user.</param>
        [SecurityPermission(SecurityAction.Demand)]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (marshaler != null)
                {
                    marshaler.Dispose();
                    marshaler = null;
                }
            }
        }

        /// <summary>
        /// Indicates whether disable validation of the value.
        /// </summary>
        public virtual bool DisableValidation
        {
            get
            {
                return disableValidation;
            }
        }

        bool IsDisableValidation()
        {
            if (RuntimeHost == null)
            {
                return false;
            }

            return RuntimeHost.DisableValidation;
        }
    }
}
