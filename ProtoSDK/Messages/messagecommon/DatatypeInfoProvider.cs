// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling
{
    /// <summary>
    /// Length Mode
    /// </summary>
    public enum LengthMode
    {
        /// <summary>
        /// Marshaling in Bit Mode
        /// </summary>
        Bit,

        /// <summary>
        /// Marshaling in Byte Mode
        /// </summary>
        Byte,
    }

    /// <summary>
    /// Datatype Info Provider
    /// </summary>
    public static class DatatypeInfoProvider
    {
        /// <summary>
        /// Check if the given data type is predefined.
        /// </summary>
        /// <param name="datatypeName">Name of the data type</param>
        /// <returns>True if it is predefined</returns>
        public static bool IsPredefinedDatatype(string datatypeName)
        {
            int step = 3;
            for (int i = 0; i < datatypeLengthInfo.Length; i += step)
            {
                if (datatypeLengthInfo[i] == datatypeName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if the given type modifier is predefined.
        /// </summary>
        /// <param name="modifier">The given type modifier</param>
        /// <returns>True if it is predefined</returns>
        public static bool isPredefinedModifier(string modifier)
        {
            foreach (string str in predefinedModifier)
            {
                if (str == modifier)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get RPC data type length
        /// </summary>
        /// <param name="datatypeName">Name of the data type</param>
        /// <returns>The RPC data type length</returns>
        public static int GetRpcDatatypeLength(string datatypeName)
        {
            string lengthStr = "-1";
            int step = 3;

            if (datatypeLengthInfo.Length % 3 != 0)
            {
                return -1;
            }

            for (int i = 0; i < datatypeLengthInfo.Length; i += step)
            {
                if (datatypeLengthInfo[i] == datatypeName)
                {
                    lengthStr = datatypeLengthInfo[i + 1];
                    break;
                }
            }

            int length;
            if (!Int32.TryParse(lengthStr, out length))
            {
                return -1;
            }

            return length;
        }

        /// <summary>
        /// Get data type length.
        /// </summary>
        /// <param name="datatypeName">Name of the data type</param>
        /// <param name="length">The data type length (in string format)</param>
        /// <param name="mode">The length mode (in string format)</param>
        public static void GetDatatypeLength(string datatypeName, out string length, out string mode)
        {
            length = "-1";
            mode = "byte";
            int step = 3;

            for (int i = 0; i < datatypeLengthInfo.Length; i += step)
            {
                if (datatypeLengthInfo[i] == datatypeName)
                {
                    length = datatypeLengthInfo[i + 1];
                    mode = datatypeLengthInfo[i + 2];
                    break;
                }
            }
        }

        private static string[] datatypeLengthInfo = 
        {
            "BIT", "1", "bit",
            "byte", "1", "byte",
            "small", "1", "byte",
            "boolean", "1", "byte",
            "int", "4", "byte",
            "__int32", "4", "byte",
            "long", "4", "byte",
            "short", "2", "byte",
            "char", "1", "byte",
            "hyper", "8", "byte",
            "wchar_t", "2", "byte",
            "float", "4", "byte",
            "double", "8", "byte",
            "__int64", "8", "byte",
            "__int3264", "-1", "byte",
            "error_status_t", "4", "byte",
            "void", "-1", "byte",
            "handle_t", "-1", "byte",
        };

        private static string[] predefinedModifier = 
        {
            "unsigned",
            "const",
            "signed",
            "enum",
            "struct",
            "union",
        };
    }
}
