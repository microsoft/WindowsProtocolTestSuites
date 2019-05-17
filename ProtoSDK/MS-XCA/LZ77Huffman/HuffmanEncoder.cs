// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress
{
    abstract class HuffmanTreeNode : IComparable<HuffmanTreeNode>
    {
        public int CompareTo(HuffmanTreeNode other)
        {
            return GetFrequency().CompareTo(other.GetFrequency());
        }

        public abstract int GetFrequency();
        public abstract void UpdateDepth(int depth);
    }

    class HuffmanTreeParentNode : HuffmanTreeNode
    {
        public HuffmanTreeParentNode(HuffmanTreeNode left, HuffmanTreeNode right)
        {
            Left = left;
            Right = right;
        }

        public HuffmanTreeNode Left { get; private set; }
        public HuffmanTreeNode Right { get; private set; }

        public override int GetFrequency()
        {
            return Left.GetFrequency() + Right.GetFrequency();
        }

        public override void UpdateDepth(int depth)
        {
            Left.UpdateDepth(depth + 1);
            Right.UpdateDepth(depth + 1);
        }
    }

    class HuffmanTreeLeafNode : HuffmanTreeNode
    {
        public HuffmanTreeLeafNode(HuffmanSymbolNode symbol)
        {
            Symbol = symbol;
        }

        public HuffmanSymbolNode Symbol { get; private set; }

        public override int GetFrequency()
        {
            return Symbol.Frequency;
        }

        public override void UpdateDepth(int depth)
        {
            Symbol.Length = depth;
        }
    }

    class HuffmanSymbolNode
    {
        public int Symbol;
        public int Frequency;
        public int Length;
        public int Code;
    }

    class HuffmanBitStreamWriter
    {
        public HuffmanBitStreamWriter(byte[] header)
        {
            FreeBits = 16;

            NextWord = 0;

            OutputPosition1 = 256;

            OutputPosition2 = 258;

            OutputPosition = 260;

            buffer = new List<byte>(header);
        }

        List<byte> buffer;
        int FreeBits;
        int NextWord;
        int OutputPosition;
        int OutputPosition1;
        int OutputPosition2;

        public void WriteBits(int NumberOfBitsToWrite, int BitsToWrite)
        {
            if (FreeBits >= NumberOfBitsToWrite)
            {
                FreeBits -= NumberOfBitsToWrite;
                NextWord = (NextWord << NumberOfBitsToWrite) + BitsToWrite;
            }
            else
            {
                NextWord <<= FreeBits;


                NextWord = NextWord + (BitsToWrite >> (NumberOfBitsToWrite - FreeBits));
                FreeBits -= NumberOfBitsToWrite;


                Write((byte)(NextWord & 0xFF), OutputPosition1);


                Write((byte)(NextWord >> 8), OutputPosition1 + 1);


                OutputPosition1 = OutputPosition2;
                OutputPosition2 = OutputPosition;


                OutputPosition += 2;

                FreeBits += 16;


                NextWord = BitsToWrite;
            }
        }

        void Write(byte b, int position)
        {
            if (position >= buffer.Count)
            {
                buffer.AddRange(new byte[position - buffer.Count + 1]);
            }
            buffer[position] = b;
        }

        public void WriteByte(byte ByteToWrite)
        {
            Write(ByteToWrite, OutputPosition);

            OutputPosition++;
        }

        public void WriteTwoBytes(ushort BytesToWrite)
        {
            Write((byte)(BytesToWrite & 0xFF), OutputPosition);

            Write((byte)(BytesToWrite >> 8), OutputPosition + 1);

            OutputPosition += 2;
        }

        public void FlushBits()
        {
            NextWord <<= FreeBits;

            Write((byte)(NextWord & 0xFF), OutputPosition1);

            Write((byte)(NextWord >> 8), OutputPosition1 + 1);

            Write(0, OutputPosition2);
            Write(0, OutputPosition2 + 1);

        }

        public byte[] GetBytes()
        {
            return buffer.ToArray();
        }
    }

    /// <summary>
    /// Huffman Encoder.
    /// </summary>
    class HuffmanEncoder
    {
        private int[] frequncy;
        private Dictionary<int, HuffmanSymbolNode> code;

        /// <summary>
        /// Generate Huffman code for LZ77 symbols.
        /// </summary>
        /// <param name="lz77Symbols">LZ77 symbols to be coded.</param>
        /// <returns>Byte array containing coded result.</returns>
        public byte[] Encode(List<LZ77Symbol> lz77Symbols)
        {
            UpdateFrequency(lz77Symbols);

            GenerateCode();

            var result = EncodeSymbols(lz77Symbols);

            return result;
        }

        private void UpdateFrequency(List<LZ77Symbol> input)
        {
            frequncy = new int[512];

            foreach (var symbol in input)
            {
                var value = symbol.Encode();
                frequncy[value]++;
            }
        }

        private void GenerateCode()
        {
            var input = new List<HuffmanSymbolNode>();
            for (int i = 0; i < 512; i++)
            {
                if (frequncy[i] > 0)
                {
                    var node = new HuffmanSymbolNode();
                    node.Symbol = i;
                    node.Frequency = frequncy[i];
                    input.Add(node);
                }
            }

            var a = input.ToArray();
            Array.Sort(a, delegate (HuffmanSymbolNode l, HuffmanSymbolNode r)
            {
                if (l.Frequency == r.Frequency)
                {
                    return l.Symbol.CompareTo(r.Symbol);
                }
                return l.Frequency.CompareTo(r.Frequency);
            });

            while (true)
            {
                var t = generateTree(a);
                t.UpdateDepth(0);
                if (input.All(x => x.Length <= 15))
                {
                    break;
                }
                for (int i = 0; i < a.Length; i++)
                {
                    a[i].Frequency = a[i].Frequency / 2 + 1;
                }
            }

            Array.Sort(a, delegate (HuffmanSymbolNode l, HuffmanSymbolNode r)
            {
                if (l.Length == r.Length)
                {
                    return l.Symbol.CompareTo(r.Symbol);
                }
                return l.Length.CompareTo(r.Length);
            });

            int ncode = 0;
            int len = 0;
            foreach (var x in a)
            {
                while (len < x.Length)
                {
                    ncode *= 2;
                    len++;
                }
                x.Code = ncode;
                ncode++;
            }

            code = new Dictionary<int, HuffmanSymbolNode>();
            foreach (var x in a)
            {
                code.Add(x.Symbol, x);
            }

        }

        private HuffmanTreeNode generateTree(HuffmanSymbolNode[] a)
        {
            var b = a.Select(x => new HuffmanTreeLeafNode(x));
            var qa = new Queue<HuffmanTreeNode>(b);
            var qb = new Queue<HuffmanTreeNode>();
            while (true)
            {
                var l = GetLeastTreeNode(qa, qb);
                var r = GetLeastTreeNode(qa, qb);

                if (r == null)
                {
                    return l;
                }

                qb.Enqueue(new HuffmanTreeParentNode(l, r));
            }
        }

        static HuffmanTreeNode GetLeastTreeNode(Queue<HuffmanTreeNode> qa, Queue<HuffmanTreeNode> qb)
        {
            var a = qa.FirstOrDefault();

            var b = qb.FirstOrDefault();

            if (a == null)
            {
                if (b != null)
                {
                    qb.Dequeue();
                }
                return b;
            }
            if (b == null)
            {
                if (a != null)
                {
                    qa.Dequeue();
                }
                return a;
            }
            if (a.CompareTo(b) > 0)
            {
                qb.Dequeue();
                return b;
            }
            else
            {
                qa.Dequeue();
                return a;
            }
        }

        private byte[] getHeader()
        {
            var result = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                result[i] = 0;
                for (int j = 0; j < 2; j++)
                {
                    int symbol = 2 * i + j;
                    int len = 0;
                    if (code.ContainsKey(symbol))
                    {
                        len = code[symbol].Length;
                    }
                    result[i] |= (byte)(len << (4 * j));
                }
            }
            return result;
        }

        private byte[] EncodeSymbols(List<LZ77Symbol> input)
        {
            var header = getHeader();

            /*
             	Write the 256-byte table of symbol bit lengths
                While there are more literals or matches to encode
                      If the next thing is a literal
                        WriteBits(SymbolLength[LiteralValue], SymbolCode[LiteralValue])
                    Else      // the next thing is a match
                        Extract the length and distance of the match
                        MatchSymbolValue = 256 + min(Length - 3, 15) + (16 * GetHighBit(Distance))
                        WriteBits(SymbolLength[MatchSymbolValue], SymbolCode[MatchSymbolValue])
                        If (Length – 3) >= 15
                            WriteByte(min(Length – 3 – 15, 255))
                            If (Length – 3 – 15) >= 255
                                WriteTwoBytes(Length – 3)
                        WriteBits(GetHighBit(Distance), Distance – (1 << GetHighBit(Distance)))
                WriteBits(SymbolLength[256], SymbolCode[256])
                FlushBits()
             */

            var stream = new HuffmanBitStreamWriter(header);

            foreach (var symbol in input)
            {
                if (symbol is LZ77Literal)
                {
                    stream.WriteBits(code[symbol.Encode()].Length, code[symbol.Encode()].Code);
                }
                else if (symbol is LZ77Match)
                {
                    stream.WriteBits(code[symbol.Encode()].Length, code[symbol.Encode()].Code);
                    var match = symbol as LZ77Match;
                    int matchLength = match.Length;
                    if (matchLength - 3 >= 15)
                    {
                        stream.WriteByte((byte)Math.Min(matchLength - 3 - 15, 255));
                        if (matchLength - 3 - 15 >= 255)
                        {
                            stream.WriteTwoBytes((ushort)(matchLength - 3));
                        }
                    }
                    int distance = match.Distance;
                    stream.WriteBits(LZ77Match.GetHighBit(distance), distance - (1 << LZ77Match.GetHighBit(distance)));
                }
                else if (symbol is LZ77EOF)
                {
                    stream.WriteBits(code[symbol.Encode()].Length, code[symbol.Encode()].Code);
                }
            }

            stream.FlushBits();

            return stream.GetBytes();
        }
    }
}
