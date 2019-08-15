// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// WSP buffer used to marshall.
    /// </summary>
    public class WspBuffer
    {
        private class DynamicByteArray
        {
            private const int InitialSize = 64;

            public byte[] Buffer;

            public DynamicByteArray()
            {
                Buffer = new byte[InitialSize];
                Size = 0;
            }

            public DynamicByteArray(byte[] bytes)
            {
                Buffer = bytes.ToArray();
                Size = bytes.Length;
            }

            public void AddByte(byte b)
            {
                GrowBuffer(Size + 1);

                Buffer[Size] = b;

                Size++;
            }

            public void AddRange(byte[] bytes)
            {
                GrowBuffer(Size + bytes.Length);

                Array.Copy(bytes, 0, Buffer, Size, bytes.Length);

                Size += bytes.Length;
            }

            public int Size { get; private set; }

            private void GrowBuffer(int newSize)
            {
                int bufferSize = Buffer.Length;

                if (bufferSize >= newSize)
                {
                    return;
                }

                while (bufferSize < newSize)
                {
                    // double each time
                    bufferSize *= 2;
                }

                var newBuffer = new byte[bufferSize];

                // copy original data
                Array.Copy(Buffer, 0, newBuffer, 0, Size);

                Buffer = newBuffer;
            }
        }

        private struct StringHelper
        {
            public string s;
        }

        private DynamicByteArray buffer;

        /// <summary>
        /// The current write buffer into buffer.
        /// </summary>
        public int ReadOffset { get; private set; }

        /// <summary>
        /// The current write offset into buffer.
        /// </summary>
        public int WriteOffset
        {
            get
            {
                return buffer.Size;
            }
        }

        #region Constructors
        /// <summary>
        /// Construct an empty WSP buffer.
        /// </summary>
        public WspBuffer()
        {
            ReadOffset = 0;
            buffer = new DynamicByteArray();
        }

        /// <summary>
        /// Construct a WSP buffer.
        /// </summary>
        /// <param name="bytes">Initial data in buffer.</param>
        public WspBuffer(byte[] bytes)
        {
            ReadOffset = 0;
            buffer = new DynamicByteArray(bytes);
        }
        #endregion

        /// <summary>
        /// Add a range of bytes to buffer.
        /// </summary>
        /// <param name="bytes">Bytes to be added.</param>
        public void AddRange(byte[] bytes)
        {
            buffer.AddRange(bytes);
        }

        /// <summary>
        /// Marshall a string to buffer.
        /// </summary>
        /// <param name="s">The string to be added.</param>
        /// <param name="isNullTerminated">Whether to append the null character.</param>
        public void AddUnicodeString(string s, bool isNullTerminated = true)
        {
            var stringBytes = TypeMarshal.ToBytes(new StringHelper { s = s });

            if (!isNullTerminated)
            {
                stringBytes = stringBytes.Take(stringBytes.Length - 2).ToArray();
            }

            AddRange(stringBytes);
        }

        /// <summary>
        /// Marshal a generic object to buffer.
        /// </summary>
        /// <typeparam name="T">The object to be added.</typeparam>
        /// <param name="t"></param>
        public void Add<T>(T t) where T : struct
        {
            AddRange(TypeMarshal.ToBytes(t));
        }

        /// <summary>
        /// Marshal a generic object to buffer with alignment.
        /// </summary>
        /// <typeparam name="T">The object to be added.</typeparam>
        /// <param name="t"></param>
        /// <param name="alignment">The alignment to be used.</param>
        public void Add<T>(T t, int alignment) where T : struct
        {
            Align(alignment);

            AddRange(TypeMarshal.ToBytes(t));
        }

        /// <summary>
        /// Align the buffer to next aligned position.
        /// </summary>
        /// <param name="alignment">The alignment to be used.</param>
        public void Align(int alignment)
        {
            while (buffer.Size % alignment != 0)
            {
                buffer.AddByte(0);
            }
        }

        /// <summary>
        /// Get the bytes in buffer.
        /// </summary>
        /// <returns>A byte array containing bytes in buffer.</returns>
        public byte[] GetBytes()
        {
            return buffer.Buffer.Take(buffer.Size).ToArray();
        }

        public T ToStruct<T>() where T : struct
        {
            int offset = ReadOffset;
            var result = TypeMarshal.ToStruct<T>(buffer.Buffer, ref offset);
            ReadOffset = offset;
            return result;
        }
    }
}
