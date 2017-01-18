// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    internal static class JsonUtility
    {
        /// <summary>
        /// Encode JSON.
        /// </summary>
        public static string Encode(string str)
        {
            return str.Replace("/", @"\/");
        }

        /// <summary>
        /// Deserialize JSON.
        /// </summary>
        public static Dictionary<string, string> DeserializeJSON(string str)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<Dictionary<string, string>>(str);
        }

        /// <summary>
        /// Deserialize JSON.
        /// </summary>
        public static object DeserializeJSON(string str, Type T)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize(str, T);
        }

        /// <summary>
        /// Serialize JSON.
        /// </summary>
        public static string SerializeJSON(object o)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(o);
        }
    }

    internal static class Base64Helper
    {
        /// <summary>
        /// Encode byte array to base64 string.
        /// </summary>
        public static string Base64Encode(byte[] bytes, bool urlSafe = true, bool padding = false)
        {
             var base64 = Convert.ToBase64String(bytes);

            if (urlSafe) {
                // Url Safe Base64 replace + with -, and replace / with _
                // in order to decode correctly, replace them back
                base64 = base64.Replace('+', '-').Replace('/', '_');
            }

            if (!padding) {
                // if the base64 does not need padding, remove it
                base64 = base64.Replace("=", string.Empty);
            }

            return base64;
        }

        /// <summary>
        /// Decode base64 string to byte array.
        /// </summary>
        public static byte[] Base64Decode(string base64, bool urlSafe = true, bool padding = false)
        {
            if (urlSafe) {
                // Url Safe Base64 replace + with -, and replace / with _
                // in order to decode correctly, replace them back
                base64 = base64.Replace('-', '+').Replace('_', '/');
            }

            if (!padding) {
                // if the base64 does not have correct padding, add padding to it
                base64 = base64.PadRight(base64.Length + (4 - base64.Length%4)%4, '=');
            }

            return Convert.FromBase64String(base64);
        }      
    }

    internal static class UrlHelper
    {
        /// <summary>
        /// Combine url strings.
        /// </summary>
        public static string CombineUrls(params string[] urls)
        {
            return (string.Join("/", urls.Select(_ => _.Trim('/')))) + // combine together
                   (urls.Last().EndsWith("/") ? "/" : string.Empty);   // keep the ending slash
        }
    }

    internal static class ReflectionHelper
    {
        internal static BindingFlags BindingFlags =
            BindingFlags.Public    |
            BindingFlags.NonPublic |
            BindingFlags.Instance  |
            BindingFlags.Static;

        /// <summary>
        /// Sets an object's field value.
        /// </summary>
        /// <param name="obj">
        /// The object instance.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <param name="value">
        /// The value to be set.
        /// </param>
        public static bool SetFieldValue(this object obj, string name, object value, bool ignoreCase = false)
        {
            if(ignoreCase) BindingFlags |= BindingFlags.IgnoreCase;
            var field = obj.GetType().GetField(name, BindingFlags);
            if (field == null) return false;
            field.SetValue(obj, value); return true;
        }

        /// <summary>
        /// Sets all the string fields in an object to the empty string.
        /// </summary>
        /// <param name="obj">
        /// The object instance.
        /// </param>
        public static void ResetStringFields(this object obj)
        {
            obj.GetType().GetFields(BindingFlags)
               .Where  (_ => _.FieldType == typeof (string))
               .ForEach(_ => _.SetValue(obj, string.Empty));
        }
    }

    internal static class StringExtensions
    {
        /// <summary>
        /// An extension to string. Compares if two strings are equal in a case-insensitive way.
        /// </summary>
        public static bool EqualsIgnoreCase(this string source, string target)
        {
            if (source == null || target == null) return false;
            return string.Equals(source, target, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// An extension to string. Judges if one string contains another in a case-insensitive way.
        /// </summary>
        public static bool ContainsIgnoreCase(this string source, string target)
        {
            if (source == null || target == null) return false;
            return Regex.IsMatch(source, target, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// An extension to string. Convert the given text to base64.
        /// </summary>
        public static string EncodeToBase64(this string text, Encoding encoding, bool urlSafe = true, bool padding = false)
        {
            return Base64Helper.Base64Encode(encoding.GetBytes(text), urlSafe, padding);
        }

        /// <summary>
        /// An extension to string. Convert the given text to base64
        /// using utf-8 as default text encoding.
        /// </summary>
        public static string EncodeToBase64(this string text, bool urlSafe = true, bool padding = false)
        {
            return EncodeToBase64(text, Encoding.UTF8);
        }

        /// <summary>
        /// An extension to string. Convert the base64 string to plain text.
        /// </summary>
        public static string DecodeFromBase64(this string base64, Encoding encoding, bool urlSafe = true, bool padding = false)
        {
            return encoding.GetString(Base64Helper.Base64Decode(base64, urlSafe, padding));
        }

        /// <summary>
        /// An extension to string. Convert the base64 string to plain text
        /// using utf-8 as the default text encoding.
        /// </summary>
        public static string DecodeFromBase64(this string base64, bool urlSafe = true, bool padding = false)
        {
            return DecodeFromBase64(base64, Encoding.UTF8);
        }
    }

    internal static class X509Certificate2Extensions
    {
        /// <summary>
        /// An extension to X509Certificate2. Convert a certificate to a base64 string.
        /// </summary>
        public static string ToBase64String(this X509Certificate2 certificate)
        {
            return Convert.ToBase64String(certificate.Export(X509ContentType.Cert));
        }
    }

    internal static class LINQExtensions
    {
        /// <summary>
        /// An extension to IEnumerable enabling ForEach in LINQ expressions.
        /// </summary>
        /// <remarks>
        /// Note that elements in the IEnumerable are readonly.
        /// </remarks>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration) {
                action(item);
            }
        }
    }
}
