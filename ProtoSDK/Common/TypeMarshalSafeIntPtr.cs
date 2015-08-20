// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Runtime.Marshaling;
using System.Security.Permissions;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// SafeIntPtr for TypeMarshal
    /// </summary>
    public class TypeMarshalSafeIntPtr : SafeIntPtr
    {
        /// <summary>
        /// marshaller
        /// </summary>
        private Marshaler marshaller;


        /// <summary>
        /// Create in instance of TypeMarshalSafeIntPtr.
        /// </summary>
        /// <param name="marshaller">The marshaller.</param>
        /// <param name="ptr">The pointer of unmanaged memory.</param>
        internal TypeMarshalSafeIntPtr(Marshaler marshaller, IntPtr ptr)
            : base(ptr)
        {
            this.marshaller = marshaller;
        }


        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">
        /// True to release both managed and unmanaged resources.<para/>
        /// False to release unmanaged resources only.
        /// </param>
        [SecurityPermission(SecurityAction.Demand)]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
                if (marshaller != null)
                {
                    marshaller.FreeMemory();
                    marshaller.Dispose();
                    marshaller = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
