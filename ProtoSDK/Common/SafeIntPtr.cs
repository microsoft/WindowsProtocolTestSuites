// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// Represents a wrapper class for IntPtr. 
    /// The pointer will be automatically released when disposing, 
    /// do not call Marshal.FreeHGlobal() on it.
    /// </summary>
    public class SafeIntPtr : IDisposable
    {
        //actual pointer to unmanaged memory
        private IntPtr ptr;


        /// <summary>
        /// Initialize SafeIntPtr class.
        /// </summary>
        /// <param name="p">A pointer, which will be released at disposing.</param>
        public SafeIntPtr(IntPtr p)
        {
            ptr = p;
        }


        /// <summary>
        /// Get the value of SafeIntPtr.
        /// </summary>
        public IntPtr Value
        {
            get
            {
                return ptr;
            }
        }


        /// <summary>
        /// Implicit convert from SafeIntPtr to IntPtr.
        /// </summary>
        /// <param name="p">A SafeIntPtr.</param>
        /// <returns>Converted IntPtr.</returns>
        public static implicit operator IntPtr(SafeIntPtr p)
        {
            return p.ptr;
        }


        /// <summary>
        /// Implicit convert from IntPtr to SafeIntPtr.
        /// </summary>
        /// <param name="p">A IntPtr.</param>
        /// <returns>Converted SafeIntPtr.</returns>
        public static implicit operator SafeIntPtr(IntPtr p)
        {
            return new SafeIntPtr(p);
        }


        #region IDisposable Members

        /// <summary>
        /// Dispose method.
        /// </summary>
        [SecurityPermission(SecurityAction.Demand)]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">
        /// True to release both managed and unmanaged resources.<para/>
        /// False to release unmanaged resources only.
        /// </param>
        [SecurityPermission(SecurityAction.Demand)]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
            }

            // Release unmanaged resources.
            if (ptr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(ptr);
                ptr = IntPtr.Zero;
            }
        }


        /// <summary>
        /// finalizer
        /// </summary>
        [SecurityPermission(SecurityAction.Demand)]
        ~SafeIntPtr()
        {
            Dispose(false);
        }

        #endregion
    }
}
