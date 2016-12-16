// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Opn.Runtime.Values;
using System.Xml;

namespace Microsoft.Protocols.TestTools.MessageAnalyzer
{
    public class ComplexType
    {

        #region variables
        
        protected ReferenceValue referenceValue = null;

        #endregion variables

        #region Property

        /// <summary>
        /// Get the type name of a reference type 
        /// </summary>
        public string TypeName
        {
            get
            {
                return referenceValue.__ReferenceTypeName;
            }
        }

        /// <summary>
        /// Get all the field names 
        /// </summary>
        public ICollection<string> FieldNames
        {
            get
            {
                if (referenceValue != null)
                    return referenceValue.__AsDictionary().Keys;
                return null;
            }
        }

        #endregion Property

        #region Constructor

        /// <summary>
        /// Constructor used internal
        /// </summary>
        /// <param name="referenceValue">referencevalue</param>
        internal ComplexType(ReferenceValue referenceValue)
        {
            if (referenceValue == null)
            {
                throw new ArgumentNullException();
            }

            this.referenceValue = referenceValue;
        }
        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ComplexType()
        { 
        }
        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Get field value by using field name
        /// </summary>
        /// <typeparam name="T">value type</typeparam>
        /// <param name="fieldName">field name</param>
        /// <returns>field value</returns>
        public T TryGetValue<T>(string fieldName)
        {
            T value;
            if (!TryGetValue<T>(fieldName, out value))
                throw new KeyNotFoundException();
            return value;
        }

        /// <summary>
        /// Get field value by using field name
        /// </summary>
        /// <typeparam name="T">Type of Value</typeparam>
        /// <param name="fieldName">field name</param>
        /// <param name="value">value to return</param>
        /// <returns>true for succeed and false for failed</returns>
        public bool TryGetValue<T>(string fieldName, out T value)
        {
            if (referenceValue != null)
            {
                Object maValueObj;
                if (referenceValue.__AsDictionary().TryGetValue(fieldName, out maValueObj))
                {
                    Object returnValue = ConvertValue(maValueObj);
                    if (returnValue is T)
                    {
                        value = (T)returnValue;
                        return true;
                    }
                }
            }
            value = default(T);
            return false;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Convert MA API value to the value used for MA Library. For example, convert ReferenceValue to ComplexType, convert ArrayValue to Array(T[]).
        /// </summary>
        /// <param name="maValueObj">Value object from MA API</param>
        /// <returns></returns>
        private Object ConvertValue(Object maValueObj)
        {
            // If maValueObject is an OptionalValue, get its value.
            if (maValueObj is IOptionalValue)
            {
                if ((maValueObj as IOptionalValue).HasValue)
                    maValueObj = (maValueObj as IOptionalValue).GetValue();
                else
                    return null;
            }

            if (maValueObj is ReferenceValue)
            {
                // Convert ReferenceValue to ComplexType
                return new ComplexType((ReferenceValue)maValueObj);
            }
            else if (maValueObj is ContainerValue)
            {

                if (((ContainerValue)maValueObj).Count == 0)
                {
                    throw new NotSupportedException("Array length should be larger than 0");
                }
                if (maValueObj is BinaryValue)
                {
                    // Convert BinaryValue to byte[]
                    int length = (maValueObj as BinaryValue).Count;
                    byte[] valueArray = new byte[length];

                    for (int i = 0; i < length; i++)
                    {
                        valueArray[i] = (maValueObj as BinaryValue)[i];
                    }
                    return valueArray;
                }
                else if (maValueObj is IEnumerable)
                {
                    // Convert ArrayValue or SetValue to Array 
                    Type elementType = null;
                    Array valueArray = null;
                    IEnumerable enumerableValue = (IEnumerable)maValueObj;
                    int index = 0;
                    foreach (object valueObj in enumerableValue)
                    {
                        if (elementType == null)
                        {
                            elementType = ConvertValue(valueObj).GetType();
                            valueArray = Array.CreateInstance(elementType, ((ContainerValue)maValueObj).Count);
                        }
                        valueArray.SetValue(ConvertValue(valueObj), index++);
                    }
                    return valueArray;
                }
                else
                {
                    throw new NotSupportedException("No supported ContainerValue type");
                }
            }
            else if (maValueObj is XmlValue)
            {
                ////Convert XmlVaule to xml
                XmlValue x = maValueObj as XmlValue;
                return x.ToString();
            }
            else
            {
                // For basic type, return directly
                return maValueObj;
            }
        }
        #endregion Private Methods
    }
}
