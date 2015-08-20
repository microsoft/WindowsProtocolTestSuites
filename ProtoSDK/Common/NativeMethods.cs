// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// P/Invoke for Windows API.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Moves a block of memory from one location to another.
        /// </summary>
        /// <param name="Destination">
        /// A pointer to the starting address of the move destination. 
        /// </param>
        /// <param name="Source">
        /// A pointer to the starting address of the block of memory to be moved.
        /// </param>
        /// <param name="Length">
        /// The size of the block of memory to move, in bytes.
        /// </param>
        [DllImport("kernel32.dll")]
        public static extern void RtlMoveMemory(IntPtr Destination, IntPtr Source, uint Length);
    }
}
