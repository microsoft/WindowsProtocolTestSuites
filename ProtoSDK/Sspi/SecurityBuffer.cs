// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// The SecBuffer structure describes a buffer allocated by a transport application to pass to a security package.
    /// reference: http://msdn.microsoft.com/en-us/library/aa379814(VS.85).aspx
    /// </summary>
    public class SecurityBuffer
    {
        /// <summary>
        /// Bit flags that indicate the type of buffer.
        /// </summary>
        public SecurityBufferType BufferType;

        /// <summary>
        /// A buffer contains data.
        /// </summary>
        public byte[] Buffer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bufferType">SecBuffer type</param>
        /// <param name="buffer">SecBuffer in bytes.</param>
        public SecurityBuffer(SecurityBufferType bufferType, byte[] buffer)
        {
            this.BufferType = bufferType;
            this.Buffer = buffer;
        }
    }
}
