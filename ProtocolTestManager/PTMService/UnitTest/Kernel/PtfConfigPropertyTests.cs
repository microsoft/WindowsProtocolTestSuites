// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Protocols.TestManager.PTMService.UnitTest.Kernel
{
    [TestClass]
    public class PtfConfigPropertyTests
    {
        [TestMethod]
        public void GetPropertyNodeByName_FileServerParallelExecutionProperty_UsesCommonGroup()
        {
            string directory = Path.Combine(
                Path.GetTempPath(),
                $"PtfConfigPropertyTests-{Guid.NewGuid():N}");
            Directory.CreateDirectory(directory);
            string configPath = Path.Combine(directory, "CommonTestSuite.ptfconfig");

            try
            {
                File.WriteAllText(
                    configPath,
                    """
                    <?xml version="1.0" encoding="utf-8"?>
                    <TestSite xmlns="http://schemas.microsoft.com/windows/ProtocolsTest/2007/07/TestConfig">
                      <Properties>
                        <Group name="Common">
                          <Property name="PTF.LogProfileParserPatch.Enabled" value="true">
                            <Description>Enable staged parallel execution.</Description>
                            <Type>Bool</Type>
                            <Choice>true,false</Choice>
                          </Property>
                        </Group>
                      </Properties>
                      <Adapters/>
                      <TestLog/>
                    </TestSite>
                    """);

                var config = new PtfConfig(new List<string> { configPath });

                var property = config.GetPropertyNodeByName(
                    "Common.PTF.LogProfileParserPatch.Enabled");

                Assert.AreEqual("true", property?.Value);
                Assert.AreEqual("Enable staged parallel execution.", property?.Description);
                CollectionAssert.AreEqual(
                    new[] { "true", "false" },
                    property?.ChoiceItems);
                Assert.IsNull(
                    config.GetPropertyNodeByName(
                        "PTF.LogProfileParserPatch.Enabled"));
            }
            finally
            {
                Directory.Delete(directory, recursive: true);
            }
        }
    }
}
