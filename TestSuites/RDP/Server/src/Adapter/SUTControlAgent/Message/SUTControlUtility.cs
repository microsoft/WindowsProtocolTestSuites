// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestSuites.Rdp.SUTControlAgent.Message
{
    public class SUTControlUtility
    {
        public static byte[] ChangeBytesOrderForNumeric(byte[] originalData, bool isLittleEndian = true)
        {
            if (BitConverter.IsLittleEndian != isLittleEndian)
            {
                Array.Reverse(originalData);
            }
            return originalData;
        }

        public static byte[] GetNumericBytes(byte[] rawData, int index, int length, bool isLittleEndian = true)
        {
            byte[] buffer = new byte[length];
            Array.Copy(rawData, index, buffer, 0, length);
            if (BitConverter.IsLittleEndian != isLittleEndian)
            {
                Array.Reverse(buffer);
            }
            return buffer;
        }
    }
}
