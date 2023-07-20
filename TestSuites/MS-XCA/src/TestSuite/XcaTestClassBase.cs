// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestSuites.MS_XCA
{
    public abstract class XcaTestClassBase : TestClassBase
    {
        #region Properties

        protected XcaTestConfig testConfig;

        protected static readonly string defaultLZ77CompressionInput01 = "../StaticData/Uncompressed_a_z.data";

        protected static readonly string defaultLZ77CompressionInput02 = "../StaticData/Uncompressed_abc_100.data";

        protected static readonly string defaultLZ77CompressionInputLarge01 = "../StaticData/Uncompressed_a_z_large.data";

        protected static readonly string defaultLZ77CompressionInputLarge02 = "../StaticData/Uncompressed_abc_100_large.data";

        protected static readonly string plainLZ77DecompressionInput01 = "../StaticData/LZ77_Compressed_a_z.data";

        protected static readonly string plainLZ77DecompressionInput02 = "../StaticData/LZ77_Compressed_abc_100.data";

        protected static readonly string plainLZ77DecompressionInputLarge01 = "../StaticData/LZ77_Compressed_a_z_large.data";

        protected static readonly string plainLZ77DecompressionInputLarge02 = "../StaticData/LZ77_Compressed_abc_100_large.data";

        protected static readonly string lz77HuffmanDecompressionInput01 = "../StaticData/LZ77Huffman_Compressed_a_z.data";

        protected static readonly string lz77HuffmanDecompressionInput02 = "../StaticData/LZ77Huffman_Compressed_abc_100.data";

        protected static readonly string lz77HuffmanDecompressionInputLarge01 = "../StaticData/LZ77Huffman_Compressed_a_z_large.data";

        protected static readonly string lz77HuffmanDecompressionInputLarge02 = "../StaticData/LZ77Huffman_Compressed_abc_100_large.data";

        protected static readonly string lznt1CompressionInput = "../StaticData/LZNT1_Uncompressed.data";

        protected static readonly string lznt1CompressionInputLarge = "../StaticData/LZNT1_Uncompressed_large.data";

        protected static readonly string lznt1DecompressionInput = "../StaticData/LZNT1_Compressed.data";

        protected static readonly string lznt1DecompressionInputLarge = "../StaticData/LZNT1_Compressed_large.data";

        public static readonly string InputFileParam = "InputFile";

        public static readonly string OutputFileParam = "OutputFile";

        public enum COMPRESS_ALGORITHM
        {
            ALGORITHM_LZ77 = 1,
            ALGORITHM_LZ77_HUFF = 2,
            ALGORITHM_LZNT1 = 3
        }

        #endregion

        protected string CurrentTestCaseName
        {
            get
            {
                string fullName = (string)BaseTestSite.TestProperties["CurrentTestCaseName"];
                return fullName.Split('.').LastOrDefault();
            }
        }

        protected Dictionary<string, Assembly> TestcaseAssemblies;

        protected override void TestInitialize()
        {
            base.TestInitialize();
            this.testConfig = new XcaTestConfig(Site);
            LogTestCaseDescription();
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }

        protected string[] GetUserDataInputFiles(bool isCompression, COMPRESS_ALGORITHM algorithm)
        {
            string property = "";
            switch (algorithm)
            {
                case COMPRESS_ALGORITHM.ALGORITHM_LZ77:
                    property = isCompression ? "UserDataCompressionInputFolder" : "UserDataDecompressionInputFolderLZ77";
                    break;
                case COMPRESS_ALGORITHM.ALGORITHM_LZ77_HUFF:
                    property = isCompression ? "UserDataCompressionInputFolder" : "UserDataDecompressionInputFolderLZ77Huffman";
                    break;
                case COMPRESS_ALGORITHM.ALGORITHM_LZNT1:
                    property = isCompression ? "UserDataCompressionInputFolder" : "UserDataDecompressionInputFolderLZNT1";
                    break;
            }
            return Directory.GetFiles(testConfig.GetProperty(property, false));
        }

        protected string GetUserDataOutputFilename(string inputFile,
            bool isCompressionTest,
            bool isTestSuiteOutput,
            [CallerMemberName] string callingMethod = "")
        {
            var property = isCompressionTest ? "UserDataCompressionOutputFolder" : "UserDataDecompressionOutputFolder";
            var baseDirectory = testConfig.GetProperty(property, false);
            return GetOutputFilename(baseDirectory, inputFile, isCompressionTest, isTestSuiteOutput, callingMethod);
        }

        protected string GetStaticDataOutputFilename(string inputFile,
        bool isCompressionTest,
        bool isTestSuiteOutput,
        [CallerMemberName] string callingMethod = "")
        {
            var baseDirectory = testConfig.GetProperty("StaticDataOutputFolder", false);
            return GetOutputFilename(baseDirectory, inputFile, isCompressionTest, isTestSuiteOutput, callingMethod);
        }

        protected string GetOutputFilename(string baseDirectory, string inputFile,
            bool isCompressionTest,
            bool isTestSuiteOutput,
            [CallerMemberName] string callingMethod = "")
        {
            var compressionDirectory = isCompressionTest ? "CompressionTestResults" : "DecompressionTestResults";
            var testExeDirectory = isTestSuiteOutput ? "TestSuite" : "TestImplementation";
            if (string.IsNullOrEmpty(baseDirectory))
                baseDirectory = Path.GetDirectoryName(inputFile);

            if (!Directory.Exists($"{baseDirectory}/{compressionDirectory}"))
            {
                Directory.CreateDirectory($"{baseDirectory}/{compressionDirectory}");
            }

            if (!Directory.Exists($"{baseDirectory}/{compressionDirectory}/{testExeDirectory}"))
            {
                Directory.CreateDirectory($"{baseDirectory}/{compressionDirectory}/{testExeDirectory}");
            }

            var outputDirectory = $"{baseDirectory}/{compressionDirectory}/{testExeDirectory}";

            var testOutputFile = $"{outputDirectory}/{callingMethod}_{Path.GetFileName(inputFile)}";
            Site.Log.Add(LogEntryKind.Comment, $"**** The path to the testOutputFile ==> {new DirectoryInfo(Path.GetDirectoryName(testOutputFile)).FullName}");

            return testOutputFile;
        }

        /// <summary>
        /// Add test case description to log
        /// </summary>
        /// <param name="testcaseAssembly">Assembly where the test case existed.</param>
        protected void LogTestCaseDescription()
        {
            var testcase = (string)BaseTestSite.TestProperties["CurrentTestCaseName"];
            int lastDotIndex = testcase.LastIndexOf('.');
            string typeName = testcase.Substring(0, lastDotIndex);
            string methodName = testcase.Substring(lastDotIndex + 1);

            Assembly testcaseAssembly = Assembly.GetExecutingAssembly();
            var type = testcaseAssembly.GetType(typeName);
            if (type == null)
            {
                BaseTestSite.Assert.Fail(String.Format("Test case type name {0} does not exist in test case assembly {1}.", typeName, testcaseAssembly.FullName));
            }
            else
            {
                var method = type.GetMethod(methodName);
                var attributes = method.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes == null)
                {
                    BaseTestSite.Assert.Fail("No description is provided for this case.");
                }
                else
                {
                    foreach (DescriptionAttribute attribute in attributes)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Comment, attribute.Description);
                    }
                }
            }
        }

        #region Invoke Local Process

        protected string GetSutExeProperty(string propertyName)
        {
            return testConfig.GetProperty("SUT", propertyName);
        }

        protected int InvokeCompression(string inputFile, string outputFile, COMPRESS_ALGORITHM algorithm)
        {
            var command = algorithm switch
            {
                COMPRESS_ALGORITHM.ALGORITHM_LZ77 => GetSutExeProperty("PLAIN_LZ77_COMPRESSION_COMMAND"),
                COMPRESS_ALGORITHM.ALGORITHM_LZ77_HUFF => GetSutExeProperty("LZ77_HUFFMAN_COMPRESSION_COMMAND"),
                COMPRESS_ALGORITHM.ALGORITHM_LZNT1 => GetSutExeProperty("LZNT1_COMPRESSION_COMMAND"),
                _ => throw new NotImplementedException()
            };
            var arguments = algorithm switch
            {
                COMPRESS_ALGORITHM.ALGORITHM_LZ77 => GetSutExeProperty("PLAIN_LZ77_COMPRESSION_ARGUMENTS"),
                COMPRESS_ALGORITHM.ALGORITHM_LZ77_HUFF => GetSutExeProperty("LZ77_HUFFMAN_COMPRESSION_ARGUMENTS"),
                COMPRESS_ALGORITHM.ALGORITHM_LZNT1 => GetSutExeProperty("LZNT1_COMPRESSION_ARGUMENTS"),
                _ => throw new NotImplementedException()
            };
            arguments = arguments.Replace($"{{{{{InputFileParam}}}}}", inputFile).Replace($"{{{{{OutputFileParam}}}}}", outputFile);

            return InvokeLocalProcess(command, arguments);
        }

        protected int InvokeDecompression(string inputFile, string outputFile, COMPRESS_ALGORITHM algorithm)
        {
            var command = algorithm switch
            {
                COMPRESS_ALGORITHM.ALGORITHM_LZ77 => GetSutExeProperty("PLAIN_LZ77_DECOMPRESSION_COMMAND"),
                COMPRESS_ALGORITHM.ALGORITHM_LZ77_HUFF => GetSutExeProperty("LZ77_HUFFMAN_DECOMPRESSION_COMMAND"),
                COMPRESS_ALGORITHM.ALGORITHM_LZNT1 => GetSutExeProperty("LZNT1_DECOMPRESSION_COMMAND"),
                _ => throw new NotImplementedException()
            };
            var arguments = algorithm switch
            {
                COMPRESS_ALGORITHM.ALGORITHM_LZ77 => GetSutExeProperty("PLAIN_LZ77_DECOMPRESSION_ARGUMENTS"),
                COMPRESS_ALGORITHM.ALGORITHM_LZ77_HUFF => GetSutExeProperty("LZ77_HUFFMAN_DECOMPRESSION_ARGUMENTS"),
                COMPRESS_ALGORITHM.ALGORITHM_LZNT1 => GetSutExeProperty("LZNT1_DECOMPRESSION_ARGUMENTS"),
                _ => throw new NotImplementedException()
            };
            arguments = arguments.Replace($"{{{{{InputFileParam}}}}}", inputFile).Replace($"{{{{{OutputFileParam}}}}}", outputFile);

            return InvokeLocalProcess(command, arguments);
        }

        protected int InvokeLocalProcess(string command, string arguments = "")
        {
            Site.Log.Add(LogEntryKind.Comment, $"command ==> {command}");
            Site.Log.Add(LogEntryKind.Comment, $"arguments ==> {arguments}");

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new NotImplementedException($"Not Implemented in OS {RuntimeInformation.OSDescription}");
            }

            ProcessStartInfo startInfo = new()
            {
                WorkingDirectory = GetSutExeProperty("WorkingDirectory"),
                FileName = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = arguments
            };
            Process localProcess = new()
            {
                StartInfo = startInfo
            };
            localProcess.Start();

            var output = localProcess.StandardOutput.ReadToEnd();
            Site.Log.Add(LogEntryKind.Comment, "--------------------------");
            Site.Log.Add(LogEntryKind.Comment, output);
            Site.Log.Add(LogEntryKind.Comment, "--------------------------");

            localProcess.Close();

            return 1;
        }

        #endregion
    }
}
