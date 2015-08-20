// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Internal used helper class
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Get the last error of p-invoke as Hex string
        /// </summary>
        /// <returns>hex String of error code</returns>
        public static string GetLastErrorCodeString()
        {
            int error = Marshal.GetLastWin32Error();
            return string.Format(" Error code is 0x{0:x}", error);
        }


        /// <summary>
        /// Concatenate several byte arrays to one byte array
        /// </summary>
        /// <param name="data">The byte arrays to be concatenated</param>
        /// <returns>The concatenated byte array</returns>
        internal static byte[] ConcatenateByteArrays(params byte[][] data)
        {
            if (data == null)
            {
                // null input is valid for this function
                return null;
            }

            //compute the length of concatenated byte array
            int concatenatedDataLength = 0;
            foreach (byte[] item in data)
            {
                if (item != null)
                {
                    concatenatedDataLength += item.Length;
                }
            }

            if (concatenatedDataLength == 0)
            {
                return new byte[0];
            }

            //copy the original byte arrays to the concatenated array
            byte[] resultArray = new byte[concatenatedDataLength];
            concatenatedDataLength = 0;
            foreach (byte[] item in data)
            {
                if (item != null)
                {
                    Array.Copy(item, 0, resultArray, concatenatedDataLength, item.Length);
                    concatenatedDataLength += item.Length;
                }
            }

            return resultArray;
        }
    }
}
