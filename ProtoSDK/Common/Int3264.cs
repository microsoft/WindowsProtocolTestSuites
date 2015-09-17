// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// Int3264 represents 32-bit integer on x86, and it represents 64-bit integer on amd64.
    /// </summary>
    public struct Int3264
    {
        // Value of this instance.
        private IntPtr value;


        /// <summary>
        /// Converts the value of this instance to a 32-bit signed integer.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer equal to the value of this instance.
        /// </returns>
        public int ToInt32()
        {
            return value.ToInt32();
        }


        /// <summary>
        /// Converts the value of this instance to a 32-bit unsigned integer.
        /// </summary>
        /// <returns>
        /// A 32-bit unsigned integer equal to the value of this instance.
        /// </returns>
        public uint ToUInt32()
        {
            unchecked
            {
                return (uint)value.ToInt32();
            }
        }


        /// <summary>
        /// Converts the value of this instance to a 64-bit signed integer.
        /// </summary>
        /// <returns>
        /// A 64-bit signed integer equal to the value of this instance.
        /// </returns>
        public long ToInt64()
        {
            return value.ToInt64();
        }


        /// <summary>
        /// Converts the value of this instance to a 64-bit unsigned integer.
        /// </summary>
        /// <returns>
        /// A 64-bit unsigned integer equal to the value of this instance.
        /// </returns>
        public ulong ToUInt64()
        {
            unchecked
            {
                return (ulong)value.ToInt64();
            }
        }


        /// <summary>
        /// Converts the value of this instance to an IntPtr.
        /// </summary>
        /// <returns>
        /// An IntPtr equal to the value of this instance.
        /// </returns>
        public IntPtr ToIntPtr()
        {
            return value;
        }


        /// <summary>
        /// Implicit convert an Int3264 to IntPtr.
        /// </summary>
        /// <param name="num">An Int3264.</param>
        /// <returns>An IntPtr.</returns>
        public static implicit operator IntPtr(Int3264 num)
        {
            return num.ToIntPtr();
        }


        /// <summary>
        /// Implicit convert an IntPtr to Int3264.
        /// </summary>
        /// <param name="p">An IntPtr.</param>
        /// <returns>An Int3264.</returns>
        public static implicit operator Int3264(IntPtr p)
        {
            Int3264 ret = new Int3264();
            ret.value = p;
            return ret;
        }


        /// <summary>
        /// Implicit convert a SafeIntPtr to Int3264.
        /// </summary>
        /// <param name="p">A SafeIntPtr.</param>
        /// <returns>An Int3264.</returns>
        public static implicit operator Int3264(SafeIntPtr p)
        {
            Int3264 ret = new Int3264();
            ret.value = p;
            return ret;
        }


        /// <summary>
        /// Implicit convert a 32-bit signed integer to Int3264.
        /// </summary>
        /// <param name="num">A 32-bit signed integer.</param>
        /// <returns>An Int3264.</returns>
        public static implicit operator Int3264(int num)
        {
            Int3264 ret = new Int3264();
            ret.value = new IntPtr(num);
            return ret;
        }


        /// <summary>
        /// Implicit convert a 32-bit unsigned integer to Int3264.
        /// </summary>
        /// <param name="num">A 32-bit unsigned integer.</param>
        /// <returns>An Int3264.</returns>
        public static implicit operator Int3264(uint num)
        {
            Int3264 ret = new Int3264();
            unchecked
            {
                ret.value = new IntPtr((int)num);
            }
            return ret;
        }


        /// <summary>
        /// Implicit convert a 64-bit signed integer to Int3264.
        /// </summary>
        /// <param name="num">A 64-bit signed integer.</param>
        /// <returns>An Int3264.</returns>
        public static implicit operator Int3264(long num)
        {
            Int3264 ret = new Int3264();
            ret.value = new IntPtr(num);
            return ret;
        }


        /// <summary>
        /// Implicit convert a 64-bit unsigned integer to Int3264.
        /// </summary>
        /// <param name="num">A 64-bit unsigned integer.</param>
        /// <returns>An Int3264.</returns>
        public static implicit operator Int3264(ulong num)
        {
            Int3264 ret = new Int3264();
            unchecked
            {
                ret.value = new IntPtr((long)num);
            }
            return ret;
        }
    }
}
