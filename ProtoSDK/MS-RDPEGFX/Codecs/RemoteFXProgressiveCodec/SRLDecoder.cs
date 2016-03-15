// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// Simplified Run-Length (SRL) Decoder 
    /// </summary>
    public class SRLDecoder
    {
        // Constants used within the RLGR1/RLGR3 algorithm
        const int KPMAX = 80;  // max value for kp or krp
        const int LSGR = 3;  // shift count to convert kp to k
        const int UP_GR = 4;  // increase in kp after a zero run in RL mode
        const int DN_GR = 6;  // decrease in kp after a nonzero symbol in RL mode
        const int UQ_GR = 3;   // increase in kp after nonzero symbol in GR mode
        const int DQ_GR = 3;   // decrease in kp after zero symbol in GR mode

        List<short> decodedList;
        byte[] encodedBytes = null;
        //BitArray bitsToDecode = null;
        int dataOffset = 0;

        int k = 1;
        int kp = 8; // k << LSGR;
        int termsToDecode = RdpegfxTileUtils.ComponentElementCount;
        bool zeroRLTerminated;

        /// <summary>
        /// SRL decode the input data.
        /// </summary>
        /// <param name="encodedData">The input data to be decoded.</param>
        public SRLDecoder(byte[] encodedData)
        {
            encodedBytes = encodedData;
            //bitsToDecode = new BitArray(encodedData);
            decodedList = new List<short>();
            dataOffset = 0;
            zeroRLTerminated = false;
        }

        //
        // Gets (returns) the next nBits from the bitstream
        // The layout of N bits in the bitstream with regard to a byte array is: 
        //     [0..N] -> [0..7](MSB..LSB),[8..15](MSB..LSB) ...,
        // where (MSB..LSB) denotes a byte.  
        //
        uint GetBits(uint nBits)
        {
            uint output = 0;
            int outOffset = (int)nBits - 1;
            while (outOffset >= 0)
            {
                int bitOffset = dataOffset & 7;
                int byteOffset = dataOffset >> 3;
                //uint outBit = (uint)(bitsToDecode.Get(bitOffset++) ? 1:0);
                uint outBit = (uint)((encodedBytes[byteOffset] & (byte)(1 << (7 - bitOffset))) == 0 ? 0 : 1);
                output |= (outBit << outOffset--);
                dataOffset++;
            }
            return output;
        }

        bool hasMoreBits(int bitCount)
        {
            int targetOffset = dataOffset + bitCount;

            int bitOffset = targetOffset & 7;
            int byteOffset = targetOffset >> 3;
            if (byteOffset < encodedBytes.Length) return true;
            return false;
        }

        //
        // From current output pointer, write "value", check and update *termsToDecode
        // 
        void WriteValue(
            int value,
            ref int termsToDecode
            )
        {
            if (termsToDecode > 0)
            {
                this.decodedList.Add((short)value);
                termsToDecode--;
            }
        }


        //
        // From current output pointer, write next nZeroes terms with value 0; 
        // check and update *termsToDecode
        //
        void WriteZeroes(
            uint nZeroes,
            ref int termsToDecode
            )
        {
            for (int i = 0; i < nZeroes && termsToDecode > 0; i++)
            {
                WriteValue(0, ref termsToDecode);
            }
        }


        //
        // Update the passed parameter and clamp it to the range [0,KPMAX]
        // Return the value of parameter right-shifted by LSGR
        //
        int UpdateParam(
            ref int param,    // parameter to update
            int deltaP    // update delta
            )
        {
            param += deltaP;// adjust parameter
            if (param > KPMAX) param = KPMAX;// max clamp
            if (param < 0) param = 0;// min clamp
            return (param >> LSGR);
        }

        
        /// <summary>
        /// Decode one element from the given data
        /// </summary>
        /// <param name="bitCount">The count of bits to be decoded.</param>
        /// <returns>The decoded element.</returns>
        public short? DecodeOne(int bitCount)
        {
            short? decodedValue = null;
            if (this.decodedList.Count > 0)
            {
                decodedValue = this.decodedList[0];
                this.decodedList.RemoveAt(0);
                return decodedValue;
            }

            if (hasMoreBits(1))
            {
                if (!zeroRLTerminated)
                {
                    int run;
                    // RL MODE
                    while (hasMoreBits(1) && GetBits(1) == 0)
                    {
                        // we have an RL escape "0", which translates to a run (1<<k) of zeros
                        WriteZeroes((uint)(1 << k), ref termsToDecode);
                        k = UpdateParam(ref kp, UP_GR);  // raise k and kp up because of zero run
                    }

                    if (hasMoreBits(k))
                    {
                        // next k bits will contain remaining run of zeros
                        run = (int)GetBits((uint)k);
                        WriteZeroes((uint)run, ref termsToDecode);
                        k = UpdateParam(ref kp, -DN_GR);
                    }

                    zeroRLTerminated = true;
                }
                else
                {
                    // get nonzero value, starting with sign bit and 
                    // then Unary Code for magnitude - 1
                    uint sign = GetBits(1);

                    int mag = 0;
                    int maxReadBits = (1 << bitCount) - 2;

                    while (hasMoreBits(1) && maxReadBits > 0 && GetBits(1) == 0)
                    {
                        maxReadBits--;
                        mag++;
                    }

                    mag += 1;
                    WriteValue(sign != 0 ? -mag : mag, ref termsToDecode);

                    zeroRLTerminated = false;
                }

            }

            if (this.decodedList.Count > 0)
            {
                decodedValue = this.decodedList[0];
                this.decodedList.RemoveAt(0);
            }
            else if (hasMoreBits(1))
            {
                decodedValue = DecodeOne(bitCount);
            }

            return decodedValue;
        }
    }
}
