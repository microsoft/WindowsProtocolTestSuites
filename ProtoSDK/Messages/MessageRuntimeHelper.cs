// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Win32;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages
{
    /// <summary>
    /// Provides a series of helper methods in message runtime module
    /// </summary>
    public static class MessageRuntimeHelper
    {
        /// <summary>
        /// Describes a (possibly symbolic) value.
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="value">(possibly symbolic) value</param>
        /// <returns>description</returns>
        public static string Describe<T>(T value)
        {
            if (value == null)
                return "null";
            Type type = value.GetType();
            if (IsStruct(type))
            {

                MethodInfo methodInfo = type.GetMethod("ToString", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                if (methodInfo == null)
                {
                    FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    bool first = true;
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("{0}(", type.Name);
                    foreach (FieldInfo field in fields)
                    {
                        if (!first)
                            sb.Append(",");
                        else
                            first = false;
                        sb.AppendFormat("{0}=", field.Name);
                        object fieldValue = field.GetValue((object)value);
                        sb.AppendFormat("{0}", Describe<object>(fieldValue));
                    }
                    sb.Append(")");
                    return sb.ToString();
                }
                else
                {
                    object result = methodInfo.Invoke((object)value, null);
                    return result as string;
                }
            }

            return value.ToString();
        }    


        #region helper methods
        /// <summary>
        /// Check if the given type is a Struct
        /// </summary>
        /// <param name="type">type to check</param>
        /// <returns>True if the type is a structure; False if not.</returns>
        private static bool IsStruct(Type type)
        {
            if (type.IsValueType && !type.IsPrimitive && !type.IsEnum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion helper methods
    }
}
