// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress
{
    /// <summary>
    /// Common exception of LZ77.
    /// </summary>
    class LZ77Exception : Exception
    {
        public LZ77Exception(string message) : base(message)
        {

        }
    }

    public abstract class LZ77Symbol
    {
        public abstract int Encode();
        public abstract void Decode(LittleEndianByteBuffer buffer, ref int offset);
    }

    public class LZ77Literal : LZ77Symbol
    {
        public byte Literal { get; set; }

        public override void Decode(LittleEndianByteBuffer buffer, ref int offset)
        {
            buffer.WriteBytes(offset, Literal, 1);
            offset++;
        }

        public override int Encode()
        {
            return Literal;
        }
    }

    public class LZ77EOF : LZ77Symbol
    {
        public override void Decode(LittleEndianByteBuffer buffer, ref int offset)
        {

        }

        public override int Encode()
        {
            return 256;
        }
    }

    public class LZ77Match : LZ77Symbol
    {
        public int Distance { get; set; }
        public int Length { get; set; }

        public override int Encode()
        {
            int result;
            if (Length - 3 < 15)
            {
                result = 256 + (Length - 3) + (16 * GetHighBit(Distance));
            }
            else
            {
                result = 256 + 15 + (16 * GetHighBit(Distance));
            }
            return result;
        }

        static int[] PrecomputedHighBitTable;

        static LZ77Match()
        {
            PrecomputedHighBitTable = new int[256];
            for (int i = 0; i < PrecomputedHighBitTable.Length; i++)
            {
                int j = 7;
                while (j >= 0)
                {
                    if ((i >> j & 1) == 1)
                    {
                        break;
                    }
                    j--;
                }
                PrecomputedHighBitTable[i] = j;
            }
        }

        public static int GetHighBit(int x)
        {
            int result;
            if (x < 0 || x >= (1 << 16))
            {
                throw new ArgumentOutOfRangeException("Input value should be 16-bit unsigned integer!");
            }
            if (x < 256)
            {
                result = PrecomputedHighBitTable[x];
            }
            else
            {
                result = 8 + PrecomputedHighBitTable[x >> 8];
            }
            return result;
        }

        public override void Decode(LittleEndianByteBuffer buffer, ref int offset)
        {
            for (int i = 0; i < Length; i++)
            {
                int matchByte = buffer.ReadBytes(offset - Distance + i, 1);
                buffer.WriteBytes(offset + i, matchByte, 1);
            }
            offset += Length;
        }
    }

    /// <summary>
    /// LZ77 Code Implementation.
    /// </summary>
    class LZ77Code
    {
        public LZ77Code(int maxMatchDistance, int maxMatchLength)
        {
            MaximumMatchDistance = maxMatchDistance;
            MaximumMatchLength = maxMatchLength;
        }

        public int MaximumMatchDistance { get; private set; }
        public int MaximumMatchLength { get; private set; }

        class LZ77HashTable
        {
            Dictionary<int, int> store;

            public LZ77HashTable()
            {
                store = new Dictionary<int, int>();
            }

            public int Match(byte[] arg, int location)
            {
                if (location + 2 >= arg.Length)
                {
                    return location;
                }


                var tag = 0;
                for (int i = 0; i < 3; i++)
                {
                    tag |= (arg[location + i] << (8 * i));
                }

                var pos = location;
                if (store.ContainsKey(tag))
                {
                    pos = store[tag];
                }
                store[tag] = location;
                return pos;
            }
        }

        /// <summary>
        /// Encode data into LZ77 symbols.
        /// </summary>
        /// <param name="data">Byte array containing data to be encoded.</param>
        /// <returns>Array containing LZ77 symbols.</returns>
        public List<LZ77Symbol> Encode(byte[] data)
        {
            var hash = new LZ77HashTable();

            int i = 0;

            var result = new List<LZ77Symbol>();

            while (i < data.Length)
            {
                LZ77Symbol symbol;
                int location = hash.Match(data, i);
                var distance = i - location;
                if (distance > 0 && distance <= MaximumMatchDistance)
                {
                    int len = 0;
                    int k = location;
                    for (int j = i; j < data.Length; j++)
                    {
                        if (len > MaximumMatchLength)
                        {
                            break;
                        }
                        if (data[k] == data[j])
                        {
                            hash.Match(data, j);
                            k++;
                            len++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    symbol = new LZ77Match() { Length = len, Distance = distance };
                    i += len;
                }
                else
                {
                    symbol = new LZ77Literal() { Literal = data[i] };
                    i++;
                }
                result.Add(symbol);
            }

            return result;
        }

        private void Verify(List<LZ77Symbol> symbols)
        {
            for (int i = 0; i < symbols.Count; i++)
            {
                var symbol = symbols[i];

                if (symbol is LZ77EOF)
                {
                    if (i != symbols.Count - 1)
                    {
                        throw new LZ77Exception("LZ77 EOF should be the last symbol!");
                    }
                }
                if (symbol is LZ77Match)
                {
                    var match = symbol as LZ77Match;
                    if (match.Length > MaximumMatchLength)
                    {
                        throw new LZ77Exception("LZ77 match length exceeded MaximumMatchLength!");
                    }
                    if (match.Distance > MaximumMatchDistance)
                    {
                        throw new LZ77Exception("LZ77 match distance exceeded MaximumMatchDistance!");
                    }
                }
            }
        }

        /// <summary>
        /// Decode LZ77 symbols to data.
        /// </summary>
        /// <param name="symbols">LZ77 symbols to be decoded.</param>
        /// <returns>The data after decoded.</returns>
        public byte[] Decode(List<LZ77Symbol> symbols)
        {
            var buffer = new LittleEndianByteBuffer();
            int offset = 0;

            Verify(symbols);

            foreach (var symbol in symbols)
            {
                symbol.Decode(buffer, ref offset);
            }

            return buffer.GetBytes();
        }
    }
}
