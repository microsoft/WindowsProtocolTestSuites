// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestManager.TestCategoryGenerationTool
{
    class Program
    {
        static public void usage()
        {
            System.Console.WriteLine("\nTestCategoryGenerationTool.exe  TestSuiteName");
            System.Console.WriteLine("-------------------------------------------------");
            System.Console.WriteLine("Please select TestSuiteName below:");
            System.Console.WriteLine("  FileServer");
            System.Console.WriteLine("  Kerberos");
            System.Console.WriteLine("  MS-AZOD");
            System.Console.WriteLine("  ADFamily");
            System.Console.WriteLine("  MS-ADOD");
            System.Console.WriteLine("  RDP-Client");
            System.Console.WriteLine("  RDP-Server");
            System.Console.WriteLine("  MS-ADFSPIP");
            System.Console.WriteLine("  BranchCache");
            System.Console.WriteLine("-------------------------------------------------");
            System.Console.WriteLine("For Example, if we want to generate TestCategory.xml for FileServer:");
            System.Console.WriteLine("  TestCategoryGenerationTool.exe FileServer");
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                usage();
                Environment.Exit(-1);
            }
            AppConfig config = AppConfig.LoadConfig(args[0]);
            TestSuite testSuite = new TestSuite();
            try
            {
                testSuite.LoadFrom(config.TestSuiteAssembly);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            if (config.TargetTestSuiteType == AppConfig.TestSuiteType.FILE_SERVER)
            {
                testSuite.AppendCategoryForFileServerCases();
            }
        }
    }
}
