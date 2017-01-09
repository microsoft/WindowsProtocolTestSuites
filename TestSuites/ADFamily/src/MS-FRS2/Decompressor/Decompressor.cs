// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Decompressor
{
    /// <summary>
    /// Decompressor Class
    /// </summary>
    public class Decompressor
    {
        /// <summary>
        /// Initializes BitString
        /// </summary>
        /// <param name="bstr">
        /// BITSTRING to initialize.
        /// </param>
        /// <param name="source">
        /// Source array
        /// </param>
        /// <param name="index">
        /// Index value
        /// </param>
        public void BitStringInit(ref BITSTRING bstr, byte[] source, UInt32 index)
        {
            bstr.mask = BitConverter.ToUInt16(source, (int)index);
            bstr.mask = bstr.mask << 16;
            index = index + 2;
            bstr.mask = bstr.mask + BitConverter.ToUInt16(source, (int)index);
            index = index + 2;
            bstr.bits = 32;
            bstr.source = source;
            bstr.index = index;
        }

        /// <summary>
        /// BitStringLookup function.
        /// </summary>
        /// <param name="bstr">
        /// BITSTRING to look up.
        /// </param>
        /// <param name="n"></param>
        /// <returns></returns>
        public UInt32 BitStringLookup(ref BITSTRING bstr, UInt32 n)
        {
            if (n == 0)
            {
                return 0;
            }
            else
            {
                return (bstr.mask >> (int)(32 - n));
            }
        }

        /// <summary>
        /// BitStringSkip function
        /// </summary>
        /// <param name="bstr"></param>
        /// <param name="n"></param>
        public void BitStringSkip(ref BITSTRING bstr, UInt32 n)
        {
            bstr.mask = bstr.mask << (int)n;
            bstr.bits = bstr.bits - (Int32)n;

            if (bstr.bits < 16)
            {
                bstr.mask = bstr.mask + (UInt32)(BitConverter.ToUInt16(bstr.source, (int)bstr.index)
                                                 << (int)(16 - bstr.bits));
                bstr.index = bstr.index + 2;
                bstr.bits = bstr.bits + 16;
            }
        }

        /// <summary>
        /// PrefixCodeTreeDecodeSymbol method decodes the symbols.
        /// </summary>
        /// <param name="bstr"></param>
        /// <param name="root">
        /// Used to traverse the tree.
        /// </param>
        /// <returns></returns>
        public UInt32 PrefixCodeTreeDecodeSymbol(ref BITSTRING bstr, PrefixCodeNode root)
        {
            UInt32 bit;
            PrefixCodeNode node = root;
            do
            {
                bit = BitStringLookup(ref bstr, 1);
                BitStringSkip(ref bstr, 1);
                node = node.Child[bit];
            } while (node.leaf == false);

            return node.symbol;
        }

        /// <summary>
        /// Compares two symbols
        /// </summary>
        /// <param name="a">
        /// First Symbol
        /// </param>
        /// <param name="b">
        /// Second Symbol
        /// </param>
        /// <returns></returns>
        public int CompareSymbols(PrefixCodeSymbol a, PrefixCodeSymbol b)
        {
            if (a.length < b.length)
                return -1;
            else if (a.length > b.length)
                return 1;
            else if (a.symbol < b.symbol)
                return -1;
            else if (a.symbol > b.symbol)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Sorts Symbols
        /// </summary>
        /// <param name="symbolArray">
        /// Array of symbols to be sorted.
        /// </param>
        public void SortSymbols(ref PrefixCodeSymbol[] symbolArray)
        {
            PrefixCodeSymbol temp;

            for (int i = 0; i < 511; i++)
            {
                for (int j = 0; j < 511 - i; j++)
                {
                    if (CompareSymbols(symbolArray[j + 1], symbolArray[j]) < 0)
                    {
                        temp = symbolArray[j];
                        symbolArray[j] = symbolArray[j + 1];
                        symbolArray[j + 1] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// Adds leaf to the tree.
        /// </summary>
        /// <param name="treeNodes">
        /// PrefixCodeNode node
        /// </param>
        /// <param name="leafIndex">
        /// Index of leaf.
        /// </param>
        /// <param name="mask"></param>
        /// <param name="bits"></param>
        /// <returns></returns>
        public UInt32 PrefixCodeTreeAddLeaf(ref PrefixCodeNode[] treeNodes, UInt32 leafIndex, UInt32 mask, UInt32 bits)
        {
            PrefixCodeNode node = treeNodes[0];
            UInt32 i = leafIndex + 1;
            UInt32 childIndex;

            while (bits > 1)
            {
                bits--;
                childIndex = (mask >> (int)bits) & 1;

                if (node.Child[childIndex] == null)
                {
                    node.Child[childIndex] = treeNodes[i];
                    treeNodes[i].leaf = false;
                    i++;
                }
                node = node.Child[childIndex];
            }
            node.Child[mask & 1] = treeNodes[leafIndex];

            return i;
        }

        /// <summary>
        /// Builds Tree.
        /// </summary>
        /// <param name="input">
        /// input byte array to build tree.
        /// </param>
        /// <param name="treeNodes"></param>
        /// <returns></returns>
        public PrefixCodeNode PrefixCodeTreeRebuild(byte[] input, ref PrefixCodeNode[] treeNodes)
        {
            PrefixCodeNode root = new PrefixCodeNode();
            PrefixCodeSymbol[] symbolInfo = new PrefixCodeSymbol[512];

            UInt32 i;
            UInt32 j;
            UInt32 mask;
            UInt32 bits;

            for (i = 0; i < 1024; i++)
            {
                treeNodes[i].symbol = 0;
                treeNodes[i].leaf = false;
                treeNodes[i].Child[0] = null;
                treeNodes[i].Child[1] = null;
            }

            for (i = 0; i < 256; i++)
            {
                symbolInfo[2 * i].symbol = (UInt16)(2 * i);
                symbolInfo[2 * i].length = (UInt16)(input[i] & 15);

                symbolInfo[2 * i + 1].symbol = (UInt16)(2 * i + 1);
                symbolInfo[2 * i + 1].length = (UInt16)(input[i] >> 4);
            }

            SortSymbols(ref symbolInfo);

            i = 0;
            while ((i < 512) && (symbolInfo[i].length == 0))
            {
                i++;
            }

            mask = 0;
            bits = 1;

            root = treeNodes[0];
            root.leaf = false;

            j = 1;

            for (; i < 512; i++)
            {
                treeNodes[j].symbol = symbolInfo[i].symbol;
                treeNodes[j].leaf = true;
                mask = mask << (int)(symbolInfo[i].length - bits);
                bits = symbolInfo[i].length;
                j = PrefixCodeTreeAddLeaf(ref treeNodes, j, mask, bits);
                mask++;
            }

            return root;
        }

        /// <summary>
        /// This method is called to decompress the compressed data
        /// </summary>
        /// <param name="input">
        /// Compressed Byte Array
        /// </param>
        /// <param name="outputSize">
        /// Uncompressed Output Size
        /// </param>
        /// <param name="outputBuffer">
        /// Decompressed output buffer.
        /// </param>
        public void Decompress(byte[] input, UInt32 outputSize, out byte[] outputBuffer)
        {
            byte[] output = new byte[outputSize];

            UInt32 i = 0;
            UInt32 stopIndex = i + outputSize;
            UInt32 symbol;
            UInt32 length;
            Int32 offset;

            PrefixCodeNode root = new PrefixCodeNode();

            PrefixCodeNode[] prefixCodeTreeNodes = new PrefixCodeNode[1024];
            for (int count = 0; count < 1024; count++)
            {
                prefixCodeTreeNodes[count] = new PrefixCodeNode();
            }

            BITSTRING bstr = new BITSTRING();

            root = PrefixCodeTreeRebuild(input, ref prefixCodeTreeNodes);

            BitStringInit(ref bstr, input, 256);

            while (i < stopIndex)
            {
                symbol = PrefixCodeTreeDecodeSymbol(ref bstr, root);

                if (symbol < 256)
                {
                    output[i] = (byte)symbol;
                    i++;
                }
                else
                {
                    symbol = symbol - 256;
                    length = symbol & 15;
                    symbol = symbol >> 4;

                    offset = (int)((1 << (int)symbol) + BitStringLookup(ref bstr, symbol));
                    offset = (-1) * offset;

                    if (length == 15)
                    {
                        length = bstr.source[bstr.index] + (UInt32)15;
                        bstr.index = bstr.index + 1;

                        if (length == 270)
                        {
                            length = BitConverter.ToUInt16(bstr.source, (int)bstr.index);
                            bstr.index = bstr.index + 2;
                        }
                    }

                    BitStringSkip(ref bstr, symbol);
                    length = length + 3;

                    do
                    {
                        output[i] = output[i + offset];
                        i++;
                        length--;
                    } while (length != 0);
                }
            }

            outputBuffer = output;
        }
    }
}
