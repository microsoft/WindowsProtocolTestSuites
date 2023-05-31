// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress;

namespace XcaTestApp
{
    class Program
    {
        public enum COMPRESS_ALGORITHM
        {
            ALGORITHM_LZ777 = 1,
            ALGORITHM_LZ777_HUFF = 2,
            ALGORITHM_LZNT1 = 3
        }

        static void Main(string[] args)
        {
            if (args.Contains("-h", StringComparer.OrdinalIgnoreCase) || args.Length == 0)
            {
                Console.WriteLine("\n Test MS-XCA Compression and Decompression");
                Console.WriteLine("\n -c : Compress (default)");
                Console.WriteLine(" -d : Decompress");
                Console.WriteLine(" -in : File to compress/decompress");
                Console.WriteLine(" -out : Path for output file");
                Console.WriteLine(" -alg : Algorithm to use. ");
                Console.WriteLine("\n Available Algorithms:");
                Console.WriteLine("                       1 = LZ777");
                Console.WriteLine("                       2 = LZ777+HUFFMAN");
                Console.WriteLine("                       3 = LZNT1");
                Console.WriteLine("\n Example Usage: XcaTestApp.exe -c -in C:\\path\\to\\input.txt -out .\\outputfile -alg 1");

                Environment.Exit(0);
            }

            string inputFile = "";
            string outputFile = "";
            bool isCompression = true;
            COMPRESS_ALGORITHM alg = 0;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-in" && !args[i + 1].StartsWith("-"))
                {
                    if (File.Exists(args[i + 1]))
                    {
                        inputFile = Path.GetFullPath(args[i + 1]);
                    }
                }

                if (args[i] == "-out" && !args[i + 1].StartsWith("-"))
                {
                    outputFile = Path.GetFullPath(args[i + 1]);
                }

                if (args[i] == "-d")
                {
                    isCompression = false;
                }

                if (args[i] == "-alg" && !args[i + 1].StartsWith("-"))
                {
                    switch (args[i + 1])
                    {
                        case "1":
                            alg = COMPRESS_ALGORITHM.ALGORITHM_LZ777;
                            break;
                        case "2":
                            alg = COMPRESS_ALGORITHM.ALGORITHM_LZ777_HUFF;
                            break;
                        case "3":
                            alg = COMPRESS_ALGORITHM.ALGORITHM_LZNT1;
                            break;
                    }
                }
            }

            if (inputFile == "")
            {
                Console.WriteLine(" [!] No input file specified.\n Use -in to specify an input file.");
                Environment.Exit(1);
            }

            if (outputFile == "")
            {
                Console.WriteLine(" [!] No output file specified.\n Use -out to specify an output file.");
                Environment.Exit(1);
            }

            if (alg == 0)
            {
                Console.WriteLine(" [!] No Algorithm specified!\n Use -alg to specify an algorithm");
                Environment.Exit(1);
            }

            if (isCompression)
            {
                PerformCompression(inputFile, outputFile, alg);
            }
            else
            {
                PerformDecompression(inputFile, outputFile, alg);
            }
        }

        static void PerformCompression(string inputFile, string outputFile, COMPRESS_ALGORITHM alg)
        {
            Console.WriteLine($"Compressing using {alg}");
            var compressor = GetCompressor(alg);
            var testData = File.ReadAllBytes(inputFile);
            Console.WriteLine($"Read {testData.Length} bytes");

            var compressedData = compressor.Compress(testData);
            Console.WriteLine($"Compressed {compressedData.Length} bytes");

            File.WriteAllBytes(outputFile, compressedData);
        }

        static void PerformDecompression(string inputFile, string outputFile, COMPRESS_ALGORITHM alg)
        {
            var decompressor = GetDecompressor(alg);
            if (decompressor == null)
            {
                Console.WriteLine($"Internal failure while trying to decompress using {alg}");
                return;
            }
            var testData = File.ReadAllBytes(inputFile);
            var decompressedData = decompressor.Decompress(testData);

            File.WriteAllBytes(outputFile, decompressedData);
        }

        static XcaCompressor GetCompressor(COMPRESS_ALGORITHM alg)
        {
            return alg switch
            {
                COMPRESS_ALGORITHM.ALGORITHM_LZ777 => new PlainLZ77Compressor(),
                COMPRESS_ALGORITHM.ALGORITHM_LZ777_HUFF => new LZ77HuffmanCompressor(),
                COMPRESS_ALGORITHM.ALGORITHM_LZNT1 => new LZNT1Compressor(),
                _ => throw new InvalidOperationException(),
            };
        }

        static XcaDecompressor? GetDecompressor(COMPRESS_ALGORITHM alg)
        {
            return alg switch
            {
                COMPRESS_ALGORITHM.ALGORITHM_LZ777 => new PlainLZ77Decompressor(),
                COMPRESS_ALGORITHM.ALGORITHM_LZ777_HUFF => new LZ77HuffmanDecompressor(),
                COMPRESS_ALGORITHM.ALGORITHM_LZNT1 => new LZNT1Decompressor(),
                _ => null,
            };
        }
    }
}