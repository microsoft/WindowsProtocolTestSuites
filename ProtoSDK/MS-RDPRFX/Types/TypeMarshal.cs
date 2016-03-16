// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// TypeMarshal is a type marshal class.
    /// It can marshal a struct to block memory.
    /// </summary>
    internal static class TypeMarshal
    {
        /// <summary>
        /// Marshal a struct to managed byte array.
        /// </summary>
        /// <typeparam name="T">Type of struct.</typeparam>
        /// <param name="t">A struct to marshal.</param>
        /// <returns>Marshalled managed byte array.</returns>
        public static byte[] ToBytes<T>(T t) where T : struct
        {
            int size;
            using (MessageUtils utils = new MessageUtils(null))
            {
                size = utils.GetSize(t);
            }

            // The size returned by MessageUtils.GetSize is not always
            // the actual block size of the given structure,
            // so allocate a buffer whose size is twice as large as it.
            // The actual size will be calculated later.
            byte[] buf = new byte[size * 2];
            using (MemoryStream memoryStream = new MemoryStream(buf))
            {
                using (Channel channel = new Channel(null, memoryStream))
                {
                    channel.BeginWriteGroup();
                    channel.Write<T>(t);
                    channel.EndWriteGroup();
                    int actualSize = (int)channel.Stream.Position;
                    byte[] actualBuf = new byte[actualSize];
                    Array.Copy(buf, 0, actualBuf, 0, actualSize);
                    return actualBuf;
                }
            }
        }
    }

    public interface IMarshalable
    {
        byte[] ToBytes();
    }
}
