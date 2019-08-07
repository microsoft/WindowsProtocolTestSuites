// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// WSP buffer used to marshall.
    /// </summary>
    public class WSPBuffer
    {
        private struct StringHelper
        {
            public string s;
        }

        private List<byte> buffer;

        /// <summary>
        /// The current offset into buffer.
        /// </summary>
        public int Offset
        {
            get
            {
                return buffer.Count;
            }
        }

        public WSPBuffer()
        {
            buffer = new List<byte>();
        }

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
        public void Add(string s)
        {
            AddRange(TypeMarshal.ToBytes(new StringHelper { s = s }));
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
            while (buffer.Count % alignment != 0)
            {
                buffer.Add(0);
            }
        }

        /// <summary>
        /// Get the bytes in buffer.
        /// </summary>
        /// <returns>A byte array containing bytes in buffer.</returns>
        public byte[] GetBytes()
        {
            return buffer.ToArray();
        }
    }
}
