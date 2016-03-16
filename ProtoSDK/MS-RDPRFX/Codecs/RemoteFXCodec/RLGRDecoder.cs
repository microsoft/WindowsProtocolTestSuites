// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The RLGR decoder
    /// </summary>
    public class RLGRDecoder
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

        /// <summary>
        /// ALGR decode the input data.
        /// </summary>
        /// <param name="encodedData">The input data to be decoded.</param>
        /// <param name="rlgrMode">The RLGR mode.</param>
        /// <param name="lengthToDecode">The expected decoding size.</param>
        /// <returns></returns>
        public short[] Decode(byte[] encodedData, EntropyAlgorithm rlgrMode, int lengthToDecode)
        {
            encodedBytes = encodedData;
            //bitsToDecode = new BitArray(encodedData);
            dataOffset = 0;
            decodedList = new List<short>();
            this.RLGR_Decode(rlgrMode, lengthToDecode);
            return decodedList.ToArray();
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
        // Returns the least number of bits required to represent a given value
        // 
        uint GetMinBits(
            uint val// returns ceil(log2(val))
            )
        {
            uint m1 = (uint)Math.Ceiling(Math.Log(val, 2));
            while ((val >> (int)m1) != 0)
            {
                m1++;
            }
            return m1;
        }

        // 
        // Converts from (2 * magnitude - sign) to integer
        //
        int GetIntFrom2MagSign(
            uint twoMs
            )
        {

            uint sign = twoMs & 1;
            int vl = (int)(twoMs + sign) / 2;
            if (sign == 1) vl *= (-1); //<0
            return vl;
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

        //
        // Outputs the Golomb/Rice encoding of a non-negative integer
        //
        uint GetGRCode(
            ref int krp,
            ref int kr
            )
        {
            uint vk;
            uint mag;

            // chew up/count leading 1s and escape 0
            for (vk = 0; GetBits(1) == 1; )
            {
                vk++;
            }

            // get next *kr bits, and combine with leading 1s
            mag = (uint)((vk << kr) | GetBits((uint)(kr)));


            // adjust kpr and kr based on vk
            if (vk == 0)
            {
                kr = UpdateParam(ref krp, -2);
            }
            else if (vk != 1)// at 1, no change!
            {
                kr = UpdateParam(ref krp, (int)vk);
            }

            return (mag);
        }

        //
        // Routine that reads and decodes stream of RLGR data
        //
        void RLGR_Decode(
            EntropyAlgorithm rlgrMode,    // RLGR1 || RLGR3
            int lenToDecode
            )
        {
            int termsToDecode = lenToDecode;
            // initialize the parameters
            int k = 1;
            int kp = k << LSGR;
            int kr = 1;
            int krp = kr << LSGR;

            while (termsToDecode > 0)
            {
                int run;

                if (k != 0)
                {
                    // RL MODE
                    while (GetBits(1) == 0)
                    {
                        if (termsToDecode > 0)
                        {
                            // we have an RL escape "0", which translates to a run (1<<k) of zeros
                            WriteZeroes((uint)(1 << k), ref termsToDecode);
                            k = UpdateParam(ref kp, UP_GR);  // raise k and kp up because of zero run
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (termsToDecode > 0)
                    {
                        // next k bits will contain remaining run of zeros
                        run = (int)GetBits((uint)k);
                        WriteZeroes((uint)run, ref termsToDecode);
                    }

                    if (termsToDecode > 0)
                    {
                        // get nonzero value, starting with sign bit and 
                        // then GRCode for magnitude - 1
                        uint sign = GetBits(1);

                        // magnitude - 1 was coded (because it was nonzero)
                        int mag = (int)GetGRCode(ref krp, ref kr) + 1;

                        WriteValue(sign != 0 ? -mag : mag, ref termsToDecode);
                        k = UpdateParam(ref kp, -DN_GR); // lower k and kp because of nonzero term
                    }
                }
                else
                {
                    // GR (GOLOMB-RICE) MODE
                    uint mag = GetGRCode(ref krp, ref kr); // values coded are 2*magnitude - sign

                    if (rlgrMode == EntropyAlgorithm.CLW_ENTROPY_RLGR1)
                    {
                        if (mag == 0)
                        {
                            WriteValue(0, ref termsToDecode);
                            k = UpdateParam(ref kp, UQ_GR); // raise k and kp due to zero
                        }
                        else
                        {
                            WriteValue(GetIntFrom2MagSign(mag), ref termsToDecode);
                            k = UpdateParam(ref kp, -DQ_GR);  // lower k and kp due to nonzero
                        }

                    }
                    else // rlgrMode == RLGR3
                    {
                        // In GR mode FOR RLGR3, we have encoded the 
                        // sum of two (2*mag - sign) values

                        // maximum possible bits for first term
                        uint nIdx = GetMinBits(mag);

                        // decode val1 is first term's (2*mag - sign) value
                        uint val1 = GetBits(nIdx);

                        // val2 is second term's (2*mag - sign) value
                        uint val2 = mag - val1;

                        if (val1 != 0 && val2 != 0)
                        {
                            // raise k and kp if both terms nonzero
                            k = UpdateParam(ref kp, -2 * DQ_GR);
                        }
                        else if (val1 == 0 && val2 == 0)
                        {
                            // lower k and kp if both terms zero
                            k = UpdateParam(ref kp, 2 * UQ_GR);
                        }


                        WriteValue(GetIntFrom2MagSign(val1), ref termsToDecode);
                        if (termsToDecode > 0)
                        {
                            WriteValue(GetIntFrom2MagSign(val2), ref termsToDecode);
                        }
                    }
                }
            }
        }

    }
}
