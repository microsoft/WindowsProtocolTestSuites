// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;

namespace Microsoft.Protocols.TestSuites.MS_XCA.Decompression
{
    [TestClass]
    public class Decompression : XcaTestClassBase
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
        /// This tests decompression of the compressed ASCII string abcdefghijklmnopqrstuvwxyz as specified in MS-XCA document
        /// </summary>
        [TestMethod]
        [TestCategory("Decompression")]
        [TestCategory("TestData")]
        [TestCategory("LZ77")]
        public void Decompression_LZ77_TestData_01()
        {
            TestDecompression(new PlainLZ77Decompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77, plainLZ77DecompressionInput01);
        }
        
        /// <summary>
        /// This tests decompression of the compressed 300 byte ASCII string 
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// as specified in MS-XCA document
        /// </summary>
        [TestMethod]
        [TestCategory("Decompression")]
        [TestCategory("TestData")]
        [TestCategory("LZ77")]
        public void Decompression_LZ77_TestData_02()
        {
            TestDecompression(new PlainLZ77Decompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77, plainLZ77DecompressionInput02);
        }

        /// <summary>
        /// This tests decompression of the compressed ASCII string abcdefghijklmnopqrstuvwxyz
        /// multiplied by 1000
        /// </summary>
        [TestMethod]
        [TestCategory("Decompression")]
        [TestCategory("TestData")]
        [TestCategory("LZ77")]
        public void Decompression_LZ77_TestData_Large_01()
        {
            TestDecompression(new PlainLZ77Decompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77, plainLZ77DecompressionInputLarge01);
        }

        /// <summary>
        /// This tests decompression of the compressed 300 byte ASCII string 
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// multiplied by 1000
        /// </summary>
        [TestMethod]
        [TestCategory("Decompression")]
        [TestCategory("TestData")]
        [TestCategory("LZ77")]
        public void Decompression_LZ77_TestData_Large_02()
        {
            TestDecompression(new PlainLZ77Decompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77,  plainLZ77DecompressionInputLarge02);
        }
        
        /// <summary>
        /// This tests decompression of the compressed ASCII string abcdefghijklmnopqrstuvwxyz as specified in MS-XCA document
        /// </summary>
        [TestMethod]
        [TestCategory("Decompression")]
        [TestCategory("TestData")]
        [TestCategory("LZ77+Huffman")]
        public void Decompression_LZ77Huffman_TestData_01()
        {
            TestDecompression(new LZ77HuffmanDecompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77_HUFF, lz77HuffmanDecompressionInput01);
        }
        
        /// <summary>
        /// This tests decompression of the compressed 300 byte ASCII string 
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// as specified in MS-XCA document
        /// </summary>
        [TestMethod]
        [TestCategory("Decompression")]
        [TestCategory("TestData")]
        [TestCategory("LZ77+Huffman")]
        public void Decompression_LZ77Huffman_TestData_02()
        {
            TestDecompression(new LZ77HuffmanDecompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77_HUFF, lz77HuffmanDecompressionInput02);
        }

        /// <summary>
        /// This tests decompression of the compressed ASCII string abcdefghijklmnopqrstuvwxyz
        /// multiplied by 1000
        /// </summary>
        [TestMethod]
        [TestCategory("Decompression")]
        [TestCategory("TestData")]
        [TestCategory("LZ77+Huffman")]
        public void Decompression_LZ77Huffman_TestData_Large_01()
        {
            TestDecompression(new LZ77HuffmanDecompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77_HUFF, lz77HuffmanDecompressionInputLarge01);
        }

        /// <summary>
        /// This tests decompression of the compressed 300 byte ASCII string 
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// 	abcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabcabc
        /// multiplied by 1000
        /// </summary>
        [TestMethod]
        [TestCategory("Decompression")]
        [TestCategory("TestData")]
        [TestCategory("LZ77+Huffman")]
        public void Decompression_LZ77Huffman_TestData_Large_02()
        {
            TestDecompression(new LZ77HuffmanDecompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77_HUFF,  lz77HuffmanDecompressionInputLarge02);
        }

        /// <summary>
        /// This tests compression of the ASCII string
        /// "F# F# G A A G F# E D D E F# F# E E F# F# G A A G F# E D D E F# E D D E E F# D E F# G F# D E F# G F# E D E A F# F# G A A G F# E D D E F# E D D\0"
        /// as specified in MS-XCA document
        /// </summary>
        [TestMethod]
        [TestCategory("Decompression")]
        [TestCategory("TestData")]
        [TestCategory("LZNT1")]
        public void Decompression_LZNT1_TestData()
        {
            TestDecompression(new LZNT1Decompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZNT1, lznt1DecompressionInput);
        }

        /// <summary>
        /// This tests compression of the ASCII string
        /// "F# F# G A A G F# E D D E F# F# E E F# F# G A A G F# E D D E F# E D D E E F# D E F# G F# D E F# G F# E D E A F# F# G A A G F# E D D E F# E D D\0"
        /// multiplied by 1000
        /// </summary>
        [TestMethod]
        [TestCategory("Decompression")]
        [TestCategory("TestData")]
        [TestCategory("LZNT1")]
        public void Decompression_LZNT1_TestData_Large()
        {
            TestDecompression(new LZNT1Decompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZNT1, lznt1DecompressionInputLarge);
        }

        private void TestDecompression(XcaDecompressor decompressor, COMPRESS_ALGORITHM algorithm, string inputFile, [CallerMemberName] string callingMethod = "")
        {
            var testOutputFile = GetTestDataOutputFilename(inputFile, false, true, callingMethod);
            var implOutputFile = GetTestDataOutputFilename(inputFile, false, false, callingMethod);

            // Read input data
            Site.Log.Add(LogEntryKind.TestStep, $"1. TestSuite reads and decompress input");
            var testData = File.ReadAllBytes(inputFile);
            Site.Log.Add(LogEntryKind.Comment, $"Read {testData.Length} bytes");

            // Decompress file
            var decompressedData = decompressor.Decompress(testData);
            Site.Log.Add(LogEntryKind.Comment, $"Decompressed {decompressedData.Length} bytes");
            File.WriteAllBytes(testOutputFile, decompressedData);

            // Invoke test application
            Site.Log.Add(LogEntryKind.TestStep, $"2. Invoke SUT");
            _ = InvokeDecompression(Path.GetFullPath(inputFile), Path.GetFullPath(implOutputFile), algorithm);

            // Compare test app result with local result
            Site.Log.Add(LogEntryKind.TestStep, $"3. Compare TestSuite output with SUT output");
            byte[] testDecompressedData = new byte[decompressedData.Length];
            using (FileStream fileStream = new(implOutputFile, FileMode.Open))
            {
                int bytesRead = fileStream.Read(testDecompressedData, 0, testDecompressedData.Length);
                Site.Assert.AreEqual(decompressedData.Length, bytesRead, "The bytes read should be the same as the decompressed bytes");
            }

            bool matchingSequence = decompressedData.SequenceEqual(testDecompressedData);
            Site.Assert.IsTrue(matchingSequence, "The decompressed outputs should match");
        }
    }
}
