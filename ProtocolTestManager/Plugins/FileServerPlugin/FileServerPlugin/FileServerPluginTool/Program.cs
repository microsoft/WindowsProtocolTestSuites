// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.FileServerPluginTool
{
    class Program
    {
        static void Main(string[] args)
        {
            AppConfig config = AppConfig.LoadConfig();
            TestSuite testSuite = new TestSuite();
            testSuite.LoadFrom(config.TestSuiteAssembly);
            testSuite.AppendCategoryForFileServerCases();
        }
    }
}
