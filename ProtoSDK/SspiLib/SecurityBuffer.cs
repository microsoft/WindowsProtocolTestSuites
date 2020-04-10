using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib
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
