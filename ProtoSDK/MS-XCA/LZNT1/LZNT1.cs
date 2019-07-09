// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress
{
    /// <summary>
    /// Common object of LZNT1.
    /// </summary>
    abstract class LZNT1_Object
    {
        public abstract void Marshal(LittleEndianByteBuffer buffer, ref int offset);

        public abstract object Unmarshal(LittleEndianByteBuffer buffer, ref int offset);

        public static T Unmarshal<T>(LittleEndianByteBuffer buffer, ref int offset) where T : LZNT1_Object, new()
        {
            var result = new T().Unmarshal(buffer, ref offset);
            return result as T;
        }
    }

    /*
        <Buffer> ::= <Chunk> <Buffer> | <Chunk>
        <Chunk> ::= <Compressed_chunk> | 
                    <Uncompressed_chunk> |
                    End_of_buffer
    
        <Uncompressed_chunk> ::= Chunk_header Uncompressed_data
        <Compressed_chunk> ::= Chunk_header <Flag_group>
        <Flag_group> ::= <Flag_data> <Flag_group> | <Flag_data>
    
        <Flag_data> ::=
            Flag_byte <Data> <Data> <Data> <Data> <Data> <Data> <Data> <Data>
          | Flag_byte <Data> <Data> <Data> <Data> <Data> <Data> <Data>
          | Flag_byte <Data> <Data> <Data> <Data> <Data> <Data>
          | Flag_byte <Data> <Data> <Data> <Data> <Data>
          | Flag_byte <Data> <Data> <Data> <Data>
          | Flag_byte <Data> <Data> <Data> 
          | Flag_byte <Data> <Data>
          | Flag_byte <Data> 
        <Data> ::= Literal | Compressed_word
     */
    class LZNT1_Buffer : LZNT1_Object
    {
        public LZNT1_Chunk[] Chunk;

        public override void Marshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            foreach (var chunk in Chunk)
            {
                chunk.Marshal(buffer, ref offset);
            }
        }

        public override object Unmarshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            var result = new LZNT1_Buffer();

            var chunks = new List<LZNT1_Chunk>();
            while (offset < buffer.Count)
            {
                var chunk = Unmarshal<LZNT1_Chunk>(buffer, ref offset);
                chunks.Add(chunk);
            }

            result.Chunk = chunks.ToArray();

            return result;
        }
    }

    class LZNT1_Chunk : LZNT1_Object
    {
        public LZNT1_ChunkHeader Chunk_header;

        public override object Unmarshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            int offset1 = offset;
            var header = Unmarshal<LZNT1_ChunkHeader>(buffer, ref offset1);
            int value = header.Value;
            if (value == 0)
            {
                var result = Unmarshal<LZNT1_EndOfBuffer>(buffer, ref offset);
                return result;
            }
            bool compressed = (value & 0x8000) != 0;
            if (compressed)
            {
                var result = Unmarshal<LZNT1_CompressedChunk>(buffer, ref offset);
                return result;
            }
            else
            {
                var result = Unmarshal<LZNT1_UncompressedChunk>(buffer, ref offset);
                return result;
            }
        }

        public virtual List<LZ77Symbol> ParseToLZ77Symbols()
        {
            throw new XcaException("Unreachable code!");
        }

        public static LZNT1_Chunk Compress(byte[] arg, List<LZNT1_Data> data)
        {
            int compressedLength = CalculateCompressedLength(data);

            if (compressedLength < arg.Length)
            {
                var compressedChunk = new LZNT1_CompressedChunk();
                compressedChunk.Chunk_header = new LZNT1_ChunkHeader();
                compressedChunk.Chunk_header.Value = 0xB000;
                compressedChunk.Chunk_header.Value |= compressedLength - 3;
                compressedChunk.Flag_group = new LZNT1_FlagGroup();
                var flagData = new List<LZNT1_FlagData>();

                int i = 0;
                while (i < data.Count)
                {
                    int groupLength = Math.Min(8, data.Count - i);

                    var group = data.Skip(i).Take(groupLength);

                    var flagDataItem = new LZNT1_FlagData(group.ToArray());

                    flagData.Add(flagDataItem);

                    i += groupLength;
                }

                compressedChunk.Flag_group.Flag_data = flagData.ToArray();
                return compressedChunk;
            }
            else
            {
                var uncompressedChunk = new LZNT1_UncompressedChunk();
                uncompressedChunk.Chunk_header = new LZNT1_ChunkHeader();
                uncompressedChunk.Chunk_header.Value = 0x3000;
                uncompressedChunk.Chunk_header.Value |= arg.Length - 1;
                uncompressedChunk.Uncompressed_data = arg.ToArray();
                return uncompressedChunk;
            }
        }

        private static int CalculateCompressedLength(List<LZNT1_Data> data)
        {
            int i = 0;
            int result = 2;
            while (i < data.Count)
            {
                int groupLength = Math.Min(8, data.Count - i);

                var group = data.Skip(i).Take(groupLength);

                int compressedGroupLength = group.Sum(delegate (LZNT1_Data x)
                {
                    if (x is LZNT1_Literal)
                    {
                        return 1;
                    }
                    else if (x is LZNT1_CompressedWord)
                    {
                        return 2;
                    }
                    else
                    {
                        throw new XcaException("Unreachable code!");
                    }
                });

                result += compressedGroupLength + 1;

                i += groupLength;
            }

            return result;
        }

        public override void Marshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            throw new XcaException("Unreachable code!");
        }
    }

    class LZNT1_ChunkHeader : LZNT1_Object
    {
        public int Value;

        public override void Marshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            buffer.WriteBytes(offset, Value, 2);
            offset += 2;
        }

        public override object Unmarshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            var result = new LZNT1_ChunkHeader();
            result.Value = buffer.ReadBytes(offset, 2);
            offset += 2;
            return result;
        }
    }

    class LZNT1_EndOfBuffer : LZNT1_Chunk
    {
        public override void Marshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            // Do not marshal since its optional.
        }

        public override object Unmarshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            var result = new LZNT1_EndOfBuffer();
            result.Chunk_header = Unmarshal<LZNT1_ChunkHeader>(buffer, ref offset);
            int value = result.Chunk_header.Value;
            if (value != 0)
            {
                throw new XcaException("Unreachable code!");
            }
            return result;
        }

        public override List<LZ77Symbol> ParseToLZ77Symbols()
        {
            var result = new List<LZ77Symbol>();
            result.Add(new LZ77EOF());
            return result;
        }
    }

    class LZNT1_CompressedChunk : LZNT1_Chunk
    {
        public LZNT1_FlagGroup Flag_group;

        public override void Marshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            Chunk_header.Marshal(buffer, ref offset);
            Flag_group.Marshal(buffer, ref offset);
        }

        public override object Unmarshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            var result = new LZNT1_CompressedChunk();
            result.Chunk_header = Unmarshal<LZNT1_ChunkHeader>(buffer, ref offset);
            int value = result.Chunk_header.Value;
            bool compressed = (value & 0x8000) != 0;
            if (!compressed)
            {
                throw new XcaException("Unreachable code!");
            }
            int signature = (value & 0x7000) >> 12;
            if (signature != 3)
            {
                throw new XcaException("[Data error]: Wrong signature!");
            }
            int size = (value & 0x0FFF) + 1;
            var subBuffer = buffer.SubCopy(offset, size);
            int subOffset = 0;
            result.Flag_group = Unmarshal<LZNT1_FlagGroup>(subBuffer, ref subOffset);
            if (subOffset != size)
            {
                throw new XcaException("[Data error]: Data length is inconsistent!");
            }
            offset += size;

            return result;
        }

        public override List<LZ77Symbol> ParseToLZ77Symbols()
        {
            var result = Flag_group.ParseToLZ77Symbols();
            return result;
        }
    }

    class LZNT1_FlagGroup : LZNT1_Object
    {
        public LZNT1_FlagData[] Flag_data;

        public override object Unmarshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            var result = new LZNT1_FlagGroup();
            var data = new List<LZNT1_FlagData>();

            while (offset < buffer.Count)
            {
                var flag = Unmarshal<LZNT1_FlagData>(buffer, ref offset);
                data.Add(flag);
            }

            result.Flag_data = data.ToArray();

            return result;
        }

        public List<LZ77Symbol> ParseToLZ77Symbols()
        {
            int proccessedBytes = 0;
            var result = Flag_data.SelectMany(x => x.ParseToLZ77Symbols(ref proccessedBytes));
            return result.ToList();
        }

        public override void Marshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            foreach (var data in Flag_data)
            {
                data.Marshal(buffer, ref offset);
            }
        }
    }

    class LZNT1_FlagData : LZNT1_Object
    {
        public LZNT1_FlagData()
        {

        }

        public LZNT1_FlagData(LZNT1_Data[] data)
        {
            if (data.Length <= 0 || data.Length > 8)
            {
                throw new XcaException("[Data error]: Too much data!");
            }

            Data = data;

            Flag_byte = 0;
            for (int i = 0; i < Data.Length; i++)
            {
                var dataItem = Data[i];
                if (dataItem is LZNT1_CompressedWord)
                {
                    Flag_byte |= (byte)(1 << i);
                }
            }
        }

        public byte Flag_byte;
        public LZNT1_Data[] Data;

        public override object Unmarshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            var result = new LZNT1_FlagData();

            result.Flag_byte = (byte)buffer.ReadBytes(offset, 1);
            offset++;

            var data = new List<LZNT1_Data>();

            for (int i = 0; i < 8; i++)
            {
                if (offset < buffer.Count)
                {
                    bool compressed = (result.Flag_byte & (1 << i)) != 0;

                    if (compressed)
                    {
                        data.Add(Unmarshal<LZNT1_CompressedWord>(buffer, ref offset));
                    }
                    else
                    {
                        data.Add(Unmarshal<LZNT1_Literal>(buffer, ref offset));
                    }
                }
            }

            result.Data = data.ToArray();

            return result;
        }

        public List<LZ77Symbol> ParseToLZ77Symbols(ref int processedBytes)
        {
            var result = new List<LZ77Symbol>();

            foreach (var data in Data)
            {
                result.Add(data.ParseToLZ77Symbol(ref processedBytes));
            }

            return result;
        }

        public override void Marshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            buffer.WriteBytes(offset, Flag_byte, 1);
            offset++;
            foreach (var data in Data)
            {
                data.Marshal(buffer, ref offset);
            }
        }
    }

    abstract class LZNT1_Data : LZNT1_Object
    {
        public abstract LZ77Symbol ParseToLZ77Symbol(ref int processedBytes);

        public static List<LZNT1_Data> GenerateFromLZ77Symbol(byte[] arg, LZ77Symbol symbol, ref int processedBytes)
        {
            var result = new List<LZNT1_Data>();
            if (symbol is LZ77Literal)
            {
                var literal = symbol as LZ77Literal;
                result.Add(LZNT1_Literal.GenerateFromLZ77Literal(literal, ref processedBytes));
            }
            else if (symbol is LZ77Match)
            {
                var match = symbol as LZ77Match;
                result.AddRange(LZNT1_CompressedWord.GenerateFromLZ77Match(arg, match, ref processedBytes));

            }
            else if (symbol is LZ77EOF)
            {
                // do nothing to EOF.
            }
            else
            {
                throw new XcaException("Unreachable code!");
            }
            return result;
        }
    }

    class LZNT1_Literal : LZNT1_Data
    {
        public byte Literal;

        public override object Unmarshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            var result = new LZNT1_Literal();
            result.Literal = (byte)buffer.ReadBytes(offset, 1);
            offset++;

            return result;
        }

        public override LZ77Symbol ParseToLZ77Symbol(ref int processedBytes)
        {
            var result = new LZ77Literal() { Literal = Literal };
            processedBytes++;
            return result;
        }

        public static LZNT1_Literal GenerateFromLZ77Literal(LZ77Literal literal, ref int processedBytes)
        {
            var result = new LZNT1_Literal();
            result.Literal = literal.Literal;
            processedBytes++;
            return result;
        }

        public override void Marshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            buffer.WriteBytes(offset, Literal, 1);
            offset++;
        }
    }

    class LZNT1_CompressedWord : LZNT1_Data
    {
        public int Value;

        public override object Unmarshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            var result = new LZNT1_CompressedWord();
            result.Value = buffer.ReadBytes(offset, 2);
            offset += 2;

            return result;
        }

        public override LZ77Symbol ParseToLZ77Symbol(ref int processedBytes)
        {
            var displacementBits = GetDisplcaementBits(processedBytes);
            int diplacement = (Value >> (16 - displacementBits)) + 1;
            int length = (Value & ((1 << (16 - displacementBits)) - 1)) + 3;
            var result = new LZ77Match() { Distance = diplacement, Length = length };
            processedBytes += length;
            return result;
        }

        private static int GetDisplcaementBits(int processedBytes)
        {
            int result = 4;
            for (int i = 4; i <= 12; i++)
            {
                if ((1 << (i - 1)) < processedBytes)
                {
                    result = i;
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        private static int GetMaximumLength(int processedBytes)
        {
            int displacementBits = GetDisplcaementBits(processedBytes);

            int result = (1 << (16 - displacementBits)) - 1 + 3;

            return result;
        }

        public static List<LZNT1_Data> GenerateFromLZ77Match(byte[] arg, LZ77Match match, ref int processedBytes)
        {
            var result = new List<LZNT1_Data>();

            int remainingLength = match.Length;
            while (remainingLength >= 3)
            {
                var compressedWord = new LZNT1_CompressedWord();
                int displacementBits = GetDisplcaementBits(processedBytes);
                int maximumLength = GetMaximumLength(processedBytes);
                int length = Math.Min(remainingLength, maximumLength);
                compressedWord.Value = 0;
                compressedWord.Value |= (length - 3) & ((1 << (16 - displacementBits)) - 1);
                compressedWord.Value |= ((match.Distance - 1) & ((1 << displacementBits) - 1)) << (16 - displacementBits);
                result.Add(compressedWord);
                processedBytes += length;
                remainingLength -= length;
            }
            for (int i = 0; i < remainingLength; i++)
            {
                var literal = new LZNT1_Literal();
                literal.Literal = arg[processedBytes - match.Distance];
                result.Add(literal);
                processedBytes++;
            }
            return result;
        }

        public override void Marshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            buffer.WriteBytes(offset, Value, 2);
            offset += 2;
        }
    }

    class LZNT1_UncompressedChunk : LZNT1_Chunk
    {
        public byte[] Uncompressed_data;

        public override void Marshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            Chunk_header.Marshal(buffer, ref offset);
            foreach (byte b in Uncompressed_data)
            {
                buffer.WriteBytes(offset, b, 1);
                offset++;
            }
        }

        public override object Unmarshal(LittleEndianByteBuffer buffer, ref int offset)
        {
            var result = new LZNT1_UncompressedChunk();
            result.Chunk_header = Unmarshal<LZNT1_ChunkHeader>(buffer, ref offset);
            int value = result.Chunk_header.Value;
            bool compressed = (value & 0x8000) != 0;
            if (compressed)
            {
                throw new XcaException("Unreachable code!");
            }
            int signature = (value & 0x7000) >> 12;
            if (signature != 3)
            {
                throw new XcaException("[Data error]: Wrong signature!");
            }
            int size = (value & 0x0FFF) + 1;
            result.Uncompressed_data = new byte[size];
            for (int i = 0; i < size; i++)
            {
                result.Uncompressed_data[i] = (byte)buffer.ReadBytes(offset + i, 1);
            }
            offset += size;
            return result;
        }

        public override List<LZ77Symbol> ParseToLZ77Symbols()
        {
            var result = Uncompressed_data.Select(x => new LZ77Literal() { Literal = x } as LZ77Symbol);

            return result.ToList();
        }
    }
}
