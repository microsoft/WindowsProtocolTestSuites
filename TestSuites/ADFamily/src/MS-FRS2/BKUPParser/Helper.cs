// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace BKUPParser
{
    /// <summary>
    /// Helper Class
    /// </summary>
    class Helper
    {
        #region GetSubArray

        /// <summary>
        /// This method is used to extract and return "length" number
        /// of bytes from a byte array starting from the given stratIndex.
        /// </summary>
        /// <param name="byteArray">
        /// Byte array to be parsed.
        /// </param>
        /// <param name="startIndex">
        /// Starting index of the required byte array.
        /// </param>
        /// <param name="length">
        /// Number of bytes in the required byte array.
        /// </param>
        /// <returns>
        /// Returns the Sub-Array
        /// Required byte array or, 
        /// null in case of some error in byte parsing.
        /// </returns>
        public byte[] GetSubArray(byte[] byteArray, int startIndex, int length)
        {
            if (length < 0)
            {                
                return null;
            }
            else if (startIndex < 0)
            {                
                return null;
            }
            else if (length > byteArray.Length)
            {
                return null;
            }

            byte[] temp = new byte[length];
            Array.Copy(byteArray, startIndex, temp, 0, length);
            return temp;
        }

        #endregion
    }
}
