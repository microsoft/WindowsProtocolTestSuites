// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;

namespace Microsoft.Protocols.TestSuites.MS_XCA.Compression
{
    [TestClass]
    public class UserDataCompression : XcaTestClassBase
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
        /// This tests compression of all the files in a User specified folder using the LZ77 algorithm
        /// </summary>
        [TestMethod]
        [TestCategory("Compression")]
        [TestCategory("UserData")]
        [TestCategory("LZ77")]
        public void Compression_LZ77_UserData()
        {
            TestCompression(new PlainLZ77Compressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77);
        }

        /// <summary>
        /// This tests compression of all the files in a User specified folder using the LZ77 + Huffman algorithm
        /// </summary>
        [TestMethod]
        [TestCategory("Compression")]
        [TestCategory("UserData")]
        [TestCategory("LZ77+Huffman")]
        public void Compression_LZ77Huffman_UserData()
        {
            TestCompression(new LZ77HuffmanCompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77_HUFF);
        }

        /// <summary>
        /// This tests compression of all the files in a User specified folder using the LZNT1 algorithm
        /// </summary>
        [TestMethod]
        [TestCategory("Compression")]
        [TestCategory("UserData")]
        [TestCategory("LZNT1")]
        public void Compression_LZNT1_UserData()
        {
            TestCompression(new LZNT1Compressor(), COMPRESS_ALGORITHM.ALGORITHM_LZNT1);
        }

        /// <summary>
        /// Perform the compression on all the files in the User specified folder
        /// </summary>
        /// <param name="compressor"></param>
        private void TestCompression(XcaCompressor compressor, COMPRESS_ALGORITHM algorithm, [CallerMemberName] string callingMethod = "")
        {
            List<TestStatus> statusList = new();

            // 1. Get all the Files in the UserData folder
            var files = GetUserDataInputFiles(true, algorithm);

            // 2. Call the Compress function for each file in Files
            // We want to process all the files and log failures before we exit
            Site.Log.Add(LogEntryKind.Comment, $"Found {files.Length} files");
            Site.Log.Add(LogEntryKind.Comment, $"Start processing {files.Length} files");
            int count = 0;
            foreach (var file in files)
            {
                Site.Log.Add(LogEntryKind.Comment, $"Processing file '{file}'");
                statusList.Add(TestCompression(compressor, algorithm, file, $"{callingMethod}_{++count}"));
            }
            Site.Log.Add(LogEntryKind.Comment, $"Done processing {files.Length} files");

            foreach (var status in statusList)
            {
                Site.Log.Add(status.IsTestPassed ? LogEntryKind.CheckSucceeded : LogEntryKind.CheckFailed, status.TestResult);
            }

            Site.Assert.IsTrue(statusList.All(x => x.IsTestPassed), "All the tests MUST pass");
        }

        private TestStatus TestCompression(XcaCompressor compressor, COMPRESS_ALGORITHM algorithm, string inputFile, [CallerMemberName] string callingMethod = "")
        {
            var testOutputFile = GetUserDataOutputFilename(inputFile, true, true, callingMethod);
            var implOutputFile = GetUserDataOutputFilename(inputFile, true, false, callingMethod);

            // Read input data
            var testData = File.ReadAllBytes(inputFile);
            Site.Log.Add(LogEntryKind.Comment, $"Read {testData.Length} bytes");

            // Compress file
            var compressedData = compressor.Compress(testData);
            Site.Log.Add(LogEntryKind.Comment, $"Compressed {compressedData.Length} bytes");
            File.WriteAllBytes(testOutputFile, compressedData);

            // Invoke the SUT application
            _ = InvokeCompression(Path.GetFullPath(inputFile), Path.GetFullPath(implOutputFile), algorithm);

            // Compare test app result with local result
            byte[] testCompressedData = new byte[compressedData.Length];
            using (FileStream fileStream = new(implOutputFile, FileMode.Open))
            {
                int bytesRead = fileStream.Read(testCompressedData, 0, testCompressedData.Length);
                if(compressedData.Length != bytesRead)
                {
                    var response = $"File: '{inputFile}' - Expected {compressedData.Length} bytes, but actual bytes is {bytesRead}";
                    Site.Log.Add(LogEntryKind.TestFailed, response);
                    return new TestStatus
                    {
                        IsTestPassed = false,
                        TestName = callingMethod,
                        TestResult = response
                    };
                }
            }

            if (!compressedData.SequenceEqual(testCompressedData))
            {
                var response = $"File: '{inputFile}' - The contents of TestSuite compression don't match the contents of SUT compression";
                Site.Log.Add(LogEntryKind.TestFailed, response);
                return new TestStatus
                {
                    IsTestPassed = false,
                    TestName = callingMethod,
                    TestResult = response
                };
            }

            Site.Log.Add(LogEntryKind.TestPassed, $"File: '{inputFile}' - TestSuite compression matches SUT compression");
            return new TestStatus
            {
                IsTestPassed = true,
                TestName = callingMethod,
                TestResult = $"File: '{inputFile}' - TestSuite compression matches SUT compression"
            };
        }

        private class TestStatus
        {
            public bool IsTestPassed { get; set; }

            public string TestName { get; set; } = "";

            public string TestResult { get; set; } = "";
        }
    }
}
