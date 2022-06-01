// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// WSP buffer used to marshall.
    /// </summary>
    public class WspBuffer
    {
        private class DynamicByteArray
        {
            private const int initialSize = 64;

            public byte[] Buffer;

            public int Size { get; private set; }

            public DynamicByteArray()
            {
                Buffer = new byte[initialSize];
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
            public string StringValue;
        }

        private DynamicByteArray buffer;

        #region Properties

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

        #endregion

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
            var stringBytes = TypeMarshal.ToBytes(new StringHelper { StringValue = s });

            if (!isNullTerminated)
            {
                stringBytes = stringBytes.Take(stringBytes.Length - 2).ToArray();
            }

            AddRange(stringBytes);
        }

        /// <summary>
        /// Marshal a generic struct to buffer.
        /// </summary>
        /// <typeparam name="T">The struct to be added.</typeparam>
        /// <param name="t">A generic struct support marshalling.</param>
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
            AlignWrite(alignment);

            AddRange(TypeMarshal.ToBytes(t));
        }

        /// <summary>
        /// Align the buffer to next aligned position for write.
        /// </summary>
        /// <param name="alignment">The alignment to be used.</param>
        public void AlignWrite(int alignment)
        {
            while (WriteOffset % alignment != 0)
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

        /// <summary>
        /// Unmarshall a generic struct from buffer.
        /// </summary>
        /// <typeparam name="T">A generic struct support unmarshalling.</typeparam>
        /// <returns>The struct unmarshalled from buffer.</returns>
        public T ToStruct<T>() where T : struct
        {
            int offset = ReadOffset;
            var result = TypeMarshal.ToStruct<T>(buffer.Buffer, ref offset);
            ReadOffset = offset;
            return result;
        }

        /// <summary>
        /// Align the buffer to next aligned position for read.
        /// </summary>
        /// <param name="alignment">The alignment to be used.</param>
        public void AlignRead(int alignment)
        {
            while (ReadOffset % alignment != 0)
            {
                ReadOffset++;
            }
        }

        /// <summary>
        /// Peeks the value at given offset in the stream without changing ReadOffset.
        /// </summary>
        public T Peek<T>(int offset) where T : struct
        {
            var result = TypeMarshal.ToStruct<T>(buffer.Buffer, ref offset);
            return result;
        }

        /// <summary>
        /// Read a specific amount of byte array from a specific offset.
        /// </summary>
        public byte[] ReadBytesFromOffset(int offset, int count)
        {
            var result = new byte[count];
            Buffer.BlockCopy(buffer.Buffer, offset, result, 0, count);
            return result;
        }

        /// <summary>
        /// Read a specific amount of byte array.
        /// </summary>
        public byte[] ReadBytes(int count)
        {
            var result = ReadBytesFromOffset(ReadOffset, count);
            ReadOffset += count;
            return result;
        }
    }
}
