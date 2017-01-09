// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents an BER ASN.1 encoding buffer.
    /// </summary>
    /// <remarks>
    /// This buffer is a reversed buffer.
    /// The data will be written to the front of the buffer each time its "Write" or "WriteByte" is invoked.
    /// </remarks>
    public class Asn1BerEncodingBuffer : IAsn1BerEncodingBuffer, IDisposable
    {

        private MemoryStream buffer;

        /// <summary>
        /// Initializes a new instance of the Asn1ReversedEncodingBuffer class that is empty and has a default capacity.
        /// </summary>
        /// <remarks>
        /// The capacity can expand automatically.
        /// </remarks>
        public Asn1BerEncodingBuffer()
        {
            buffer = new MemoryStream();
        }

        /// <summary>
        /// Initializes a new instance of the Asn1ReversedEncodingBuffer class that is empty and has a user defined capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity.</param>
        /// <remarks>
        /// The capacity can expand automatically.
        /// </remarks>
        public Asn1BerEncodingBuffer(int capacity)
        {
            buffer = new MemoryStream(capacity);
        }

        /// <summary>
        /// Gets the encoding result.
        /// </summary>
        public byte[] Data
        {
            get
            {
                byte[] result = buffer.ToArray();
                Array.Reverse(result);
                return result;
            }
        }

        /// <summary>
        /// Writes a byte to the front of Data property.
        /// </summary>
        public void WriteByte(byte b)
        {
            //After Array.Reverse in Data property, its in the front.
            buffer.WriteByte(b);
        }

        /// <summary>
        /// Writes some bytes to the front of the buffer.
        /// </summary>
        /// <param name="bytes">An array that contains the bytes.</param>
        /// <param name="offset">The begin index of the bytes in the array.</param>
        /// <param name="count">The number of the bytes.</param>
        public void WriteBytes(byte[] bytes, int offset, int count)
        {
            //Write inversely. After Array.Reverse in Data property, the original order is retrieved.
            for (int i = offset + count - 1; i >= offset; i--)
            {
                buffer.WriteByte(bytes[i]);
            }
        }

        /// <summary>
        /// Write all the bytes in a byte array to the front of the buffer.
        /// </summary>
        /// <param name="bytes"></param>
        public void WriteBytes(byte[] bytes)
        {
            if (bytes != null)
            {
                WriteBytes(bytes, 0, bytes.Length);
            }
        }


        #region Dispose

        private bool disposed = false;

        /// <summary>
        /// Dispose method from IDisposable.
        /// </summary>
        /// <param name="disposing">Indicating whether the managed resources need to be disposed immediately.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //managed resources are disposed here
                    buffer.Dispose();
                }

                //unmanaged resources are disposed here

                this.disposed = true;
            }
        }

        /// <summary>
        /// Dispose method that can be manually called.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destructor which disposes the unmanaged resources.
        /// </summary>
        ~Asn1BerEncodingBuffer()
        {
            Dispose(false);
        }

        #endregion Dispose
    }
}
