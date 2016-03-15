// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    /// <summary>
    /// The BasicTypeDecoder class provide basic decode functionality.
    /// </summary>
    public class BasicTypeDecoder
    {
        #region fields
        /// <summary>
        /// The byte stream to be decoded.
        /// </summary>
        protected byte[] decodeData;
        /// <summary>
        /// The next position to be decoded in byte stream.
        /// </summary>
        protected uint offset;

        #endregion fields

        #region method

        /// <summary>
        /// Constructor, initialize the data to be decoded and offset
        /// </summary>
        /// <param name = "Data"> the byte stream to be decoded </param>
        public BasicTypeDecoder(byte[] Data)
        {
            decodeData = Data;
            offset = 0;
        }

        /// <summary>
        /// read a byte from decoder
        /// </summary>
        /// <param name = "value"> specify where the decoded byte value to save  </param>
        /// <return> true if decode success, otherwise return false </return>
        public bool DecodeByte(ref byte value)
        {
            if (offset >= decodeData.Length)
            {   // reach to byte stream termination
                return false;
            }
            else
            {
                value = decodeData[offset];
                offset++;
                return true;
            }
        }

        /// <summary>
        /// read a ushort from decoder
        /// </summary>
        /// <param name = "value"> specify where the decoded ushort value to save  </param>
        /// <return> true if decode success, otherwise return false </return>
        public bool DecodeUShort(ref ushort value)
        {
            if (offset + 1 >= decodeData.Length)
            {   // reach to byte stream termination
                return false;
            }
            else
            {
                value = (ushort)decodeData[offset];
                offset += 2;
                return true;
            }
        }

        /// <summary>
        /// read a uint from decoder
        /// </summary>
        /// <param name = "value"> specify where the decoded uint value to save  </param>
        /// <return> true if decode success, otherwise return false </return>
        public bool DecodeUInt(ref uint value)
        {
            if (offset + 3 >= decodeData.Length)
            {   // reach to byte stream termination
                return false;
            }
            else
            {
                value = (uint)decodeData[offset];
                offset += 4;
                return true;
            }
        }

        /// <summary>
        /// read a run length factor from decoder
        /// </summary>
        /// <param name = "value"> specify where the decoded run length factor to save  </param>
        /// <return> true if decode success, otherwise return false </return>
        public bool DecodeRulLengthFactor(ref uint rlFactor)
        {
            byte rlfactor1 = 0;
            if (!DecodeByte(ref rlfactor1)) return false;

            if (rlfactor1 < 0xff)   // 1 byte rlfactor
                rlFactor = rlfactor1;
            else           // 2 or 4 bytes rlfactor
            {
                ushort rlfactor2 = 0;
                if (!DecodeUShort(ref rlfactor2)) return false;

                if (rlfactor2 < 0xffff)     // 2 bytes rlfactor
                    rlFactor = rlfactor2;
                else  // 4 bytes rlfactor
                {
                    uint rlfactor3 = 0;
                    if (!DecodeUInt(ref rlfactor3)) return false;
                    rlFactor = rlfactor3;    
                }
            }
            return true;
        }
        #endregion method
    }
}
