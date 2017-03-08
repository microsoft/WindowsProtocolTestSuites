// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.MessageAnalyzer;
using System.Globalization;

namespace Microsoft.Protocol.TestSuites.ADOD.Adapter
{
    public class MAVerifyUtil
    {              

        /// <summary>
        /// Get field value from a MA complexValue by using field name
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="fieldName">Field name</param>
        /// <param name="complexValue">MA ComplexValue</param>
        /// <param name="returnValue">Specifies the value to be returned.</param>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
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
        }
              
        /// <summary>
        /// Verify whether the value is applied to the verified item.
        /// </summary>
        /// <param name="value">Specifies the value from the captured message.</param>
        /// <param name="verifyItem">Verify item</param>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
        public static bool VerifyValue(Object value, VerifyItem verifyItem)
        {
            
            if (value is ComplexType && verifyItem.FieldName != null)
            {
                ComplexType complexValue = (ComplexType)value;
                if (!GetFieldValue<Object>(verifyItem.FieldName, complexValue, out value))
                    return false;
            }

            string operation = verifyItem.Operation;
            if (operation.ToLower(CultureInfo.InvariantCulture).Equals("equal"))
            {
                return IsEqual(value, verifyItem.Value, verifyItem.Type);
            }
            else if (operation.ToLower(CultureInfo.InvariantCulture).Equals("notequal"))
            {
                return IsNotEqual(value, verifyItem.Value, verifyItem.Type);
            }
            else if (operation.ToLower(CultureInfo.InvariantCulture).Equals("flagset"))
            {
                return IsFlagSet(value, verifyItem.Value, verifyItem.Type);
            }
            return false;
        }

        /// <summary>
        /// Verify equal operation
        /// </summary>
        /// <param name="complexValue">MA complex value</param>
        /// <param name="verifyItem">Verify Item</param>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
        private static bool IsEqual(Object value, string expectValue, string valueType)
        {
            if (value == null)
            {
                return false;
            }

            switch(valueType)
            {
                case "BinaryValue":
                    string tmpStr = Encoding.UTF8.GetString((byte[])value);
                    return string.Equals(tmpStr, expectValue, StringComparison.OrdinalIgnoreCase);
                default:
                    return string.Equals(value.ToString(), expectValue, StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// Verify notequal operation
        /// </summary>
        /// <param name="complexValue">MA complex value</param>
        /// <param name="verifyItem">Verify Item</param>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
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
        /// <param name="complexValue">MA complex value</param>
        /// <param name="verifyItem">Verify Item</param>
        /// <returns>If succeeded, return TRUE, else return FALSE.</returns>
        private static bool IsFlagSet(Object value, string expectValue, string valueType)
        {
            if (value == null)
            {
                return false;
            }

            if (value is IConvertible)
            {
                ulong valueLong = (value as IConvertible).ToUInt64(null);
                ulong expectValueLong = UInt64.Parse(expectValue, CultureInfo.InvariantCulture);
                if ((valueLong & expectValueLong) == expectValueLong)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Get field value, the field name may be an array item, such as fieldname[0].
        /// </summary>
        /// <typeparam name="T">Type of field</typeparam>
        /// <param name="fieldName">field name</param>
        /// <param name="complexValue">Specifies the complex type contained in field name.</param>
        ///  <returns>FiledValue</returns>
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
                int index = int.Parse(strArray[1].Remove(strArray[1].Length - 1), CultureInfo.InvariantCulture);
                returnValue = complexValue.TryGetValue<T[]>(arrayName)[index];
            }
            else
            {
                try
                {
                    returnValue = complexValue.TryGetValue<T>(fieldName);
                }
                catch
                {
                }
            }
            return returnValue;

        }
    }
    
}
