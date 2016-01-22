// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.MessageAnalyzer;

namespace Microsoft.Protocol.TestSuites.Azod.Adapter
{
    public class MAVerifyUtil
    {              

        /// <summary>
        /// Get Fieldvalue from a MMA complexValue by using field name
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="fieldName">Field name</param>
        /// <param name="complexValue">MMA ComplexValue</param>
        /// <param name="returnValue">value to be returned</param>
        /// <returns>true if success, false if failed</returns>
        public static bool GetFieldValue<T>(string fieldName, ComplexType complexValue, out T returnValue)
        {
            try
            {
                string[] fields = fieldName.Split('.');
                ComplexType referenceValue = complexValue;
                for (int i = 0; i < fields.Length - 1; i++)
                {
                    referenceValue = GetFieldValue<ComplexType>(fields[i], referenceValue);
                }
                returnValue = GetFieldValue<T>(fields[fields.Length - 1], referenceValue);
                return true;
            }
            catch (KeyNotFoundException)
            {
                returnValue = default(T);
                return false;
            }
            catch (NotSupportedException)
            {
                returnValue = default(T);
                return false;
            }
        }
              
        /// <summary>
        /// Verify whether the value apply the verify item.
        /// </summary>
        /// <param name="value">Value from captured message</param>
        /// <param name="verifyItem">Verify item</param>
        /// <returns></returns>
        public static bool VerifyValue(Object value, VerifyItem verifyItem)
        {
            
            if (value is ComplexType && verifyItem.FieldName != null)
            {
                ComplexType complexValue = (ComplexType)value;
                if (!GetFieldValue<Object>(verifyItem.FieldName, complexValue, out value))
                    return false;
            }

            string operation = verifyItem.Operation;
            if (operation.ToLower().Equals("equal"))
            {
                return IsEqual(value, verifyItem.Value, verifyItem.Type);
            }
            else if (operation.ToLower().Equals("notequal"))
            {
                return IsNotEqual(value, verifyItem.Value, verifyItem.Type);
            }
            else if (operation.ToLower().Equals("flagset"))
            {
                return IsFlagSet(value, verifyItem.Value, verifyItem.Type);
            }
            //TODO: add more operation
            return false;
        }
        /// <summary>
        /// Verify equal operation
        /// </summary>
        /// <param name="complexValue">MMA complex value</param>
        /// <param name="verifyItem">Verify Item</param>
        /// <returns>true if success, false if failed</returns>
        private static bool IsEqual(Object value, string expectValue, string valueType)
        {
			if (value == null)
            {
                return false;
            }
            //TODO: add types if necessary.
            switch(valueType)
            {
                case "BinaryValue":
                    string tmpStr = Encoding.UTF8.GetString((byte[])value);
                    return string.Equals(tmpStr, expectValue, StringComparison.InvariantCultureIgnoreCase);
                default:
                    return string.Equals(value.ToString(), expectValue, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Verify notequal operation
        /// </summary>
        /// <param name="complexValue">MMA complex value</param>
        /// <param name="verifyItem">Verify Item</param>
        /// <returns>true if success, false if failed</returns>
        private static bool IsNotEqual(Object value, string expectValue, string valueType)
        {
			if (value == null)
            {
                return false;
            }
            return !(IsEqual(value, expectValue, valueType));
        }

        /// <summary>
        /// Verify flagset operation
        /// </summary>
        /// <param name="complexValue">MMA complex value</param>
        /// <param name="verifyItem">Verify Item</param>
        /// <returns>true if success, false if failed</returns>
        private static bool IsFlagSet(Object value, string expectValue, string valueType)
        {
			if (value == null)
            {
                return false;
            }
            if (value is IConvertible)
            {
                ulong valueLong = (value as IConvertible).ToUInt64(null);
                ulong expectValueLong = UInt64.Parse(expectValue);
                if ((valueLong & expectValueLong) == expectValueLong)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Get Field value, the field name may be an array item, such as fieldname[0].
        /// </summary>
        /// <typeparam name="T">Type of field</typeparam>
        /// <param name="fieldName">field name</param>
        /// <param name="complexValue">complex type contains field name</param>
        /// <returns></returns>
        private static T GetFieldValue<T>(string fieldName, ComplexType complexValue)
        {
            if (fieldName == null || fieldName.Length == 0)
            {
                throw new ArgumentNullException();
            }
            T returnValue = default(T);

            if (fieldName.Contains('['))
            {
                // If field is an array with index, for example: fieldName[0]
                string[] strArray = fieldName.Split('[');
                string arrayName = strArray[0];
                int index = int.Parse(strArray[1].Remove(strArray[1].Length - 1));
                returnValue = complexValue.TryGetValue<T[]>(arrayName)[index];
            }
            else
            {                
                returnValue = complexValue.TryGetValue<T>(fieldName);
            }
            return returnValue;

        }
    }
    
}
