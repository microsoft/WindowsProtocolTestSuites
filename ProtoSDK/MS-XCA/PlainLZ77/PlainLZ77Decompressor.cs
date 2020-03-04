// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress
{
    /// <summary>
    /// Decompressor Implementation of Plain LZ77 Decompression Algorithm
    /// </summary>
    public class PlainLZ77Decompressor : XcaDecompressor
    {
        /// <summary>
        /// Decompress data.
        /// </summary>
        /// <param name="data">Data to be decompressed.</param>
        /// <returns>Decompressed Data.</returns>
        public byte[] Decompress(byte[] data)
        {
            /*
                	BufferedFlags = 0
	                BufferedFlagCount = 0
	                InputPosition = 0
	                OutputPosition = 0
	                LastLengthHalfByte = 0
	                Loop until break instruction or error
	                    If BufferedFlagCount == 0
	                        BufferedFlags = read 4 bytes at InputPosition
	                        InputPosition += 4
	                        BufferedFlagCount = 32
	                    BufferedFlagCount = BufferedFlagCount – 1
	                    If (BufferedFlags & (1 << BufferedFlagCount)) == 0
	                        Copy 1 byte from InputPosition to OutputPosition.  Advance both.
	                    Else
	                        If InputPosition == InputBufferSize
	                            Decompression is complete.  Return with success.
	                        MatchBytes = read 2 bytes from InputPosition
	                        InputPosition += 2
	                        MatchLength = MatchBytes mod 8
	                        MatchOffset = (MatchBytes / 8) + 1
	                        If MatchLength == 7
	                            If LastLengthHalfByte == 0
	                                MatchLength = read 1 byte from InputPosition
	                                MatchLength = MatchLength mod 16
	                                LastLengthHalfByte = InputPosition
	                                InputPosition += 1
	                            Else
	                                MatchLength = read 1 byte from LastLengthHalfByte position
	                                MatchLength = MatchLength / 16
	                                LastLengthHalfByte = 0
	                            If MatchLength == 15
	                                MatchLength = read 1 byte from InputPosition
	                                InputPosition += 1
	                                If MatchLength == 255
	                                    MatchLength = read 2 bytes from InputPosition
	                                    InputPosition += 2
	                                    If MatchLength == 0
	                                        MatchLength = read 4 bytes from InputPosition
	                                        InputPosition += 4 bytes
	                                    If MatchLength < 15 + 7
	                                       Return error.
	                                    MatchLength -= (15 + 7)
	                                MatchLength += 15
	                            MatchLength += 7
	                        MatchLength += 3
	                        For i = 0 to MatchLength – 1
	                            Copy 1 byte from OutputBuffer[OutputPosition – MatchOffset]
	                            OutputPosition += 1
             */


            int BufferedFlags = 0;
            int BufferedFlagCount = 0;
            int InputPosition = 0;
            int OutputPosition = 0;
            int LastLengthHalfByte = 0;

            int InputBufferSize = data.Length;
            int MatchLength;

            var inputReader = new LittleEndianByteBuffer(data);
            var outputWriter = new LittleEndianByteBuffer();

            while (true)
            {
                if (BufferedFlagCount == 0)
                {
                    BufferedFlags = inputReader.ReadBytes(InputPosition, 4);
                    InputPosition += 4;
                    BufferedFlagCount = 32;
                }
                BufferedFlagCount -= 1;
                if ((BufferedFlags & (1 << BufferedFlagCount)) == 0)
                {
                    byte literal = (byte)inputReader.ReadBytes(InputPosition, 1);
                    outputWriter.WriteBytes(OutputPosition, literal, 1);
                    InputPosition++;
                    OutputPosition++;
                }
                else
                {
                    if (InputPosition == InputBufferSize)
                    {
                        break;
                    }


                    int MatchBytes = inputReader.ReadBytes(InputPosition, 2);

                    InputPosition += 2;
                    MatchLength = MatchBytes % 8;
                    int MatchOffset = (MatchBytes / 8) + 1;
                    if (MatchLength == 7)
                    {
                        if (LastLengthHalfByte == 0)
                        {
                            MatchLength = inputReader.ReadBytes(InputPosition, 1);
                            MatchLength %= 16;
                            LastLengthHalfByte = InputPosition;
                            InputPosition += 1;
                        }
                        else
                        {
                            MatchLength = inputReader.ReadBytes(LastLengthHalfByte, 1);
                            MatchLength /= 16;
                            LastLengthHalfByte = 0;
                        }
                        if (MatchLength == 15)
                        {
                            MatchLength = inputReader.ReadBytes(InputPosition, 1);
                            InputPosition += 1;
                            if (MatchLength == 255)
                            {
                                MatchLength = inputReader.ReadBytes(InputPosition, 2);

                                InputPosition += 2;

                                if (MatchLength == 0)
                                {
                                    MatchLength = inputReader.ReadBytes(InputPosition, 4);

                                    InputPosition += 4;
                                }

                                if (MatchLength < 15 + 7)
                                {
                                    throw new XcaException("[Data error]: Match length is invalid!");
                                }
                                MatchLength -= (15 + 7);
                            }
                            MatchLength += 15;
                        }
                        MatchLength += 7;
                    }
                    MatchLength += 3;
                    for (int i = 0; i <= MatchLength - 1; i++)
                    {
                        int matchPosition = OutputPosition - MatchOffset;
                        if (matchPosition < 0 || matchPosition >= outputWriter.Count)
                        {
                            throw new XcaException("[Data error]: Match offset is invalid!");
                        }
                        var matchByte = outputWriter[matchPosition];
                        outputWriter.WriteBytes(OutputPosition, matchByte, 1);
                        OutputPosition += 1;
                    }
                }
            }
            return outputWriter.GetBytes();
        }
    }
}
