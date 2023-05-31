// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Compression.Xpress;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;

namespace Microsoft.Protocols.TestSuites.MS_XCA.Decompression
{
    [TestClass]
    public class UserDataDecompression : XcaTestClassBase
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
        /// This tests decompression of all the files in a User specified folder using the LZ77 algorithm
        /// </summary>
        [TestMethod]
        [TestCategory("Decompression")]
        [TestCategory("UserData")]
        [TestCategory("LZ77")]
        public void Decompression_LZ77_UserData()
        {
            TestDecompression(new PlainLZ77Decompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77);
        }

        /// <summary>
        /// This tests decompression of all the files in a User specified folder using the LZ77 + Huffman algorithm
        /// </summary>
        [TestMethod]
        [TestCategory("Decompression")]
        [TestCategory("UserData")]
        [TestCategory("LZ77+Huffman")]
        public void Decompression_LZ77Huffman_UserData()
        {
            TestDecompression(new LZ77HuffmanDecompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZ77_HUFF);
        }

        /// <summary>
        /// This tests decompression of all the files in a User specified folder using the LZNT1 algorithm
        /// </summary>
        [TestMethod]
        [TestCategory("Decompression")]
        [TestCategory("UserData")]
        [TestCategory("LZNT1")]
        public void Decompression_LZNT1_UserData()
        {
            TestDecompression(new LZNT1Decompressor(), COMPRESS_ALGORITHM.ALGORITHM_LZNT1);
        }

        /// <summary>
        /// Perform the decompression on all the files in the User specified folder
        /// </summary>
        /// <param name="decompressor"></param>
        private void TestDecompression(XcaDecompressor decompressor, COMPRESS_ALGORITHM algorithm, [CallerMemberName] string callingMethod = "")
        {
            List<TestStatus> statusList = new();

            // 1. Get all the Files in the UserData folder
            var files = GetUserDataInputFiles(false, algorithm);

            // 2. Call the Decompress function for each file in Files
            // We want to process all the files and log failures before we exit
            Site.Log.Add(LogEntryKind.Comment, $"Found {files.Length} files");
            Site.Log.Add(LogEntryKind.Comment, $"Start processing {files.Length} files");
            foreach (var file in files)
            {
                Site.Log.Add(LogEntryKind.Comment, $"Processing file '{file}'");
                statusList.Add(TestDecompression(decompressor, algorithm, file, callingMethod));
            }
            Site.Log.Add(LogEntryKind.Comment, $"Done processing {files.Length} files");

            foreach (var status in statusList)
            {
                Site.Log.Add(status.IsTestPassed ? LogEntryKind.CheckSucceeded : LogEntryKind.CheckFailed, status.TestResult);
            }

            Site.Assert.IsTrue(statusList.All(x => x.IsTestPassed), "All the tests MUST pass");
        }

        private TestStatus TestDecompression(XcaDecompressor decompressor, COMPRESS_ALGORITHM algorithm, string inputFile, [CallerMemberName] string callingMethod = "")
        {
            var testOutputFile = GetUserDataOutputFilename(inputFile, false, true, callingMethod);
            var implOutputFile = GetUserDataOutputFilename(inputFile, false, false, callingMethod);

            // Read input data
            var testData = File.ReadAllBytes(inputFile);
            Site.Log.Add(LogEntryKind.Comment, $"Read {testData.Length} bytes");

            // Decompress file
            var decompressedData = decompressor.Decompress(testData);
            Site.Log.Add(LogEntryKind.Comment, $"Decompressed {decompressedData.Length} bytes");
            File.WriteAllBytes(testOutputFile, decompressedData);

            // Invoke the SUT application
            _ = InvokeDecompression(Path.GetFullPath(inputFile), Path.GetFullPath(implOutputFile), algorithm);

            // Compare test app result with local result
            byte[] testDecompressedData = new byte[decompressedData.Length];
            using (FileStream fileStream = new(implOutputFile, FileMode.Open))
            {
                int bytesRead = fileStream.Read(testDecompressedData, 0, testDecompressedData.Length);
                if (decompressedData.Length != bytesRead)
                {
                    var response = $"File: '{inputFile}' - Expected {decompressedData.Length} bytes, but actual bytes is {bytesRead}";
                    Site.Log.Add(LogEntryKind.TestFailed, response);
                    return new TestStatus
                    {
                        IsTestPassed = false,
                        TestName = callingMethod,
                        TestResult = response
                    };
                }
            }

            if (!decompressedData.SequenceEqual(testDecompressedData))
            {
                var response = $"File: '{inputFile}' - The contents of TestSuite decompression don't match the contents of SUT decompression";
                Site.Log.Add(LogEntryKind.TestFailed, response);
                return new TestStatus
                {
                    IsTestPassed = false,
                    TestName = callingMethod,
                    TestResult = response
                };
            }

            Site.Log.Add(LogEntryKind.TestPassed, $"File: '{inputFile}' - TestSuite decompression matches SUT decompression");
            return new TestStatus
            {
                IsTestPassed = true,
                TestName = callingMethod,
                TestResult = $"File: '{inputFile}' - TestSuite decompression matches SUT decompression"
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
