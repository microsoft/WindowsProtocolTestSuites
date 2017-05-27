// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// The EndianUtility class provides a collection of methods for reverse byte order of some
    /// basic types.
    /// </summary>
    public static class EndianUtility
    {
        /// <summary>
        /// Reverse the byte order of a short variable.
        /// </summary>
        /// <param name="v">Value to be reversed.</param>
        /// <returns>The reversed value.</returns>
        public static short ReverseByteOrder(short v)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }

        /// <summary>
        /// Reverse the byte order of an unsigned short variable.
        /// </summary>
        /// <param name="v">Value to be reversed.</param>
        /// <returns>The reversed value.</returns>
        public static ushort ReverseByteOrder(ushort v)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        /// Reverse the byte order of a int variable.
        /// </summary>
        /// <param name="v">Value to be reversed.</param>
        /// <returns>The reversed value.</returns>
        public static int ReverseByteOrder(int v)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Reverse the byte order of an unsigned int variable.
        /// </summary>
        /// <param name="v">Value to be reversed.</param>
        /// <returns>The reversed value.</returns>
        public static uint ReverseByteOrder(uint v)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// Reverse the byte order of a long variable.
        /// </summary>
        /// <param name="v">Value to be reversed.</param>
        /// <returns>The reversed value.</returns>
        public static long ReverseByteOrder(long v)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            Array.Reverse(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// Reverse the byte order of an unsigned long variable.
        /// </summary>
        /// <param name="v">Value to be reversed.</param>
        /// <returns>The reversed value.</returns>
        public static ulong ReverseByteOrder(ulong v)
        {
            byte[] bytes = BitConverter.GetBytes(v);
            Array.Reverse(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }

        /// <summary>
        /// Reverse the byte order of a GUID variable.
        /// </summary>
        /// <param name="v">GUID to be reversed.</param>
        /// <returns>The reversed GUID.</returns>
        public static Guid ReverseByteOrder(Guid v)
        {
            byte[] reversed = new byte[16];
            byte[] origin = v.ToByteArray();

            reversed[3] = origin[0];
            reversed[2] = origin[1];
            reversed[1] = origin[2];
            reversed[0] = origin[3];
            reversed[5] = origin[4];
            reversed[4] = origin[5];
            reversed[7] = origin[6];
            reversed[6] = origin[7];
            for (int i = 8; i < 16; i++)
            {
                reversed[i] = origin[i];
            }

            return new Guid(reversed);
        }

    }
}
