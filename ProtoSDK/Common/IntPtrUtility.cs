// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// Provides a collection of methods for manipulating unmanaged memory, converting
    /// between unmanaged and managed types.
    /// </summary>
    public static class IntPtrUtility
    {
        // The list for all pointers that allocated in this class.
        private static Dictionary<IntPtr, List<SafeIntPtr>> ptrList = new Dictionary<IntPtr, List<SafeIntPtr>>();


        // Add a SafeIntPtr to pending release list
        private static void AppendToPtrList(IntPtr key, SafeIntPtr safeIntPtr)
        {
            List<SafeIntPtr> list;
            if (!ptrList.TryGetValue(key, out list))
            {
                list = new List<SafeIntPtr>();
                ptrList.Add(key, list);
            }
            list.Add(safeIntPtr);
        }


        /// <summary>
        /// Frees a block of unmanaged memory pointed by a pointer,
        /// and set pointer to zero.
        /// </summary>
        /// <param name="p">Pointer to a block of unmanaged memory.</param>
        [SecurityPermission(SecurityAction.Demand)]        
        public static void FreePtr(ref IntPtr p)
        {
            if (p != IntPtr.Zero)
            {
                List<SafeIntPtr> list;
                if (ptrList.TryGetValue(p, out list))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].Dispose();
                    }
                    ptrList.Remove(p);
                }

                Marshal.FreeHGlobal(p);
                p = IntPtr.Zero;
            }
        }


        /// <summary>
        /// Converts a UInt32 to IntPtr
        /// </summary>
        /// <param name="value">The value to be converted to IntPtr.</param>
        /// <returns>The IntPtr.</returns>
        public static IntPtr UInt32ToPtr(uint value)
        {
            unchecked
            {
                return new IntPtr((int)value);
            }
        }


        /// <summary>
        /// Unmarshals data from an unmanaged block of memory to an array of managed object.
        /// </summary>
        /// <typeparam name="T">The type of the element of the array.</typeparam>
        /// <param name="ptr">Pointer to an unmanaged block of memory.</param>
        /// <param name="count">Count of elements to be marshaled.</param>
        /// <returns>An array of managed object.</returns>
        // suppress CA1004 because this is utility method for array,
        // and returning object[] is not user-friendly.
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static T[] PtrToArray<T>(IntPtr ptr, uint count)
            where T : struct
        {
            return PtrToArray<T>(ptr, 0, count);
        }


        /// <summary>
        /// Unmarshals data from an unmanaged block of memory to an array of managed object,
        /// begins at the offset.
        /// </summary>
        /// <typeparam name="T">The type of the element of the array.</typeparam>
        /// <param name="ptr">Pointer to an unmanaged block of memory.</param>
        /// <param name="count">Count of elements to be marshaled.</param>
        /// <param name="offset">The array address' offset from <paramref name="ptr"/></param>
        /// <returns>An array of managed object.</returns>
        // suppress CA1004 because this is utility method for array,
        // and returning object[] is not user-friendly.
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static T[] PtrToArray<T>(IntPtr ptr, int offset, uint count)
            where T : struct
        {
            if (ptr == IntPtr.Zero)
            {
                if (count == 0)
                {
                    return new T[0];
                }
                else
                {
                    throw new ArgumentNullException("ptr");
                }
            }

            T[] array = new T[count];

            // 32 bits platform
            if (IntPtr.Size == sizeof(Int32))
            {
                int elementAddress = ptr.ToInt32() + offset;
                for (uint i = 0; i < count; i++)
                {
                    array[i] = TypeMarshal.ToStruct<T>(new IntPtr(elementAddress));
                    int elementSize = TypeMarshal.GetNativeMemorySize<T>();
                    elementAddress += elementSize;
                }
            }
            // 64 bits platform
            else if (IntPtr.Size == sizeof(Int64))
            {
                long elementAddress = ptr.ToInt64() + offset;
                for (uint i = 0; i < count; i++)
                {
                    array[i] = TypeMarshal.ToStruct<T>(new IntPtr(elementAddress));
                    int elementSize = TypeMarshal.GetNativeMemorySize<T>();
                    elementAddress += elementSize;
                }
            }
            // not supported
            else
            {
                throw new NotSupportedException("Platform is neither 32 bits nor 64 bits.");
            }

            return array;
        }


        /// <summary>
        /// Marshals data from an array of managed object to an unmanaged block of memory.
        /// </summary>
        /// <typeparam name="T">The type of the element of the array.</typeparam>
        /// <param name="array">An array of managed object.</param>
        /// <returns>Pointer to an unmanaged block of memory.</returns>
        // suppress CA1004 because this is utility method for array,
        // and returning object[] is not user-friendly.
        [SecurityPermission(SecurityAction.Demand)]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IntPtr ArrayToPtr<T>(T[] array) where T : struct
        {
            if (array == null)
            {
                return IntPtr.Zero;
            }

            int size = 0;
            for (uint i = 0; i < array.Length; i++)
            {
                size += TypeMarshal.GetNativeMemorySize<T>();
            }

            IntPtr ptr = Marshal.AllocHGlobal(size);

            ArrayToPtr<T>(array, ptr, 0);

            return ptr;
        }


        /// <summary>
        /// Marshals data from an array of managed object to an unmanaged block of memory,
        /// writes the unmanaged data to <paramref name="offset"/> from given <paramref name="ptr"/>.
        /// </summary>
        /// <typeparam name="T">The type of the element of the array.</typeparam>
        /// <param name="array">An array of managed object.</param>
        /// <param name="ptr">The unmanaged base address to write, must 
        /// have been allocated of enough memory by the caller.</param>
        /// <param name="offset">The offset to write to.</param>
        [SecurityPermission(SecurityAction.Demand)]
        public static void ArrayToPtr<T>(T[] array, IntPtr ptr, int offset) where T : struct
        {
            // 32 bits platform
            if (IntPtr.Size == sizeof(Int32))
            {
                int ptrValue = ptr.ToInt32() + offset;
                for (uint i = 0; i < array.Length; i++)
                {
                    int elementSize = TypeMarshal.GetNativeMemorySize<T>();
                    SafeIntPtr safePtr = TypeMarshal.ToIntPtr<T>(array[i]);
                    AppendToPtrList(ptr, safePtr);
                    NativeMethods.RtlMoveMemory(new IntPtr(ptrValue), safePtr, (uint)elementSize);
                    ptrValue += elementSize;
                }
            }
            // 64 bits platform
            else if (IntPtr.Size == sizeof(Int64))
            {
                long ptrValue = ptr.ToInt64() + offset;
                for (uint i = 0; i < array.Length; i++)
                {
                    int elementSize = TypeMarshal.GetNativeMemorySize<T>();
                    SafeIntPtr safePtr = TypeMarshal.ToIntPtr<T>(array[i]);
                    AppendToPtrList(safePtr, safePtr);
                    NativeMethods.RtlMoveMemory(new IntPtr(ptrValue), safePtr, (uint)elementSize);
                    ptrValue += elementSize;
                }
            }
            // not supported
            else
            {
                throw new NotSupportedException("Platform is neither 32 bits nor 64 bits.");
            }
        }


        /// <summary>
        /// Add an offset to a pointer.
        /// </summary>
        /// <param name="ptr">A pointer as the base.</param>
        /// <param name="offset">The offset to be added.</param>
        /// <returns>A new pointer.</returns>
        public static IntPtr Add(IntPtr ptr, int offset)
        {
            if (IntPtr.Size == sizeof(Int32))
            {
                // x86
                return new IntPtr(ptr.ToInt32() + offset);
            }
            else if (IntPtr.Size == sizeof(Int64))
            {
                // amd64
                return new IntPtr(ptr.ToInt64() + offset);
            }
            else
            {
                throw new NotSupportedException("Platform is neither 32 bits nor 64 bits.");
            }
        }


        /// <summary>
        /// Calculate the offset between 2 pointers.
        /// </summary>
        /// <param name="ptr1">A pointer.</param>
        /// <param name="ptr2">Another pointer.</param>
        /// <returns>A new pointer represents the offset.</returns>
        public static IntPtr CalculateOffset(IntPtr ptr1, IntPtr ptr2)
        {
            if (IntPtr.Size == sizeof(Int32))
            {
                //x86
                return new IntPtr(ptr1.ToInt32() - ptr2.ToInt32());
            }
            else if (IntPtr.Size == sizeof(Int64))
            {
                //amd64
                return new IntPtr(ptr1.ToInt64() - ptr2.ToInt64());
            }
            else
            {
                throw new NotSupportedException("Platform is neither 32 bits nor 64 bits.");
            }
        }


        /// <summary>
        /// Align a pointer. 
        /// Example: Align(33, 4) == 36
        /// </summary>
        /// <param name="ptr">A pointer.</param>
        /// <param name="align">The alignment.</param>
        /// <returns>A new aligned pointer.</returns>
        public static IntPtr Align(IntPtr ptr, int align)
        {
            checked { align--; }
            if (IntPtr.Size == sizeof(Int32))
            {
                //x86
                return new IntPtr((ptr.ToInt32() + align) & ~align);
            }
            else if (IntPtr.Size == sizeof(Int64))
            {
                //amd64
                return new IntPtr((ptr.ToInt64() + align) & ~align);
            }
            else
            {
                throw new NotSupportedException("Platform is neither 32 bits nor 64 bits.");
            }
        }

    }
}
