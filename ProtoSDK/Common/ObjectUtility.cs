// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// Supply function for the following jobs:
    /// 1. Get a Clone of one object. it will copy every fields of the object, if the 
    /// field is reference type, it will try to alloc new memory and copy the real data.
    /// 
    /// 2. Compare two object. reference type is compared based on the real data alloced
    /// in memory.
    /// 
    /// 3. create internal, private type of object in an assembly. 
    /// 
    /// 4  supply methods for private access to one object
    /// </summary>
    public static class ObjectUtility
    {
        private static readonly Type RuntimeType = Type.GetType("System.RuntimeType");

        private const BindingFlags GetFieldsFlags =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private const BindingFlags GetFieldFlags =
            BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private const BindingFlags SetFieldFlags =
            BindingFlags.SetField | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private const BindingFlags CreateInstanceFlags =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private const BindingFlags InvokeInstanceMemberFlags =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private const BindingFlags InvokeStaticMemberFlags =
            BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

        private const BindingFlags InvokeStaticOrInstanceMemberFlags =
            BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

        #region Public methods
        /// <summary>
        /// Clones an object. It copies all fields in the object along the class hierarchy. 
        /// Reflection types like "MemberInfo, MethodInfo, FieldInfo", etc., as well as cross-reference types, are not
        /// supported.
        /// </summary>
        /// <remarks>
        /// Note it performs deep copy, and supports all types and types nested EXCEPT that types with parameterized 
        /// constructor are NOT supported. Also be aware that if you intend to change class states in the constructor,
        /// please do not use this method as it will invoke constructor multiple times, which may causes the states 
        /// vary. Also, if a "pure" object is passed in (meaning that no boxing or cast from some type to object), the
        /// method actually does NOTHING but returns a newly created object, which, if compared by ReferenceEqual, will
        /// return false.
        /// 
        /// A cross-reference type example: 
        ///     class1.child = new class2();
        ///     class1.child.parent = class1;
        /// This will result in infinite comparisons until stack overflows.
        /// </remarks>
        /// <param name="instance">The type instance.</param>
        /// <returns>A deep copy of the instance.</returns>
        public static object DeepClone(object instance)
        {
            if (instance == null)
            {
                return instance;
            }

            Type instanceType = instance.GetType();

            // Special types, copy directly because a recursive copy of them may result in 
            // copying of a RuntimeType, which CANNOT be created dynamically; and String doesn't provide 
            // a parameterless constructor, it's actually treated as primitive types.
            // Note that structures must be copied recursively.
            if (instanceType.IsPrimitive ||
                instanceType == typeof(Type) ||
                instanceType == typeof(string) ||
                instanceType == RuntimeType) // RuntimeType is an internal class
            {
                return instance;
            }
            else if (instanceType.IsArray)
            {
                return CopyArray(instance);
            }
            else if (instanceType.IsGenericType)
            {
                return CopyGenericType(instance);
            }
            else // User defined types
            {
                // The clone object to be returned
                object cloneObject = Activator.CreateInstance(instanceType);
                CopyFields(instance, ref cloneObject);

                return cloneObject;
            }
        }


        /// <summary>
        /// Performs a deep comparison on two objects. It will compare all fields of the two objects along
        /// the class hierarchy. If the field is by reference, e.g., a class instance, or array, it will do a recursive 
        /// comparison, which compares each field of the class instance, or each element of the array.
        /// Reflection types like "MemberInfo, MethodInfo, FieldInfo", etc., as well as cross-reference types, are not
        /// supported.
        /// </summary>
        /// <remarks>
        /// Note that this method performs type comparison first, so if a long with the value 4 and an int with
        /// the same value are compared, false will be returned as they are of different types.
        /// Also, if two "pure" objects are passed in (meaning that no boxing or cast from some type to object), the 
        /// method does NOTHING but returns true while actually if the objects are compared by ReferenceEqual, false
        /// will be returned.
        /// 
        /// A cross-reference type example: 
        ///     class1.child = new class2();
        ///     class1.child.parent = class1;
        /// This will result in infinite comparisons until stack overflows.
        /// </remarks>
        /// <param name="instance1">The first instance to be compared.</param>
        /// <param name="instance2">The second instance to be compared.</param>
        /// <exception cref="InvalidOperationException">Raised if the instance is not of the input type.</exception>
        /// <returns>True if the two instances contains the same field values, false otherwise.</returns>
        public static bool DeepCompare(object instance1, object instance2)
        {
            // Both null.
            if (instance1 == null && instance2 == null)
            {
                return true;
            }

            // One of the instance is null.
            if (instance1 == null || instance2 == null)
            {
                return false;
            }

            Type instanceType = instance1.GetType();
            // Type mismatch.
            if (instanceType != instance2.GetType())
            {
                return false;
            }

            if (instanceType == typeof(Type) || instanceType == RuntimeType)
            {
                return instance1 == instance2;
            }
            else if (instanceType.IsPrimitive || instanceType == typeof(string))
            {
                return ComparePrimitiveObjects(instance1, instance2);
            }
            else if (instanceType.IsArray)
            {
                return CompareArrays(instance1, instance2);
            }
            else
            {
                return CompareFields(instance1, instance2);
            }
        }


        /// <summary>
        /// Creates an instance of specified type, with parameterized constructor.
        /// </summary>
        /// <param name="assemblyName">The assembly name that contains the type, use full path. If no full
        /// path is provided, the method will try to find the assembly in the current execution path.</param>
        /// <param name="typeName">The type name, use type name with namespace, i.e., Namespace.TypeName.</param>
        /// <param name="args">Constructor arguments. If null is passed in, it's assumed to call the parameterless
        /// constructor.</param>
        /// <exception cref="FileNotFoundException">Raised when the assembly file cannot be found.</exception>
        /// <exception cref="StackException">Raised when specified type cannot be found.</exception>
        /// <returns>The instance.</returns>
        public static object CreateInstance(string assemblyName, string typeName, object[] args)
        {
            Type type = LoadTypeFromAssembly(assemblyName, typeName);
            return Activator.CreateInstance(type, CreateInstanceFlags, null, args, null);
        }


        /// <summary>
        /// Creates an instance of specified type, with parameterless constructor.
        /// </summary>
        /// <param name="assemblyName">The assembly name that contains the type, use full path. If no full
        /// path is provided, the method will try to find the assembly in the current execution path.</param>
        /// <param name="typeName">The type name, use type name with namespace, i.e., Namespace.TypeName.</param>
        /// <exception cref="FileNotFoundException">Raised when the assembly file cannot be found.</exception>
        /// <exception cref="StackException">Raised when specified type cannot be found.</exception>
        /// <returns>The instance.</returns>
        public static object CreateInstance(string assemblyName, string typeName)
        {
            return CreateInstance(assemblyName, typeName, null);
        }


        /// <summary>
        /// Invokes an instance method.
        /// </summary>
        /// <remarks>
        /// This method doesn't verify parameter types of the target method. It just finds the matched method name, 
        /// and invokes this method. It should be used when there's no overloaded methods.
        /// </remarks>
        /// <param name="target">The target object instance.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="args">Arguments. If null is passed in, it's assumed to call the parameterless method.</param>
        /// <exception cref="StackException">Raised when the method is not found.</exception>
        /// <returns>Invocation result. The value can be ignored if a method is "void".</returns>
        public static object InvokeMethod(object target, string methodName, object[] args)
        {
            return InvokeMethod(target, methodName, null, args);
        }


        /// <summary>
        /// Invokes an instance method.
        /// </summary>
        /// <remarks>
        /// This method verifies parameter types of the target method. It will do an exact match of the parameter types
        /// If there's no match, an exception will be thrown. It should be used when there're overloaded methods.
        /// </remarks>
        /// <param name="target">The target object instance.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="argTypes">The type of each parameter.</param>
        /// <param name="args">Arguments.If null is passed in, it's assumed to call the parameterless method.</param>
        /// <exception cref="StackException">Raised when the method is not found.</exception>
        /// <returns>Invocation result. The value can be ignored if a method is "void".</returns>
        public static object InvokeMethod(object target, string methodName, Type[] argTypes, object[] args)
        {
            MethodInfo method = FindMethod(target.GetType(), methodName, argTypes, false);
            return method.Invoke(target, args);
        }


        /// <summary>
        /// Invokes a static method.
        /// </summary>
        /// <remarks>
        /// This method doesn't verify parameter types of the target method. It just finds the matched method name, 
        /// and invoke this method. It should be used when there's no overloaded methods.
        /// </remarks>
        /// <param name="assemblyName">The name of assembly that contains the type. If no full
        /// path is provided, the method will try to find the assembly in the current execution path.</param>
        /// <param name="typeName">The type that contains the method, use type name with namespace, i.e., 
        /// Namespace.TypeName.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="args">Arguments.If null is passed in, it's assumed to call the parameterless method.</param>
        /// <returns>Invocation result.</returns>
        public static object InvokeStaticMethod(
            string assemblyName,
            string typeName,
            string methodName,
            object[] args)
        {
            return InvokeStaticMethod(assemblyName, typeName, methodName, null, args);
        }


        /// <summary>
        /// Invokes a static method.
        /// </summary>
        /// <remarks>
        /// This method verifies parameter types of the target method. It will do an exact match of the parameter types
        /// If there's no match, an exception will be thrown. It should be used when there're overloaded methods.
        /// </remarks>
        /// <param name="assemblyName">The name of assembly that contains the type. If no full
        /// path is provided, the method will try to find the assembly in the current execution path.</param>
        /// <param name="typeName">The type that contains the method, use type name with namespace, i.e., 
        /// Namespace.TypeName.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="argTypes">Method argument types.</param>
        /// <param name="args">Arguments.If null is passed in, it's assumed to call the parameterless method.</param>
        /// <returns>Invocation result.</returns>
        public static object InvokeStaticMethod(
            string assemblyName,
            string typeName,
            string methodName,
            Type[] argTypes,
            object[] args)
        {
            Type type = LoadTypeFromAssembly(assemblyName, typeName);
            MethodInfo method = FindMethod(type, methodName, argTypes, true);

            return method.Invoke(null, args);
        }


        /// <summary>
        /// Gets a field value of the target object.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <param name="fieldName">Field name.</param>
        /// <exception cref="StackException">Raised when the field is not found.</exception>
        /// <returns>The field value.</returns>
        public static object GetFieldValue(object target, string fieldName)
        {
            FieldInfo field = FindField(target.GetType(), fieldName);
            return field.GetValue(target);
        }


        /// <summary>
        /// Sets a field value of the target object.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <param name="fieldName">Field name.</param>
        /// <param name="fieldValue">The field value.</param>
        /// <exception cref="StackException">Raised when the field is not found.</exception>
        public static void SetFieldValue(object target, string fieldName, object fieldValue)
        {
            FieldInfo field = FindField(target.GetType(), fieldName);
            field.SetValue(target, fieldValue);
        }


        /// <summary>
        /// Sets a field value of the target structure.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <param name="fieldName">Field name.</param>
        /// <param name="fieldValue">The field value.</param>
        /// <exception cref="StackException">Raised when the field is not found.</exception>
        public static void SetFieldValue<T>(ref T target, string fieldName, object fieldValue) where T : struct
        {
            object obj = target;
            SetFieldValue(obj, fieldName, fieldValue);
            target = (T)obj;
        }


        /// <summary>
        /// Gets a field value of the static type.
        /// </summary>
        /// <param name="assemblyName">The name of assembly that contains the type. If no full
        /// path is provided, the method will try to find the assembly in the current execution path.</param>
        /// <param name="typeName">The type that contains the field, use type name with namespace, i.e., 
        /// Namespace.TypeName.</param>
        /// <param name="fieldName">The field name.</param>
        /// <exception cref="StackException">Raised when the field is not found.</exception>
        /// <returns>The field value.</returns>
        public static object GetStaticFieldValue(string assemblyName, string typeName, string fieldName)
        {
            Type type = LoadTypeFromAssembly(assemblyName, typeName);
            FieldInfo field = FindField(type, fieldName);

            return field.GetValue(null);
        }


        /// <summary>
        /// Sets a field value of the static type.
        /// </summary>
        /// <param name="assemblyName">The name of assembly that contains the type. If no full
        /// path is provided, the method will try to find the assembly in the current execution path.</param>
        ///  <param name="typeName">The type that contains the field, use type name with namespace, i.e., 
        /// Namespace.TypeName.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="fieldValue">Field value.</param>
        /// <exception cref="StackException">Raised when the field is not found.</exception>
        public static void SetStaticFieldValue(
            string assemblyName,
            string typeName,
            string fieldName,
            object fieldValue)
        {
            Type type = LoadTypeFromAssembly(assemblyName, typeName);
            FieldInfo field = FindField(type, fieldName);

            field.SetValue(null, fieldValue);
        }


        /// <summary>
        /// Gets a property value of the target object. Indexer is not supported.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>The field value.</returns>
        /// <exception cref="StackException">Raised when the field is not found.</exception>
        public static object GetPropertyValue(object target, string propertyName)
        {
            PropertyInfo property = FindProperty(target.GetType(), propertyName);
            return property.GetValue(target, null);
        }


        /// <summary>
        /// Sets a property value of the target object. Indexer is not supported.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <exception cref="StackException">Raised when the field is not found.</exception>
        public static void SetPropertyValue(object target, string propertyName, object propertyValue)
        {
            PropertyInfo property = FindProperty(target.GetType(), propertyName);
            property.SetValue(target, propertyValue, null);
        }


        /// <summary>
        /// Gets the property value of a static type.
        /// </summary>
        /// <param name="assemblyName">The name of assembly that contains the type. If no full
        /// path is provided, the method will try to find the assembly in the current execution path.</param>
        /// <param name="typeName">The name of type that contains the property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <exception cref="StackException">Raised when the field is not found.</exception>
        public static object GetStaticPropertyValue(string assemblyName, string typeName, string propertyName)
        {
            Type type = LoadTypeFromAssembly(assemblyName, typeName);
            PropertyInfo property = FindProperty(type, propertyName);

            return property.GetValue(null, null);
        }


        /// <summary>
        /// Sets the property value of a static type.
        /// </summary>
        /// <param name="assemblyName">The name of assembly that contains the type. If no full
        /// path is provided, the method will try to find the assembly in the current execution path.</param>
        ///  <param name="typeName">The type that contains the property, use type name with namespace, i.e., 
        /// Namespace.TypeName.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="propertyValue">Property value.</param>
        /// <exception cref="StackException">Raised when the field is not found.</exception>
        public static void SetStaticPropertyValue(
            string assemblyName,
            string typeName,
            string propertyName,
            object propertyValue)
        {
            Type type = LoadTypeFromAssembly(assemblyName, typeName);
            PropertyInfo property = FindProperty(type, propertyName);
            property.SetValue(null, propertyValue, null);
        }

        #endregion Public methods

        #region Helper methods
        /// <summary>
        /// Performs a deep copy on an array.
        /// </summary>
        /// <param name="array">The array to be copied.</param>
        /// <returns>The deep-copied array.</returns>
        private static object CopyArray(object array)
        {
            Type fieldType = array.GetType();
            Array originalArray = array as Array;

            int rank = originalArray.Rank;
            object[] lengthOfDim = new object[rank];
            for (int i = 0; i < lengthOfDim.Length; i++)
            {
                lengthOfDim[i] = originalArray.GetLength(i);
            }
            Array newArray = Activator.CreateInstance(fieldType, lengthOfDim) as Array;

            // Copy elements, for multi-dimension arrays, the indices must be determined first
            // e.g., array[2,3,4], for the 10th element, we count the indices to be:
            // array[10/4/3%2, 10/4%3, 10%4]. A more formal representation:
            // for array[n1, n2, ..., nn] and given index N, the indices in the array can be calculated by:
            //   array[ N/n2/n3/.../nn%n1, N/n3/n4/.../nn%n2, ..., N%nn ].
            // The code below caches the quotient for performance.
            for (int i = 0; i < originalArray.Length; i++)
            {
                int[] elementIndices = new int[rank];
                int quotient = i;

                for (int j = elementIndices.Length - 1; j >= 0; j--)
                {
                    elementIndices[j] = quotient % originalArray.GetLength(j);
                    quotient /= originalArray.GetLength(j);
                }

                // Value-type and special types, copy directly.
                if (fieldType.GetElementType().IsPrimitive ||
                    fieldType.GetElementType() == typeof(Type) ||
                    fieldType.GetElementType() == typeof(string) ||
                    fieldType.GetElementType() == RuntimeType)
                {
                    newArray.SetValue(originalArray.GetValue(elementIndices), elementIndices);
                }
                else // Reference type, perform a recursive clone
                {
                    newArray.SetValue(DeepClone(originalArray.GetValue(elementIndices)), elementIndices);
                }
            }

            return newArray;
        }


        /// <summary>
        /// Performs a deep copy on a generic object.
        /// </summary>
        /// <param name="genericObject">The generic object to be copied.</param>
        /// <returns>The deep-copied object.</returns>
        private static object CopyGenericType(object genericObject)
        {
            Type fieldType = genericObject.GetType();
            // Copy list elements
            Type cloneGenericType = fieldType.GetGenericTypeDefinition().MakeGenericType(
                fieldType.GetGenericArguments());
            object cloneGenericObj = Activator.CreateInstance(cloneGenericType);
            CopyFields(genericObject, ref cloneGenericObj);

            return cloneGenericObj;
        }


        /// <summary>
        /// Copies all fields from source object to destination object.
        /// </summary>
        /// <param name="srcObject">Source object.</param>
        /// <param name="destObject">Destination object.</param>
        private static void CopyFields(object srcObject, ref object destObject)
        {
            Type typeHierachy = srcObject.GetType();

            // Because reflection can only get private fields at the current class hierachy rather than along the class
            // hierachy, we must track down the class hierachy to get all private fields.
            while (typeHierachy != typeof(object))
            {
                // Iterate all accessiable fields 
                FieldInfo[] fields = typeHierachy.GetFields(GetFieldsFlags);

                foreach (FieldInfo field in fields)
                {
                    // Get field value
                    object fieldValue = typeHierachy.InvokeMember(
                        field.Name,
                        GetFieldFlags,
                        null,
                        srcObject,
                        null, 
                        CultureInfo.InvariantCulture);

                    object cloneFieldValue = null;
                    if (fieldValue != null)
                    {
                        cloneFieldValue = DeepClone(fieldValue);
                    }

                    typeHierachy.InvokeMember(
                        field.Name,
                        SetFieldFlags,
                        null,
                        destObject,
                        new object[] { cloneFieldValue },
                        CultureInfo.InvariantCulture
                    );
                }

                typeHierachy = typeHierachy.BaseType;
            }
        }


        /// <summary>
        /// Compares two arrays. It compares every element of the array. If the element is a class, it will
        /// compare all public fields of the element.
        /// </summary>
        /// <param name="array1">The first array.</param>
        /// <param name="array2">The second array.</param>
        /// <returns>True if all elements are equal, false otherwise.</returns>
        private static bool CompareArrays(object array1, object array2)
        {
            Array firstArray = array1 as Array;
            Array secondArray = array2 as Array;

            int rank = firstArray.Rank;
            int length = firstArray.Length;

            // Contains different elements
            if (length != secondArray.Length)
            {
                return false;
            }

            object[] lengthOfDim = new object[rank];
            for (int i = 0; i < lengthOfDim.Length; i++)
            {
                lengthOfDim[i] = firstArray.GetLength(i);

                // Contains different rank length, e.g., a 2x3 array with a 3x2 array
                if ((int)lengthOfDim[i] != secondArray.GetLength(i))
                {
                    return false;
                }
            }

            for (int i = 0; i < firstArray.Length; i++)
            {
                int[] elementIndices = new int[rank];
                int quotient = i;

                for (int j = elementIndices.Length - 1; j >= 0; j--)
                {
                    elementIndices[j] = quotient % firstArray.GetLength(j);
                    quotient /= firstArray.GetLength(j);
                }

                object value1 = firstArray.GetValue(elementIndices);
                object value2 = secondArray.GetValue(elementIndices);
                if (!DeepCompare(value1, value2))
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Compares two instances with primitive types.
        /// </summary>
        /// <param name="instance1">The first instance.</param>
        /// <param name="instance2">The second instance.</param>
        /// <exception cref="NotSupportedException">Raised if the instances don't implement IComparable, IComparer or
        /// IEqualityComparer.</exception>
        /// <returns>True if they are equal, false otherwise.</returns>
        private static bool ComparePrimitiveObjects(object instance1, object instance2)
        {
            IComparable comparable = instance1 as IComparable;
            if (comparable != null)
            {
                return comparable.CompareTo(instance2) == 0;
            }

            IComparer comparer = instance1 as IComparer;
            if (comparer != null)
            {
                return comparer.Compare(instance1, instance2) == 0;
            }

            IEqualityComparer equaler = instance1 as IEqualityComparer;
            if (equaler != null)
            {
                return equaler.Equals(instance2);
            }

            ValueType valueType = instance1 as ValueType;
            if (valueType != null)
            {
                return valueType.Equals(instance2);
            }

            throw new NotSupportedException("Not supported object type: " + instance1.GetType().FullName);
        }


        /// <summary>
        /// Compares all fields of two instances.
        /// </summary>
        /// <param name="instance1">The first instance.</param>
        /// <param name="instance2">The second instance.</param>
        /// <returns>True if all fields are equal, false otherwise.</returns>
        private static bool CompareFields(object instance1, object instance2)
        {
            Type typeHierachy = instance1.GetType();

            while (typeHierachy != typeof(object))
            {
                FieldInfo[] fields = typeHierachy.GetFields(GetFieldsFlags);

                // Iterate all fields
                foreach (FieldInfo field in fields)
                {
                    // Get field value
                    object fieldValue1 = typeHierachy.InvokeMember(
                        field.Name,
                        GetFieldFlags,
                        null,
                        instance1,
                        null,
                        CultureInfo.InvariantCulture);

                    object fieldValue2 = typeHierachy.InvokeMember(
                        field.Name,
                        GetFieldFlags,
                        null,
                        instance2,
                        null,
                        CultureInfo.InvariantCulture);

                    if (!DeepCompare(fieldValue1, fieldValue2))
                    {
                        return false;
                    }
                }

                typeHierachy = typeHierachy.BaseType;
            }

            return true;
        }


        /// <summary>
        /// Loads a type from assembly.
        /// </summary>
        /// <param name="assemblyName">The assembly name that contains the type, use full path.</param>
        /// <param name="typeName">The type name, use full name including the namespace.</param>
        /// <exception cref="FileNotFoundException">Raised when the assembly file cannot be found.</exception>
        /// <exception cref="StackException">Raised when specified type cannot be found.</exception>
        /// <returns>The type.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods")]
        private static Type LoadTypeFromAssembly(string assemblyName, string typeName)
        {
            string fullAssemblyName = assemblyName;
            if (!File.Exists(fullAssemblyName))
            {
                fullAssemblyName = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), assemblyName);
            }

            Assembly assembly = Assembly.LoadFrom(fullAssemblyName);
            Module[] modules = assembly.GetModules();

            foreach (Module module in modules)
            {
                Type[] types = module.GetTypes();
                foreach (Type type in types)
                {
                    if (type.FullName == typeName)
                    {
                        return type;
                    }
                }
            }

            throw new StackException(string.Format(CultureInfo.InvariantCulture, "Type {0} not found.", typeName));
        }


        /// <summary>
        /// Finds a method from a type.
        /// </summary>
        /// <param name="type">The type that contains the method.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="argTypes">Method argument types. If set to null, types will not be checked.</param>
        /// <param name="isStatic">Indicates if the method is static.</param>
        /// <exception cref="StackException">Raised when the method is not found.</exception>
        /// <returns>The match method.</returns>
        private static MethodInfo FindMethod(Type type, string methodName, Type[] argTypes, bool isStatic)
        {
            Type typeHierachy = type;
            while (typeHierachy != null)
            {
                // Get both instance and static methods
                MethodInfo[] methods = typeHierachy.GetMethods(InvokeStaticOrInstanceMemberFlags);

                foreach (MethodInfo method in methods)
                {
                    if (method.Name == methodName)
                    {
                        if (MatchMethodParameterTypes(method, argTypes) && method.IsStatic == isStatic)
                        {
                            return method;
                        }
                    }
                }

                typeHierachy = typeHierachy.BaseType;
            }

            throw new StackException(string.Format(CultureInfo.InvariantCulture, "Method {0} is not found.", methodName));
        }


        /// <summary>
        /// Compares method parameter types.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="argTypes">Argument types. If set to null, no check will be done.</param>
        /// <returns>True if each parameter type match exactly, false otherwise.</returns>
        private static bool MatchMethodParameterTypes(MethodInfo method, Type[] argTypes)
        {
            if (argTypes == null)
            {
                return true;
            }

            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length != argTypes.Length)
            {
                return false;
            }

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType != argTypes[i])
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Finds a field in the type.
        /// </summary>
        /// <param name="type">The type that contains the field.</param>
        /// <param name="fieldName">The field name.</param>
        /// <exception cref="StackException">Raised when the field is not found.</exception>
        /// <returns>The field.</returns>
        private static FieldInfo FindField(Type type, string fieldName)
        {
            Type typeHierachy = type;
            while (typeHierachy != null)
            {
                FieldInfo[] fields = typeHierachy.GetFields(InvokeStaticOrInstanceMemberFlags);
                foreach (FieldInfo field in fields)
                {
                    if (field.Name == fieldName)
                    {
                        return field;
                    }
                }

                typeHierachy = typeHierachy.BaseType;
            }

            throw new StackException(string.Format(CultureInfo.InvariantCulture, "Field {0} is not found.", fieldName));
        }


        /// <summary>
        /// Finds a property in the type.
        /// </summary>
        /// <param name="type">The type that contains the property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <exception cref="StackException">Raised when the property is not found.</exception>
        /// <returns>The property.</returns>
        private static PropertyInfo FindProperty(Type type, string propertyName)
        {
            Type typeHierachy = type;
            while (typeHierachy != null)
            {
                PropertyInfo[] properties = typeHierachy.GetProperties(InvokeStaticOrInstanceMemberFlags);
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name == propertyName)
                    {
                        return property;
                    }
                }

                typeHierachy = typeHierachy.BaseType;
            }

            throw new StackException(string.Format(CultureInfo.InvariantCulture, "Property {0} is not found.", propertyName));
        }

        #endregion Helper methods
    }
}
