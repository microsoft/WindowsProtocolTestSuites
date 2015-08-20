// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// A CRC32 Implementation
    /// </summary>
    public static class Crc32
    {
        //contain all possible value's remainder for fast computing.
        //because one byte is 8-bits long, it has 256 different values
        private static uint[] crcTable = new uint[256];

        // official polynomial used by Ethernet,zip
        private const uint ploynomial = 0x4c11db7;

        // the bits count of crc32 code
        private const int numBitsOfCrc32 = 32;

        // the bits count of one byte
        private const int numBitsOfByte = 8;

        
        /// <summary>
        /// Initialize static field
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static Crc32()
        {
            CrcInit();
        }


        /// <summary>
        /// Compute crc code for the input data, reverseChecksume as default.
        /// </summary>
        /// <param name="data">input data</param>
        /// <returns>crc value</returns>
        public static byte[] ComputeChecksum(byte[] data)
        {
            return ComputeChecksum(data, true);
        }


        /// <summary>
        /// Compute crc code for the input data
        /// </summary>
        /// <param name="data">input data</param>
        /// <param name="reverseChecksum">Whether reverse checksum before and after computing, KILE use un-reversed version
        /// and Nlmp use reversed version</param>
        /// <returns>crc value</returns>
        public static byte[] ComputeChecksum(byte[] data, bool reverseChecksum)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            uint outCrc = 0;

            //Nlmp used crc32 need to reverse the checksum and KILE don't
            if (reverseChecksum)
            {
                outCrc ^= 0xffffffff;
            }

            for (int i = 0; i < data.Length; i++)
            {
                outCrc = (outCrc >> numBitsOfByte) ^ crcTable[(outCrc & 0xff) ^ data[i]];
            }

            //Nlmp used crc32 need to reverse the checksum and KILE don't
            if (reverseChecksum)
            {
                //reverse every bit of outCrc
                outCrc ^= 0xffffffff;
            }

            return BitConverter.GetBytes(outCrc);
        }


        /// <summary>
        /// reverse bit order of the data
        /// </summary>
        /// <param name="data">the data to be reversed</param>
        /// <param name="bitsCount">the bit count of the data</param>
        /// <returns>the reversed data</returns>
        private static uint Reverse(uint data, byte bitsCount)
        {
            uint reversedData = 0;

            //change the bit order of data, for example:
            //if data is 8-bits long , then swap bit 7 with bit 0, 
            //bit 6 with bit 1, ...
            for (byte i = 0; i < bitsCount; i++)
            {
                // if current bit is 1, then set bit (bitsCount-i-1) to 1;
                // else set to 0, because reflection is initialized to zero, so 
                // we don't need to set to zero explicitly
                if ((data & 0x01) != 0)
                {
                    reversedData |= (uint)(1 << ((bitsCount - 1) - i));
                }

                //remove the checked bit from data. put the bit waiting for check 
                //to the rightest position.
                data = (data >> 1);
            }

            return reversedData;
        }


        /// <summary>
        /// Initialize crc table 
        /// </summary>
        private static void CrcInit()
        {
            //range of byte is from 0 to 255
            for (int i = 0; i <= 0xff; i++)
            {
                crcTable[i] = Reverse((uint)i, numBitsOfByte) << (numBitsOfCrc32 - numBitsOfByte);

                for (int j = 0; j < numBitsOfByte; j++)
                {
                    crcTable[i] = (crcTable[i] << 1) ^
                        (((crcTable[i] & (1 << (numBitsOfCrc32 - 1))) != 0) ? ploynomial : 0);
                }

                //type of value in crcTable is int, it has 32-bits
                crcTable[i] = Reverse(crcTable[i], 32);
            }
        }
    }
}
