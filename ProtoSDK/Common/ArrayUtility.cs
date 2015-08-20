// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// Utility class, contains frequently used array-operation methods by Stack SDK.
    /// </summary>
    public static class ArrayUtility
    {
        /// <summary>
        /// Compares two arrays
        /// </summary>
        /// <typeparam name="T">
        /// Type of array, must implement IComparable&lt;T&gt; interface
        /// </typeparam>
        /// <param name="array1">The first array</param>
        /// <param name="array2">The second array</param>
        /// <returns>True if the two arrays are equal, false otherwise</returns>
        public static bool CompareArrays<T>(T[] array1, T[] array2) where T : IComparable<T>
        {
            // Reference equal
            if (array1 == array2)
            {
                return true;
            }
            // one of the arrays is null
            if (array1 == null || array2 == null)
            {
                return false;
            }
            // The two arrays have different size
            if (array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].CompareTo(array2[i]) != 0)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Concatenates a list of arrays to one array.
        /// </summary>
        /// <typeparam name="T">The type of array.</typeparam>
        /// <param name="arrays">The input arrays.</param>
        /// <exception cref="ArgumentNullException">Raised when arrays is null.</exception>
        /// <returns>Concatenated array.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static T[] ConcatenateArrays<T>(params T[][] arrays)
        {
            if (arrays == null)
            {
                throw new ArgumentNullException("arrays");
            }

            List<T> arrayList = new List<T>();
            foreach (T[] array in arrays)
            {
                if (array != null)
                {
                    arrayList.AddRange(array);
                }
            }

            return arrayList.ToArray();
        }


        /// <summary>
        /// Gets a sub array from an array.
        /// </summary>
        /// <typeparam name="T">The type of array.</typeparam>
        /// <param name="array">The original array.</param>
        /// <param name="startIndex">The start index to copy.</param>
        /// <param name="length">The length of sub array.</param>
        /// <exception cref="ArgumentException">Raised when startIndex or startIndex plus the length of 
        /// sub array exceeds the range of original array.</exception>
        /// <returns>The sub array.</returns>
        public static T[] SubArray<T>(T[] array, int startIndex, int length)
        {
            T[] subArray = new T[length];
            Array.Copy(array, startIndex, subArray, 0, length);

            return subArray;
        }


        /// <summary>
        /// Gets a sub array from an array. With given start index, it will return the rest of the array.
        /// </summary>
        /// <typeparam name="T">The type of array.</typeparam>
        /// <param name="array">The original array.</param>
        /// <param name="startIndex">The start index to copy.</param>
        /// <exception cref="ArgumentException">Raised when startIndex or startIndex plus the length of 
        /// sub array exceeds the range of original array.</exception>
        /// <returns>The sub array.</returns>
        public static T[] SubArray<T>(T[] array, int startIndex)
        {
            return SubArray<T>(array, startIndex, array.Length - startIndex);
        }
    }
}
