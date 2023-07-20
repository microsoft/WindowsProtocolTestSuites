// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;

namespace Microsoft.Protocols.TestSuites.MS_XCA.Compression
{
    [TestClass]
    public class Compression : XcaTestClassBase
    {
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            Initialize(testContext);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            Cleanup();
        }

        /// <summary>
        /// This tests compression of the ASCII string abcdefghijklmnopqrstuvwxyz 
        /// as specified in MS-XCA document
        /// </summary>
        [TestMethod]
        [TestCategory("Compression")]
        [TestCategory("StaticData")]
        [TestCategory("LZ77")]
        public void Compression_LZ77_StaticData_01()
        {
            TestCompression(new PlainLZ77Compressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77, defaultLZ77CompressionInput01);
        }

        /// <summary>
        /// This tests compression of the 300 byte ASCII string 
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// as specified in MS-XCA document
        /// </summary>
        [TestMethod]
        [TestCategory("Compression")]
        [TestCategory("StaticData")]
        [TestCategory("LZ77")]
        public void Compression_LZ77_StaticData_02()
        {
            TestCompression(new PlainLZ77Compressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77, defaultLZ77CompressionInput02);
        }

        /// <summary>
        /// This tests compression of the ASCII string abcdefghijklmnopqrstuvwxyz
        /// multiplied by 1000
        /// </summary>
        [TestMethod]
        [TestCategory("Compression")]
        [TestCategory("StaticData")]
        [TestCategory("LZ77")]
        public void Compression_LZ77_StaticData_Large_01()
        {
            TestCompression(new PlainLZ77Compressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77, defaultLZ77CompressionInputLarge01);
        }

        /// <summary>
        /// This tests compression of the 300 byte ASCII string 
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// multiplied by 1000
        /// </summary>
        [TestMethod]
        [TestCategory("Compression")]
        [TestCategory("StaticData")]
        [TestCategory("LZ77")]
        public void Compression_LZ77_StaticData_Large_02()
        {
            TestCompression(new PlainLZ77Compressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77, defaultLZ77CompressionInputLarge02);
        }

        /// <summary>
        /// This tests compression of the ASCII string abcdefghijklmnopqrstuvwxyz 
        /// as specified in MS-XCA document
        /// </summary>
        [TestMethod]
        [TestCategory("Compression")]
        [TestCategory("StaticData")]
        [TestCategory("LZ77+Huffman")]
        public void Compression_LZ77Huffman_StaticData_01()
        {
            TestCompression(new LZ77HuffmanCompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77_HUFF, defaultLZ77CompressionInput01);
        }

        /// <summary>
        /// This tests compression of the 300 byte ASCII string 
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// as specified in MS-XCA document
        /// </summary>
        [TestMethod]
        [TestCategory("Compression")]
        [TestCategory("StaticData")]
        [TestCategory("LZ77+Huffman")]
        public void Compression_LZ77Huffman_StaticData_02()
        {
            TestCompression(new LZ77HuffmanCompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77_HUFF, defaultLZ77CompressionInput02);
        }

        /// <summary>
        /// This tests compression of the ASCII string abcdefghijklmnopqrstuvwxyz
        /// multiplied by 1000
        /// </summary>
        [TestMethod]
        [TestCategory("Compression")]
        [TestCategory("StaticData")]
        [TestCategory("LZ77+Huffman")]
        public void Compression_LZ77Huffman_StaticData_Large_01()
        {
            TestCompression(new LZ77HuffmanCompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77_HUFF, defaultLZ77CompressionInputLarge01);
        }

        /// <summary>
        /// This tests compression of the 300 byte ASCII string 
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// multiplied by 1000
        /// </summary>
        [TestMethod]
        [TestCategory("Compression")]
        [TestCategory("StaticData")]
        [TestCategory("LZ77+Huffman")]
        public void Compression_LZ77Huffman_StaticData_Large_02()
        {
            TestCompression(new LZ77HuffmanCompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77_HUFF, defaultLZ77CompressionInputLarge02);
        }

        /// <summary>
        /// This tests compression of the ASCII string
        /// "F# F# G A A G F# E D D E F# F# E E F# F# G A A G F# E D D E F# E D D E E F# D E F# G F# D E F# G F# E D E A F# F# G A A G F# E D D E F# E D D\0"
        /// as specified in MS-XCA document
        /// </summary>
        [TestMethod]
        [TestCategory("Compression")]
        [TestCategory("StaticData")]
        [TestCategory("LZNT1")]
        public void Compression_LZNT1_StaticData()
        {
            TestCompression(new LZNT1Compressor(), COMPRESS_ALGORITHM.ALGORITHM_LZNT1, lznt1CompressionInput);
        }

        /// <summary>
        /// This tests compression of the ASCII string
        /// "F# F# G A A G F# E D D E F# F# E E F# F# G A A G F# E D D E F# E D D E E F# D E F# G F# D E F# G F# E D E A F# F# G A A G F# E D D E F# E D D\0"
        /// multiplied by 1000
        /// </summary>
        [TestMethod]
        [TestCategory("Compression")]
        [TestCategory("StaticData")]
        [TestCategory("LZNT1")]
        public void Compression_LZNT1_StaticData_Large()
        {
            TestCompression(new LZNT1Compressor(), COMPRESS_ALGORITHM.ALGORITHM_LZNT1, lznt1CompressionInputLarge);
        }

        private void TestCompression(XcaCompressor compressor, COMPRESS_ALGORITHM algorithm, string inputFile, [CallerMemberName] string callingMethod = "")
        {
            var testOutputFile = GetStaticDataOutputFilename(inputFile, true, true, callingMethod);
            var implOutputFile = GetStaticDataOutputFilename(inputFile, true, false, callingMethod);

            // Read input data
            Site.Log.Add(LogEntryKind.TestStep, $"1. TestSuite reads and compress input");
            var testData = File.ReadAllBytes(inputFile);
            Site.Log.Add(LogEntryKind.Comment, $"Read {testData.Length} bytes");
            
            // Compress file
            var compressedData = compressor.Compress(testData);
            Site.Log.Add(LogEntryKind.Comment, $"Compressed {compressedData.Length} bytes");
            File.WriteAllBytes(testOutputFile, compressedData);

            // Invoke test application
            Site.Log.Add(LogEntryKind.TestStep, $"2. Invoke SUT");
            _ = InvokeCompression(Path.GetFullPath(inputFile), Path.GetFullPath(implOutputFile), algorithm);

            // Compare test app result with local result
            Site.Log.Add(LogEntryKind.TestStep, $"3. Compare TestSuite output with SUT output");
            byte[] testCompressedData = new byte[compressedData.Length];
            using (FileStream fileStream = new FileStream(implOutputFile, FileMode.Open))
            {
                int bytesRead = fileStream.Read(testCompressedData, 0, testCompressedData.Length);
                Site.Assert.AreEqual(compressedData.Length, bytesRead, "The bytes read should be the same as the compressed bytes");
            }

            bool matchingSequence = compressedData.SequenceEqual(testCompressedData);
            Site.Assert.IsTrue(matchingSequence, "The compressed outputs should match");
        }
    }
}
