// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpei
{
    #region Common Data Types

    /// <summary>
    /// The TWO_BYTE_UNSIGNED_INTEGER structure is used to encode a value in the range 0x0000 to 0x7FFF by using a variable number of bytes. 
    /// </summary>
    public class TWO_BYTE_UNSIGNED_INTEGER
    {
        /// <summary>
        /// A 1-bit unsigned integer field containing an encoded representation of the number of bytes in this structure.
        /// </summary>
        public TWO_BYTE_C_Values c;

        /// <summary>
        /// A 7-bit unsigned integer field containing the most significant 7 bits of the value represented by this structure.
        /// </summary>
        public byte val1;

        /// <summary>
        /// An optional, 8-bit unsigned integer containing the least significant bits of the value represented by this structure.
        /// </summary>
        public byte? val2;

        public TWO_BYTE_UNSIGNED_INTEGER()
        {
        }

        public TWO_BYTE_UNSIGNED_INTEGER(ushort num)
        {
            if (num > 0x7FFF)
            {
                throw new ArgumentException("The integer is out of the valid range of TWO_BYTE_UNSIGNED_INTEGER.", "num");
            }
            if (num > 0x7F)
            {
                c = TWO_BYTE_C_Values.TWO_BYTE_VAL;
                val1 = (byte)(num >> 8);
                val2 = (byte)(num & 0xFF);
            }
            else
            {
                c = TWO_BYTE_C_Values.ONE_BYTE_VAL;
                val1 = (byte)num;
            }
        }

        public ushort ToUShort()
        {
            if (c == TWO_BYTE_C_Values.ONE_BYTE_VAL)
            {
                return val1;
            }
            else
            {
                return (ushort)(((int)val1 << 8) | (int)val2);
            }
        }

        public uint Length()
        {
            return (uint)((c == TWO_BYTE_C_Values.ONE_BYTE_VAL) ? 1 : 2);
        }

        #region Encoding/Decoding

        public void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteByte((byte)((int)c << 7 | val1));
            if (c == TWO_BYTE_C_Values.TWO_BYTE_VAL)
            {
                marshaler.WriteByte((byte)val2);
            }
        }

        public bool Decode(PduMarshaler marshaler)
        {
            try
            {
                byte t = marshaler.ReadByte();
                c = (TWO_BYTE_C_Values)(t >> 7);
                val1 = (byte)(t & 0x7F);
                if (c == TWO_BYTE_C_Values.TWO_BYTE_VAL)
                {
                    val2 = marshaler.ReadByte();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }

    /// <summary>
    /// Containing an encoded representation of the number of bytes in TWO_BYTE_SIGNED_INTEGER or TWO_BYTE_UNSIGNED_INTEGER.
    /// </summary>
    public enum TWO_BYTE_C_Values : byte
    {
        /// <summary>
        /// Implies that the optional val2 field is not present. Hence, the structure is 1 byte in size.
        /// </summary>
        ONE_BYTE_VAL = 0,

        /// <summary>
        /// Implies that the optional val2 field is present. Hence, the structure is 2 bytes in size.
        /// </summary>
        TWO_BYTE_VAL = 1
    }

    /// <summary>
    /// The TWO_BYTE_SIGNED_INTEGER structure is used to encode a value in the range -0x3FFF to 0x3FFF by using a variable number of bytes.
    /// </summary>
    public class TWO_BYTE_SIGNED_INTEGER
    {
        /// <summary>
        /// A 1-bit unsigned integer field containing an encoded representation of the number of bytes in this structure.
        /// </summary>
        public TWO_BYTE_C_Values c;

        /// <summary>
        /// A 1-bit unsigned integer field containing an encoded representation of whether the value is positive or negative.
        /// </summary>
        public S_Values s;

        /// <summary>
        /// A 6-bit unsigned integer field containing the most significant 6 bits of the value represented by this structure.
        /// </summary>
        public byte val1;

        /// <summary>
        /// An optional, 8-bit unsigned integer containing the least significant bits of the value represented by this structure.
        /// </summary>
        public byte? val2;

        public TWO_BYTE_SIGNED_INTEGER()
        {
        }

        public TWO_BYTE_SIGNED_INTEGER(short num)
        {
            if (num > 0x3FFF || num < -0x3FFF)
            {
                throw new ArgumentException("The integer is out of the valid range of TWO_BYTE_SIGNED_INTEGER.", "num");
            }
            ushort n;
            if (num < 0)
            {
                n = (ushort)(-num);
                s = S_Values.NEGATIVE_VAL;
            }
            else
            {
                n = (ushort)num;
                s = S_Values.POSITIVE_VAL;
            }
            if (n > 0x3F)
            {
                c = TWO_BYTE_C_Values.TWO_BYTE_VAL;
                val1 = (byte)(n >> 8);
                val2 = (byte)n;
            }
            else
            {
                c = TWO_BYTE_C_Values.ONE_BYTE_VAL;
                val1 = (byte)n;
            }
        }

        public short ToShort()
        {
            if (c == TWO_BYTE_C_Values.ONE_BYTE_VAL)
            {
                return s == S_Values.POSITIVE_VAL ? val1 : (short)(-val1);
            }
            else
            {
                short t = (short)(((int)val1 << 8) | (int)val2);
                return (s == S_Values.POSITIVE_VAL) ? t : (short)(-t);
            }
        }

        public uint Length()
        {
            return (uint)((c == TWO_BYTE_C_Values.ONE_BYTE_VAL) ? 1 : 2);
        }

        #region Encoding/Decoding

        public void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteByte((byte)((int)c << 7 | (int)s << 6 | val1));
            if (c == TWO_BYTE_C_Values.TWO_BYTE_VAL)
            {
                marshaler.WriteByte((byte)val2);
            }
        }

        public bool Decode(PduMarshaler marshaler)
        {
            try
            {
                byte t = marshaler.ReadByte();
                c = (TWO_BYTE_C_Values)(t >> 7);
                s = (S_Values)(t >> 6 & 0x40);
                val1 = (byte)(t & 0x3F);
                if (c == TWO_BYTE_C_Values.TWO_BYTE_VAL)
                {
                    val2 = marshaler.ReadByte();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }

    /// <summary>
    /// Containing an encoded representation of whether the value of FOUR_BYTE_SIGNED_INTEGER or TWO_BYTE_SIGNED_INTEGER is positive or negative.
    /// </summary>
    public enum S_Values : byte
    {
        /// <summary>
        /// Implies that the value represented by this structure is positive.
        /// </summary>
        POSITIVE_VAL = 0,

        /// <summary>
        /// Implies that the value represented by this structure is negative.
        /// </summary>
        NEGATIVE_VAL = 1
    }

    /// <summary>
    /// The FOUR_BYTE_UNSIGNED_INTEGER structure is used to encode a value in the range 0x00000000 to 0x3FFFFFFF by using a variable number of bytes. 
    /// </summary>
    public class FOUR_BYTE_UNSIGNED_INTEGER
    {
        /// <summary>
        /// A 2-bit unsigned integer field containing an encoded representation of the number of bytes in this structure.
        /// </summary>
        public FOUR_BYTE_C_Values c;

        /// <summary>
        /// A 6-bit unsigned integer field containing the most significant 6 bits of the value represented by this structure.
        /// </summary>
        public byte val1;

        /// <summary>
        /// An optional, 8-bit unsigned integer containing the second most significant bits of the value represented by this structure.
        /// </summary>
        public byte? val2;

        /// <summary>
        /// An optional, 8-bit unsigned integer containing the third most significant bits of the value represented by this structure.
        /// </summary>
        public byte? val3;

        /// <summary>
        /// An optional, 8-bit unsigned integer containing the least significant bits of the value represented by this structure.
        /// </summary>
        public byte? val4;

        public FOUR_BYTE_UNSIGNED_INTEGER()
        {
        }

        public FOUR_BYTE_UNSIGNED_INTEGER(uint num)
        {
            if (num > 0x3FFFFFFF)
            {
                throw new ArgumentException("The integer is out of the valid range of FOUR_BYTE_UNSIGNED_INTEGER.", "num");
            }

            if (num <= 0x3F)
            {
                c = FOUR_BYTE_C_Values.ONE_BYTE_VAL;
                val1 = (byte)num;
            }
            else if (num <= 0x3FFF)
            {
                c = FOUR_BYTE_C_Values.TWO_BYTE_VAL;
                val1 = (byte)(num >> 8);
                val2 = (byte)num;
            }
            else if (num <= 0x3FFFFF)
            {
                c = FOUR_BYTE_C_Values.THREE_BYTE_VAL;
                val1 = (byte)(num >> 16);
                val2 = (byte)(num >> 8);
                val3 = (byte)num;
            }
            else
            {
                c = FOUR_BYTE_C_Values.FOUR_BYTE_VAL;
                val1 = (byte)(num >> 24);
                val2 = (byte)(num >> 16);
                val3 = (byte)(num >> 8);
                val4 = (byte)num;
            }
        }

        public uint ToUInt()
        {
            if (c == FOUR_BYTE_C_Values.ONE_BYTE_VAL)
            {
                return val1;
            }
            else if (c == FOUR_BYTE_C_Values.TWO_BYTE_VAL)
            {
                return ((uint)val1 << 8) | (uint)val2;
            }
            else if (c == FOUR_BYTE_C_Values.THREE_BYTE_VAL)
            {
                return ((uint)val1 << 16) | ((uint)val2 << 8) | (uint)val3;
            }
            else
            {
                return ((uint)val1 << 24) | ((uint)val2 << 16) | ((uint)val3 << 8) | (uint)val4;
            }
        }

        public uint Length()
        {
            switch (c)
            {
                case FOUR_BYTE_C_Values.ONE_BYTE_VAL:
                    return 1;
                case FOUR_BYTE_C_Values.TWO_BYTE_VAL:
                    return 2;
                case FOUR_BYTE_C_Values.THREE_BYTE_VAL:
                    return 3;
                default:
                    return 4;
            }
        }

        #region Encoding/Decoding

        public void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteByte((byte)((int)c << 6 | val1));
            if (c >= FOUR_BYTE_C_Values.TWO_BYTE_VAL)
            {
                marshaler.WriteByte((byte)val2);
            }
            if (c >= FOUR_BYTE_C_Values.THREE_BYTE_VAL)
            {
                marshaler.WriteByte((byte)val3);
            }
            if (c == FOUR_BYTE_C_Values.FOUR_BYTE_VAL)
            {
                marshaler.WriteByte((byte)val4);
            }
        }

        public bool Decode(PduMarshaler marshaler)
        {
            try
            {
                byte t = marshaler.ReadByte();
                c = (FOUR_BYTE_C_Values)(t >> 6);
                val1 = (byte)(t & 0x3F);
                if (c >= FOUR_BYTE_C_Values.TWO_BYTE_VAL)
                {
                    val2 = marshaler.ReadByte();
                }
                if (c >= FOUR_BYTE_C_Values.THREE_BYTE_VAL)
                {
                    val3 = marshaler.ReadByte();
                }
                if (c == FOUR_BYTE_C_Values.FOUR_BYTE_VAL)
                {
                    val4 = marshaler.ReadByte();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }

    /// <summary>
    /// Containing an encoded representation of the number of bytes in this structure FOUR_BYTE_UNSIGNED_INTEGER and FOUR_BYTE_SIGNED_INTEGER.
    /// </summary>
    public enum FOUR_BYTE_C_Values : byte
    {
        /// <summary>
        /// Implies that the optional val2, val3, and val4 fields are not present. Hence, the structure is 1 byte in size.
        /// </summary>
        ONE_BYTE_VAL = 0,

        /// <summary>
        /// Implies that the optional val2 field is present, while the optional val3, and val4 fields are not present. Hence, the structure is 2 bytes in size.
        /// </summary>
        TWO_BYTE_VAL = 1,

        /// <summary>
        /// Implies that the optional val2 and val3 fields are present, while the optional val4 field is not present. Hence, the structure is 3 bytes in size.
        /// </summary>
        THREE_BYTE_VAL = 2,

        /// <summary>
        /// Implies that the optional val2, val3, and val4 fields are all present. Hence, the structure is 4 bytes in size.
        /// </summary>
        FOUR_BYTE_VAL = 3
    }

    /// <summary>
    /// The FOUR_BYTE_SIGNED_INTEGER structure is used to encode a value in the range -0x1FFFFFFF to 0x1FFFFFFF by using a variable number of bytes. 
    /// </summary>
    public class FOUR_BYTE_SIGNED_INTEGER
    {
        /// <summary>
        /// A 2-bit unsigned integer field containing an encoded representation of the number of bytes in this structure.
        /// </summary>
        public FOUR_BYTE_C_Values c;

        /// <summary>
        /// A 1-bit unsigned integer field containing an encoded representation of whether the value is positive or negative.
        /// </summary>
        public S_Values s;

        /// <summary>
        /// A 5-bit unsigned integer field containing the most significant 5 bits of the value represented by this structure.
        /// </summary>
        public byte val1;

        /// <summary>
        /// An optional, 8-bit unsigned integer containing the second most significant bits of the value represented by this structure.
        /// </summary>
        public byte? val2;

        /// <summary>
        /// An optional, 8-bit unsigned integer containing the third most significant bits of the value represented by this structure.
        /// </summary>
        public byte? val3;

        /// <summary>
        /// An optional, 8-bit unsigned integer containing the least significant bits of the value represented by this structure.
        /// </summary>
        public byte? val4;

        public FOUR_BYTE_SIGNED_INTEGER()
        {
        }

        public FOUR_BYTE_SIGNED_INTEGER(int num)
        {
            if (num > 0x1FFFFFFF || num < -0x1FFFFFFF)
            {
                throw new ArgumentException("The integer is out of the valid range of FOUR_BYTE_SIGNED_INTEGER.", "num");
            }
            uint n;
            if (num < 0)
            {
                s = S_Values.NEGATIVE_VAL;
                n = (uint)(-num);
            }
            else
            {
                s = S_Values.POSITIVE_VAL;
                n = (uint)num;
            }
            if (n <= 0x1F)
            {
                c = FOUR_BYTE_C_Values.ONE_BYTE_VAL;
                val1 = (byte)n;
            }
            else if (n <= 0x1FFF)
            {
                c = FOUR_BYTE_C_Values.TWO_BYTE_VAL;
                val1 = (byte)(n >> 8);
                val2 = (byte)n;
            }
            else if (n <= 0x1FFFFF)
            {
                c = FOUR_BYTE_C_Values.THREE_BYTE_VAL;
                val1 = (byte)(n >> 16);
                val2 = (byte)(n >> 8);
                val3 = (byte)n;
            }
            else
            {
                c = FOUR_BYTE_C_Values.FOUR_BYTE_VAL;
                val1 = (byte)(n >> 24);
                val2 = (byte)(n >> 16);
                val3 = (byte)(n >> 8);
                val4 = (byte)n;
            }
        }

        public int ToInt()
        {
            if (c == FOUR_BYTE_C_Values.ONE_BYTE_VAL)
            {
                return s == S_Values.POSITIVE_VAL ? val1 : (-val1);
            }
            else if (c == FOUR_BYTE_C_Values.TWO_BYTE_VAL)
            {
                int t = ((int)val1 << 8) | (int)val2;
                return s == S_Values.POSITIVE_VAL ? t : (-t);
            }
            else if (c == FOUR_BYTE_C_Values.THREE_BYTE_VAL)
            {
                int t = ((int)val1 << 16) | ((int)val2 << 8) | (int)val3;
                return s == S_Values.POSITIVE_VAL ? t : (-t);
            }
            else
            {
                int t = ((int)val1 << 24) | ((int)val2 << 16) | ((int)val3 << 8) | (int)val4;
                return s == S_Values.POSITIVE_VAL ? t : (-t);
            }
        }

        public uint Length()
        {
            switch (c)
            {
                case FOUR_BYTE_C_Values.ONE_BYTE_VAL:
                    return 1;
                case FOUR_BYTE_C_Values.TWO_BYTE_VAL:
                    return 2;
                case FOUR_BYTE_C_Values.THREE_BYTE_VAL:
                    return 3;
                default:
                    return 4;
            }
        }

        #region Encoding/Decoding

        public void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteByte((byte)((int)c << 6 | (int)s << 5 | val1));
            if (c >= FOUR_BYTE_C_Values.TWO_BYTE_VAL)
            {
                marshaler.WriteByte((byte)val2);
            }
            if (c >= FOUR_BYTE_C_Values.THREE_BYTE_VAL)
            {
                marshaler.WriteByte((byte)val3);
            }
            if (c == FOUR_BYTE_C_Values.FOUR_BYTE_VAL)
            {
                marshaler.WriteByte((byte)val4);
            }
        }

        public bool Decode(PduMarshaler marshaler)
        {
            try
            {
                byte t = marshaler.ReadByte();
                c = (FOUR_BYTE_C_Values)(t >> 6);
                s = (S_Values)((t >> 5) & 0x20);
                val1 = (byte)(t & 0x1F);
                if (c >= FOUR_BYTE_C_Values.TWO_BYTE_VAL)
                {
                    val2 = marshaler.ReadByte();
                }
                if (c >= FOUR_BYTE_C_Values.THREE_BYTE_VAL)
                {
                    val3 = marshaler.ReadByte();
                }
                if (c == FOUR_BYTE_C_Values.FOUR_BYTE_VAL)
                {
                    val4 = marshaler.ReadByte();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }

    /// <summary>
    /// The EIGHT_BYTE_UNSIGNED_INTEGER structure is used to encode a value in the range 0x0000000000000000 to 0x1FFFFFFFFFFFFFFF by using a variable number of bytes. 
    /// </summary>
    public class EIGHT_BYTE_UNSIGNED_INTEGER
    {
        /// <summary>
        /// A 3-bit unsigned integer field containing an encoded representation of the number of bytes in this structure.
        /// </summary>
        public EIGHT_BYTE_C_Values c;

        /// <summary>
        /// A 5-bit unsigned integer field containing the most significant 5 bits of the value represented by this structure.
        /// </summary>
        public byte val1;

        /// <summary>
        /// An optional, 8-bit unsigned integer containing the second most significant bits of the value represented by this structure.
        /// </summary>
        public byte? val2;

        /// <summary>
        /// An optional, 8-bit unsigned integer containing the third most significant bits of the value represented by this structure.
        /// </summary>
        public byte? val3;

        /// <summary>
        /// An optional, 8-bit unsigned integer containing the fourth most significant bits of the value represented by this structure.
        /// </summary>
        public byte? val4;

        /// <summary>
        /// An optional, 8-bit unsigned integer containing the fifth most significant bits of the value represented by this structure.
        /// </summary>
        public byte? val5;

        /// <summary>
        /// An optional, 8-bit unsigned integer containing the sixth most significant bits of the value represented by this structure.
        /// </summary>
        public byte? val6;

        /// <summary>
        /// An optional, 8-bit unsigned integer containing the seventh most significant bits of the value represented by this structure.
        /// </summary>
        public byte? val7;

        /// <summary>
        /// An optional, 8-bit unsigned integer containing the least significant bits of the value represented by this structure.
        /// </summary>
        public byte? val8;

        public EIGHT_BYTE_UNSIGNED_INTEGER()
        {
        }

        public EIGHT_BYTE_UNSIGNED_INTEGER(ulong num)
        {
            if (num > 0x1FFFFFFFFFFFFFFF)
            {
                throw new ArgumentException("The integer is out of the valid range of EIGHT_BYTE_UNSIGNED_INTEGER.", "num");
            }
            if (num <= 0x1F)
            {
                c = EIGHT_BYTE_C_Values.ONE_BYTE_VAL;
                val1 = (byte)num;
            }
            else if (num <= 0x1FFF)
            {
                c = EIGHT_BYTE_C_Values.TWO_BYTE_VAL;
                val1 = (byte)(num >> 8);
                val2 = (byte)num;
            }
            else if (num <= 0x1FFFFF)
            {
                c = EIGHT_BYTE_C_Values.THREE_BYTE_VAL;
                val1 = (byte)(num >> 16);
                val2 = (byte)(num >> 8);
                val3 = (byte)num;
            }
            else if (num <= 0x1FFFFFFF)
            {
                c = EIGHT_BYTE_C_Values.FOUR_BYTE_VAL;
                val1 = (byte)(num >> 24);
                val2 = (byte)(num >> 16);
                val3 = (byte)(num >> 8);
                val4 = (byte)num;
            }
            else if (num <= 0x1FFFFFFFFF)
            {
                c = EIGHT_BYTE_C_Values.FIVE_BYTE_VAL;
                val1 = (byte)(num >> 32);
                val2 = (byte)(num >> 24);
                val3 = (byte)(num >> 16);
                val4 = (byte)(num >> 8);
                val5 = (byte)num;
            }
            else if (num <= 0x1FFFFFFFFFFF)
            {
                c = EIGHT_BYTE_C_Values.SIX_BYTE_VAL;
                val1 = (byte)(num >> 40);
                val2 = (byte)(num >> 32);
                val3 = (byte)(num >> 24);
                val4 = (byte)(num >> 16);
                val5 = (byte)(num >> 8);
                val6 = (byte)num;
            }
            else if (num <= 0x1FFFFFFFFFFFFF)
            {
                c = EIGHT_BYTE_C_Values.SEVEN_BYTE_VAL;
                val1 = (byte)(num >> 48);
                val2 = (byte)(num >> 40);
                val3 = (byte)(num >> 32);
                val4 = (byte)(num >> 24);
                val5 = (byte)(num >> 16);
                val6 = (byte)(num >> 8);
                val7 = (byte)num;
            }
            else
            {
                c = EIGHT_BYTE_C_Values.EIGHT_BYTE_VAL;
                val1 = (byte)(num >> 56);
                val2 = (byte)(num >> 48);
                val3 = (byte)(num >> 40);
                val4 = (byte)(num >> 32);
                val5 = (byte)(num >> 24);
                val6 = (byte)(num >> 16);
                val7 = (byte)(num >> 8);
                val8 = (byte)num;
            }
        }

        public ulong ToULong()
        {
            if (c == EIGHT_BYTE_C_Values.ONE_BYTE_VAL)
            {
                return val1;
            }
            else if (c == EIGHT_BYTE_C_Values.TWO_BYTE_VAL)
            {
                return ((ulong)val1 << 8) | (ulong)val2;
            }
            else if (c == EIGHT_BYTE_C_Values.THREE_BYTE_VAL)
            {
                return ((ulong)val1 << 16) | ((ulong)val2 << 8) | (ulong)val3;
            }
            else if (c == EIGHT_BYTE_C_Values.FOUR_BYTE_VAL)
            {
                return ((ulong)val1 << 24) | ((ulong)val2 << 16) | ((ulong)val3 << 8) | (ulong)val4;
            }
            else if (c == EIGHT_BYTE_C_Values.FIVE_BYTE_VAL)
            {
                return ((ulong)val1 << 32) | ((ulong)val2 << 24) | ((ulong)val3 << 16) | ((ulong)val4 << 8) | (ulong)val5;
            }
            else if (c == EIGHT_BYTE_C_Values.SIX_BYTE_VAL)
            {
                return ((ulong)val1 << 40) | ((ulong)val2 << 32) | ((ulong)val3 << 24) | ((ulong)val4 << 16) | ((ulong)val5 << 8) | (ulong)val6;
            }
            else if (c == EIGHT_BYTE_C_Values.SEVEN_BYTE_VAL)
            {
                return ((ulong)val1 << 48) | ((ulong)val2 << 40) | ((ulong)val3 << 32) | ((ulong)val4 << 24) | ((ulong)val5 << 16) | ((ulong)val6 << 8) | (ulong)val7;
            }
            else
            {
                return ((ulong)val1 << 56) | ((ulong)val2 << 48) | ((ulong)val3 << 40) | ((ulong)val4 << 32) | ((ulong)val5 << 24) | ((ulong)val6 << 16) | ((ulong)val7 << 8) | (ulong)val8;
            }
        }

        public uint Length()
        {
            switch (c)
            {
                case EIGHT_BYTE_C_Values.ONE_BYTE_VAL:
                    return 1;
                case EIGHT_BYTE_C_Values.TWO_BYTE_VAL:
                    return 2;
                case EIGHT_BYTE_C_Values.THREE_BYTE_VAL:
                    return 3;
                case EIGHT_BYTE_C_Values.FOUR_BYTE_VAL:
                    return 4;
                case EIGHT_BYTE_C_Values.FIVE_BYTE_VAL:
                    return 5;
                case EIGHT_BYTE_C_Values.SIX_BYTE_VAL:
                    return 6;
                case EIGHT_BYTE_C_Values.SEVEN_BYTE_VAL:
                    return 7;
                default:
                    return 8;
            }
        }

        #region Encoding/Decoding

        public void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteByte((byte)((int)c << 5 | val1));
            if (c >= EIGHT_BYTE_C_Values.TWO_BYTE_VAL)
            {
                marshaler.WriteByte((byte)val2);
            }
            if (c >= EIGHT_BYTE_C_Values.THREE_BYTE_VAL)
            {
                marshaler.WriteByte((byte)val3);
            }
            if (c >= EIGHT_BYTE_C_Values.FOUR_BYTE_VAL)
            {
                marshaler.WriteByte((byte)val4);
            }
            if (c >= EIGHT_BYTE_C_Values.FIVE_BYTE_VAL)
            {
                marshaler.WriteByte((byte)val5);
            }
            if (c >= EIGHT_BYTE_C_Values.SIX_BYTE_VAL)
            {
                marshaler.WriteByte((byte)val6);
            }
            if (c >= EIGHT_BYTE_C_Values.SEVEN_BYTE_VAL)
            {
                marshaler.WriteByte((byte)val7);
            }
            if (c == EIGHT_BYTE_C_Values.EIGHT_BYTE_VAL)
            {
                marshaler.WriteByte((byte)val8);
            }
        }

        public bool Decode(PduMarshaler marshaler)
        {
            try
            {
                byte t = marshaler.ReadByte();
                c = (EIGHT_BYTE_C_Values)(t >> 5);
                val1 = (byte)(t & 0x1F);
                if (c >= EIGHT_BYTE_C_Values.TWO_BYTE_VAL)
                {
                    val2 = marshaler.ReadByte();
                }
                if (c >= EIGHT_BYTE_C_Values.THREE_BYTE_VAL)
                {
                    val3 = marshaler.ReadByte();
                }
                if (c >= EIGHT_BYTE_C_Values.FOUR_BYTE_VAL)
                {
                    val4 = marshaler.ReadByte();
                }
                if (c >= EIGHT_BYTE_C_Values.FIVE_BYTE_VAL)
                {
                    val5 = marshaler.ReadByte();
                }
                if (c >= EIGHT_BYTE_C_Values.SIX_BYTE_VAL)
                {
                    val6 = marshaler.ReadByte();
                }
                if (c >= EIGHT_BYTE_C_Values.SEVEN_BYTE_VAL)
                {
                    val7 = marshaler.ReadByte();
                }
                if (c == EIGHT_BYTE_C_Values.EIGHT_BYTE_VAL)
                {
                    val8 = marshaler.ReadByte();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }

    /// <summary>
    /// Containing an encoded representation of the number of bytes in this structure EIGHT_BYTE_UNSIGNED_INTEGER.
    /// </summary>
    public enum EIGHT_BYTE_C_Values : byte
    {
        /// <summary>
        /// Implies that the optional val2, val3, val4, val5, val6, val7 and val8 fields are not present. Hence, the structure is 1 byte in size.
        /// </summary>
        ONE_BYTE_VAL = 0,

        /// <summary>
        /// Implies that the optional val2 field is present, while the optional val3, val4, val5, val6, val7 and val8 fields are not present. Hence, the structure is 2 bytes in size.
        /// </summary>
        TWO_BYTE_VAL = 1,

        /// <summary>
        /// Implies that the optional val2 and val3 fields are present, while the optional val4, val5, val6, val7 and val8 fields are not present. Hence, the structure is 3 bytes in size.
        /// </summary>
        THREE_BYTE_VAL = 2,

        /// <summary>
        /// Implies that the optional val2, val3, and val4 fields are all present, while the optional val5, val6, val7 and val8 fields are not present. Hence, the structure is 4 bytes in size.
        /// </summary>
        FOUR_BYTE_VAL = 3,

        /// <summary>
        /// Implies that the optional val2, val3, val4 and val5 fields are all present, while the optional val6, val7 and val8 fields are not present. Hence, the structure is 5 bytes in size.
        /// </summary>
        FIVE_BYTE_VAL = 4,

        /// <summary>
        /// Implies that the optional val2, val3, val4, val5 and val6 fields are all present, while the optional val7 and val8 fields are not present. Hence, the structure is 6 bytes in size.
        /// </summary>
        SIX_BYTE_VAL = 5,

        /// <summary>
        /// Implies that the optional val2, val3, val4, val5, val6 and val7 fields are all present, while the optional val8 field is not present. Hence, the structure is 7 bytes in size.
        /// </summary>
        SEVEN_BYTE_VAL = 6,

        /// <summary>
        /// Implies that the optional val2, val3, val4, val5, val6, val7 and val8 fields are all present. Hence, the structure is 8 bytes in size.
        /// </summary>
        EIGHT_BYTE_VAL = 7
    }


    /// <summary>
    /// The RDPINPUT_HEADER structure is included in all input event PDUs and is used to identify the input event type and to specify the length of the PDU.
    /// </summary>
    public struct RDPINPUT_HEADER
    {
        /// <summary>
        /// A 16-bit unsigned integer that identifies the type of the input event PDU.
        /// </summary>
        public EventId_Values eventId;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the length of the input event PDU in bytes. This value MUST include the length of the RDPINPUT_HEADER (6 bytes).
        /// </summary>
        public uint pduLength;
    }

    /// <summary>
    /// A 16-bit unsigned integer that identifies the type of the input event PDU.
    /// </summary>
    public enum EventId_Values : ushort
    {
        /// <summary>
        /// RDPINPUT_SC_READY_PDU (section 2.2.3.1)
        /// </summary>
        EVENTID_SC_READY = 0x0001,

        /// <summary>
        /// RDPINPUT_CS_READY_PDU (section 2.2.3.2)
        /// </summary>
        EVENTID_CS_READY = 0x0002,

        /// <summary>
        /// RDPINPUT_TOUCH_EVENT_PDU (section 2.2.3.3)
        /// </summary>
        EVENTID_TOUCH = 0x0003,

        /// <summary>
        /// RDPINPUT_SUSPEND_TOUCH_PDU (section 2.2.3.4)
        /// </summary>
        EVENTID_SUSPEND_TOUCH = 0x0004,

        /// <summary>
        /// RDPINPUT_RESUME_TOUCH_PDU (section 2.2.3.5)
        /// </summary>
        EVENTID_RESUME_TOUCH = 0x0005,

        /// <summary>
        /// RDPINPUT_DISMISS_HOVERING_CONTACT_PDU (section 2.2.3.6)
        /// </summary>
        EVENTID_DISMISS_HOVERING_CONTACT = 0x0006,

        /// <summary>
        /// RDPINPUT_PEN_EVENT_PDU (section 2.2.3.7)
        /// </summary>
        EVENTID_PEN = 0x0008,
    }

    #endregion

    #region Input Messages

    /// <summary>
    /// The base pdu of all RDPEI messages.
    /// </summary>
    public class RDPINPUT_PDU : BasePDU
    {
        public RDPINPUT_HEADER header;

        #region Encoding/Decoding

        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt16((ushort)header.eventId);
            marshaler.WriteUInt32(header.pduLength);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                header.eventId = (EventId_Values)marshaler.ReadUInt16();
                header.pduLength = marshaler.ReadUInt32();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// The RDPINPUT_SC_READY_PDU message is sent by the server endpoint and is used to indicate readiness to commence with touch remoting transactions.
    /// </summary>
    public class RDPINPUT_SC_READY_PDU : RDPINPUT_PDU
    {
        /// <summary>
        /// A 32-bit unsigned integer that specifies the input protocol version.
        /// </summary>
        public RDPINPUT_SC_READY_ProtocolVersion protocolVersion;

        public uint Length()
        {
            return 10;
        }

        #region Encoding/Decoding

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32((uint)protocolVersion);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                if (!base.Decode(marshaler))
                {
                    return false;
                }
                protocolVersion = (RDPINPUT_SC_READY_ProtocolVersion)marshaler.ReadUInt32();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// A 32-bit unsigned integer that specifies the input protocol version.
    /// </summary>
    public enum RDPINPUT_SC_READY_ProtocolVersion : uint
    {
        /// <summary>
        /// Version 1.0.0 of the RDP input remoting protocol.
        /// </summary>
        RDPINPUT_PROTOCOL_V100 = 0x00010000,

        /// <summary>
        /// Version 1.0.1 of the RDP input remoting protocol.
        /// </summary>
        RDPINPUT_PROTOCOL_V101 = 0x00010001,

        /// <summary>
        /// Version 2.0.0 of the RDP input remoting protocol. 
        /// This version supports the remoting of both multitouch and pen frames.
        /// </summary>
        RDPINPUT_PROTOCOL_V200 = 0x00020000

    }

    /// <summary>
    /// The RDPINPUT_CS_READY_PDU message is sent by the client endpoint and is used to indicate readiness to commence with touch remoting transactions.
    /// </summary>
    public class RDPINPUT_CS_READY_PDU : RDPINPUT_PDU
    {
        /// <summary>
        /// A 32-bit unsigned integer that specifies touch initialization flags.
        /// </summary>
        public RDPINPUT_CS_READY_Flag flags;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the input protocol version.
        /// </summary>
        public RDPINPUT_CS_READY_ProtocolVersion protocolVersion;

        /// <summary>
        /// A 16-bit unsigned integer that specifies the maximum number of simultaneous touch contacts supported by the client.
        /// </summary>
        public ushort maxTouchContacts;

        public uint Length()
        {
            return 16;
        }

        #region Encoding/Decoding

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32((uint)flags);
            marshaler.WriteUInt32((uint)protocolVersion);
            marshaler.WriteUInt16(maxTouchContacts);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                if (!base.Decode(marshaler))
                {
                    return false;
                }
                flags = (RDPINPUT_CS_READY_Flag)marshaler.ReadUInt32();
                protocolVersion = (RDPINPUT_CS_READY_ProtocolVersion)marshaler.ReadUInt32();
                maxTouchContacts = marshaler.ReadUInt16();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// A 32-bit unsigned integer that specifies touch initialization flags.
    /// </summary>
    [Flags]
    public enum RDPINPUT_CS_READY_Flag : uint
    {
        /// <summary>
        /// A 16-bit unsigned integer that specifies the maximum number of simultaneous touch contacts supported by the client.
        /// </summary>
        READY_FLAGS_SHOW_TOUCH_VISUALS = 0x00000001,

        /// <summary>
        /// The client does not support touch frame timestamp remoting. 
        /// The server MUST ignore any values specified in the frameOffset field of the RDPINPUT_TOUCH_FRAME (section 2.2.3.3.1) structure 
        /// and the encodeTime field of the RDPINPUT_TOUCH_EVENT_PDU (section 2.2.3.3) message.
        /// </summary>
        READY_FLAGS_DISABLE_TIMESTAMP_INJECTION = 0x00000002
    }

    /// <summary>
    /// A 32-bit unsigned integer that specifies the input protocol version.
    /// </summary>
    public enum RDPINPUT_CS_READY_ProtocolVersion : uint
    {
        /// <summary>
        /// Version 1.0.0 of the RDP input remoting protocol.
        /// </summary>
        RDPINPUT_PROTOCOL_V100 = 0x00010000,

        /// <summary>
        /// Version 1.0.1 of the RDP input remoting protocol.
        /// </summary>
        RDPINPUT_PROTOCOL_V101 = 0x00010001,

        /// <summary>
        /// Version 2.0.0 of the RDP input remoting protocol. 
        /// This version supports the remoting of both multitouch and pen frames.
        /// </summary>
        RDPINPUT_PROTOCOL_V200 = 0x00020000
    }

    /// <summary>
    /// The RDPINPUT_TOUCH_EVENT_PDU message is sent by the client endpoint and is used to remote a collection of touch frames.
    /// </summary>
    public class RDPINPUT_TOUCH_EVENT_PDU : RDPINPUT_PDU
    {
        /// <summary>
        /// A FOUR_BYTE_UNSIGNED_INTEGER (section 2.2.2.3) structure that specifies the time that has elapsed (in milliseconds) from when the oldest touch frame was generated to when it was encoded for transmission by the client.
        /// </summary>
        public FOUR_BYTE_UNSIGNED_INTEGER encodeTime;

        /// <summary>
        /// A TWO_BYTE_UNSIGNED_INTEGER (section 2.2.2.1) structure that specifies the number of RDPINPUT_TOUCH_FRAME (section 2.2.3.3.1) structures in the frames field.
        /// </summary>
        public TWO_BYTE_UNSIGNED_INTEGER frameCount;

        /// <summary>
        /// An array of RDPINPUT_TOUCH_FRAME structures ordered from the oldest in time to the most recent in time. The number of structures in this array is specified by the frameCount field.
        /// </summary>
        public RDPINPUT_TOUCH_FRAME[] frames;

        public uint Length()
        {
            uint length = 6;
            length += encodeTime.Length();
            length += frameCount.Length();
            foreach (RDPINPUT_TOUCH_FRAME f in frames)
            {
                length += f.contactCount.Length();
                length += f.frameOffset.Length();
                foreach (RDPINPUT_CONTACT_DATA d in f.contacts)
                {
                    length += 1;
                    length += d.fieldsPresent.Length();
                    length += d.x.Length();
                    length += d.y.Length();
                    length += d.contactFlags.Length();
                    if (((RDPINPUT_CONTACT_DATA_FieldsPresent)(d.fieldsPresent.ToUShort())).HasFlag(RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_CONTACTRECT_PRESENT))
                    {
                        length += d.contactRectLeft.Length();
                        length += d.contactRectTop.Length();
                        length += d.contactRectRight.Length();
                        length += d.contactRectBottom.Length();
                    }
                    if (((RDPINPUT_CONTACT_DATA_FieldsPresent)(d.fieldsPresent.ToUShort())).HasFlag(RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_ORIENTATION_PRESENT))
                    {
                        length += d.orientation.Length();
                    }
                    if (((RDPINPUT_CONTACT_DATA_FieldsPresent)(d.fieldsPresent.ToUShort())).HasFlag(RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_PRESSURE_PRESENT))
                    {
                        length += d.pressure.Length();
                    }
                }
            }
            return length;
        }

        #region Encoding/Decoding

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            encodeTime.Encode(marshaler);
            frameCount.Encode(marshaler);
            foreach (RDPINPUT_TOUCH_FRAME f in frames)
            {
                f.contactCount.Encode(marshaler);
                f.frameOffset.Encode(marshaler);
                foreach (RDPINPUT_CONTACT_DATA d in f.contacts)
                {
                    marshaler.WriteByte(d.contactId);
                    d.fieldsPresent.Encode(marshaler);
                    d.x.Encode(marshaler);
                    d.y.Encode(marshaler);
                    d.contactFlags.Encode(marshaler);
                    ushort fieldPresent = d.fieldsPresent.ToUShort();
                    if ((ushort)(fieldPresent & (ushort)(RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_CONTACTRECT_PRESENT)) == (ushort)RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_CONTACTRECT_PRESENT)
                    {
                        d.contactRectLeft.Encode(marshaler);
                        d.contactRectTop.Encode(marshaler);
                        d.contactRectRight.Encode(marshaler);
                        d.contactRectBottom.Encode(marshaler);
                    }
                    if ((ushort)(fieldPresent & (ushort)(RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_ORIENTATION_PRESENT)) == (ushort)RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_ORIENTATION_PRESENT)
                    {
                        d.orientation.Encode(marshaler);
                    }
                    if ((ushort)(fieldPresent & (ushort)(RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_PRESSURE_PRESENT)) == (ushort)RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_PRESSURE_PRESENT)
                    {
                        d.pressure.Encode(marshaler);
                    }
                }
            }
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                if (!base.Decode(marshaler))
                {
                    return false;
                }
                encodeTime = new FOUR_BYTE_UNSIGNED_INTEGER();
                if (!encodeTime.Decode(marshaler))
                {
                    return false;
                }
                frameCount = new TWO_BYTE_UNSIGNED_INTEGER();
                if (!frameCount.Decode(marshaler))
                {
                    return false;
                }
                ushort count = frameCount.ToUShort();
                if (count > 0)
                {
                    frames = new RDPINPUT_TOUCH_FRAME[count];
                }
                for (int i = 0; i < count; i++)
                {
                    frames[i] = new RDPINPUT_TOUCH_FRAME();
                    if (!frames[i].contactCount.Decode(marshaler) || !frames[i].frameOffset.Decode(marshaler))
                    {
                        return false;
                    }
                    ushort contactCount = frames[i].contactCount.ToUShort();
                    if (contactCount > 0)
                    {
                        frames[i].contacts = new RDPINPUT_CONTACT_DATA[contactCount];
                    }
                    for (int j = 0; j < contactCount; j++)
                    {
                        frames[i].contacts[j] = new RDPINPUT_CONTACT_DATA();
                        frames[i].contacts[j].contactId = marshaler.ReadByte();
                        if (!frames[i].contacts[j].fieldsPresent.Decode(marshaler) || !frames[i].contacts[j].x.Decode(marshaler)
                            || !frames[i].contacts[j].y.Decode(marshaler) || !frames[i].contacts[j].contactFlags.Decode(marshaler))
                        {
                            return false;
                        }

                        ushort fieldsPresent = frames[i].contacts[j].fieldsPresent.ToUShort();
                        if ((ushort)(fieldsPresent & (ushort)(RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_CONTACTRECT_PRESENT)) == (ushort)RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_CONTACTRECT_PRESENT)
                        {
                            TWO_BYTE_SIGNED_INTEGER left = new TWO_BYTE_SIGNED_INTEGER();
                            if (!left.Decode(marshaler))
                            {
                                return false;
                            }
                            frames[i].contacts[j].contactRectLeft = left;
                            TWO_BYTE_SIGNED_INTEGER top = new TWO_BYTE_SIGNED_INTEGER();
                            if (!top.Decode(marshaler))
                            {
                                return false;
                            }
                            frames[i].contacts[j].contactRectTop = top;
                            TWO_BYTE_SIGNED_INTEGER right = new TWO_BYTE_SIGNED_INTEGER();
                            if (!right.Decode(marshaler))
                            {
                                return false;
                            }
                            frames[i].contacts[j].contactRectRight = right;
                            TWO_BYTE_SIGNED_INTEGER bottom = new TWO_BYTE_SIGNED_INTEGER();
                            if (!bottom.Decode(marshaler))
                            {
                                return false;
                            }
                            frames[i].contacts[j].contactRectBottom = bottom;
                        }
                        if ((ushort)(fieldsPresent & (ushort)(RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_ORIENTATION_PRESENT)) == (ushort)RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_ORIENTATION_PRESENT)
                        {
                            FOUR_BYTE_UNSIGNED_INTEGER orientation = new FOUR_BYTE_UNSIGNED_INTEGER();
                            if (!orientation.Decode(marshaler))
                            {
                                return false;
                            }
                            frames[i].contacts[j].orientation = orientation;
                        }
                        if ((ushort)(fieldsPresent & (ushort)(RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_PRESSURE_PRESENT)) == (ushort)RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_PRESSURE_PRESENT)
                        {
                            FOUR_BYTE_UNSIGNED_INTEGER pressure = new FOUR_BYTE_UNSIGNED_INTEGER();
                            if (!pressure.Decode(marshaler))
                            {
                                return false;
                            }
                            frames[i].contacts[j].pressure = pressure;
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// The RDPINPUT_TOUCH_FRAME structure encapsulates a collection of RDPINPUT_CONTACT_DATA (section 2.2.3.3.1.1) structures that are part of the same logical touch frame.
    /// </summary>
    public class RDPINPUT_TOUCH_FRAME
    {
        /// <summary>
        /// A TWO_BYTE_UNSIGNED_INTEGER (section 2.2.2.1) structure that specifies the number of RDPINPUT_CONTACT_DATA structures in the contacts field.
        /// </summary>
        public TWO_BYTE_UNSIGNED_INTEGER contactCount;

        /// <summary>
        /// An EIGHT_BYTE_UNSIGNED_INTEGER (section 2.2.2.5) structure that specifies the time offset from the previous frame (in microseconds). 
        /// </summary>
        public EIGHT_BYTE_UNSIGNED_INTEGER frameOffset;

        /// <summary>
        /// An array of RDPINPUT_CONTACT_DATA structures. The number of structures in this array is specified by the contactCount field.
        /// </summary>
        public RDPINPUT_CONTACT_DATA[] contacts;

        public RDPINPUT_TOUCH_FRAME()
        {
            contactCount = new TWO_BYTE_UNSIGNED_INTEGER();
            frameOffset = new EIGHT_BYTE_UNSIGNED_INTEGER();
        }
    }

    /// <summary>
    /// The RDPINPUT_CONTACT_DATA structure describes the characteristics of a contact that is encapsulated in an RDPINPUT_TOUCH_FRAME (section 2.2.3.3.1) structure.
    /// </summary>
    public class RDPINPUT_CONTACT_DATA
    {
        /// <summary>
        /// An 8-bit unsigned integer that specifies the ID assigned to the contact. This value MUST be in the range 0x00 to 0xFF (inclusive).
        /// </summary>
        public byte contactId;

        /// <summary>
        /// A TWO_BYTE_UNSIGNED_INTEGER (section 2.2.2.1) structure that specifies the presence of the optional contactRectLeft, contactRectTop, contactRectRight, contactRectBottom, orientation, and pressure fields.
        /// </summary>
        public TWO_BYTE_UNSIGNED_INTEGER fieldsPresent;

        /// <summary>
        /// A FOUR_BYTE_SIGNED_INTEGER (section 2.2.2.4) structure that specifies the X-coordinate (relative to the virtual-desktop origin) of the contact.
        /// </summary>
        public FOUR_BYTE_SIGNED_INTEGER x;

        /// <summary>
        /// A FOUR_BYTE_SIGNED_INTEGER structure that specifies the Y-coordinate (relative to the virtual-desktop origin) of the contact.
        /// </summary>
        public FOUR_BYTE_SIGNED_INTEGER y;

        /// <summary>
        /// A FOUR_BYTE_UNSIGNED_INTEGER (section 2.2.2.3) structure that specifies the current state of the contact. 
        /// </summary>
        public FOUR_BYTE_UNSIGNED_INTEGER contactFlags;

        /// <summary>
        /// An optional TWO_BYTE_SIGNED_INTEGER (section 2.2.2.2) structure that specifies the leftmost bound (relative to the contact point specified by the x and y fields) of the exclusive rectangle describing the geometry of the contact. 
        /// </summary>
        public TWO_BYTE_SIGNED_INTEGER contactRectLeft;

        /// <summary>
        /// An optional TWO_BYTE_SIGNED_INTEGER structure that specifies the upper bound (relative to the contact point specified by the x and y fields) of the exclusive rectangle describing the geometry of the contact.
        /// </summary>
        public TWO_BYTE_SIGNED_INTEGER contactRectTop;

        /// <summary>
        /// An optional TWO_BYTE_SIGNED_INTEGER structure that specifies the rightmost bound (relative to the contact point specified by the x and y fields) of the exclusive rectangle describing the geometry of the contact. 
        /// </summary>
        public TWO_BYTE_SIGNED_INTEGER contactRectRight;

        /// <summary>
        /// An optional TWO_BYTE_SIGNED_INTEGER structure that specifies the lower bound (relative to the contact point specified by the x and y fields) of the exclusive rectangle describing the geometry of the contact. 
        /// </summary>
        public TWO_BYTE_SIGNED_INTEGER contactRectBottom;

        /// <summary>
        /// An optional FOUR_BYTE_UNSIGNED_INTEGER structure that specifies the angle through which the contact rectangle (specified in the contactRectLeft, contactRectTop, contactRectRight and contactRectBottom fields) MUST be rotated to yield the actual contact geometry. 
        /// </summary>
        public FOUR_BYTE_UNSIGNED_INTEGER orientation;

        /// <summary>
        /// An optional FOUR_BYTE_UNSIGNED_INTEGER structure that specifies the contact pressure.
        /// </summary>
        public FOUR_BYTE_UNSIGNED_INTEGER pressure;

        public RDPINPUT_CONTACT_DATA()
        {
            fieldsPresent = new TWO_BYTE_UNSIGNED_INTEGER();
            x = new FOUR_BYTE_SIGNED_INTEGER();
            y = new FOUR_BYTE_SIGNED_INTEGER();
            contactFlags = new FOUR_BYTE_UNSIGNED_INTEGER();
            contactRectLeft = null;
            contactRectTop = null;
            contactRectRight = null;
            contactRectBottom = null;
            orientation = null;
            pressure = null;
        }
    }

    /// <summary>
    /// A TWO_BYTE_UNSIGNED_INTEGER (section 2.2.2.1) structure that specifies the presence of the optional contactRectLeft, contactRectTop, contactRectRight, contactRectBottom, orientation, and pressure fields.
    /// </summary>
    [Flags]
    public enum RDPINPUT_CONTACT_DATA_FieldsPresent : ushort
    {
        /// <summary>
        /// The optional contactRectLeft, contactRectTop, contactRectRight, and contactRectBottom fields are all present.
        /// </summary>
        CONTACT_DATA_CONTACTRECT_PRESENT = 0x0001,

        /// <summary>
        /// The optional orientation field is present.
        /// </summary>
        CONTACT_DATA_ORIENTATION_PRESENT = 0x0002,

        /// <summary>
        /// The optional pressure field is present.
        /// </summary>
        CONTACT_DATA_PRESSURE_PRESENT = 0x0004
    }

    /// <summary>
    /// A FOUR_BYTE_UNSIGNED_INTEGER (section 2.2.2.3) structure that specifies the current state of the contact.
    /// </summary>
    [Flags]
    public enum RDPINPUT_CONTACT_DATA_ContactFlags : uint
    {
        /// <summary>
        /// The contact transitioned to the engaged state (made contact).
        /// </summary>
        CONTACT_FLAG_DOWN = 0x0001,

        /// <summary>
        /// Contact update.
        /// </summary>
        CONTACT_FLAG_UPDATE = 0x0002,

        /// <summary>
        /// The contact transitioned from the engaged state (broke contact).
        /// </summary>
        CONTACT_FLAG_UP = 0X0004,

        /// <summary>
        /// The contact has not departed and is still in range.	
        /// </summary>
        CONTACT_FLAG_INRANGE = 0x0008,

        /// <summary>
        /// The contact is in the engaged state.
        /// </summary>
        CONTACT_FLAG_INCONTACT = 0x0010,

        /// <summary>
        /// The contact has been canceled.
        /// </summary>
        CONTACT_FLAG_CANCELED = 0x0020
    }

    /// <summary>
    /// The RDPINPUT_SUSPEND_TOUCH_PDU message is sent by the server endpoint and is used to instruct the client 
    /// to suspend the transmission of the RDPINPUT_TOUCH_EVENT_PDU (section 2.2.3.2) message.
    /// </summary>
    public class RDPINPUT_SUSPEND_TOUCH_PDU : RDPINPUT_PDU
    {
        // The same as the RDPINPUT_HEADER
        public uint Length()
        {
            return 6;
        }
    }

    /// <summary>
    /// The RDPINPUT_RESUME_TOUCH_PDU message is sent by the server endpoint and 
    /// is used to instruct the client to resume the transmission of the RDPINPUT_TOUCH_EVENT_PDU (section 2.2.3.2).
    /// </summary>
    public class RDPINPUT_RESUME_TOUCH_PDU : RDPINPUT_PDU
    {
        // The same as the RDPINPUT_HEADER
        public uint Length()
        {
            return 6;
        }
    }

    /// <summary>
    /// The RDPINPUT_DISMISS_HOVERING_CONTACT_PDU message is sent by the client endpoint 
    /// to instruct the server to transition a contact in the "hovering" state to the "out of range" state (section 3.1.1.1).
    /// </summary>
    public class RDPINPUT_DISMISS_HOVERING_CONTACT_PDU : RDPINPUT_PDU
    {
        /// <summary>
        /// An 8-bit unsigned integer that specifies the ID assigned to the contact. This value MUST be in the range 0x00 to 0xFF (inclusive).
        /// </summary>
        public byte contactId;

        public uint Length()
        {
            return 7;
        }

        #region Encoding/Decoding

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteByte(contactId);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            base.Decode(marshaler);
            contactId = marshaler.ReadByte();
            return true;
        }

        #endregion
    }

    #endregion
}
