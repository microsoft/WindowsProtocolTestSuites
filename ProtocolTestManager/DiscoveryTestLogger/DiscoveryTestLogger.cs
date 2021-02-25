// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Common;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Microsoft.Protocols.TestManager.DiscoveryTestLogger
{
    [ExtensionUri("logger://DiscoveryTestLogger")]
    [FriendlyName("Discovery")]
    public class DiscoveryTestLogger : ITestLogger
    {
        private string TestRunDirectory { get; set; }

        private IEnumerable<Common.TestCaseInfo> TestCaseInfos { get; set; }

        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
            if (!Directory.Exists(testRunDirectory))
            {
                Directory.CreateDirectory(testRunDirectory);
            }

            TestRunDirectory = testRunDirectory;

            events.DiscoveryStart += Events_DiscoveryStart;

            events.DiscoveredTests += Events_DiscoveredTests;

            events.DiscoveryComplete += Events_DiscoveryComplete;
        }

        private void Events_DiscoveryStart(object sender, DiscoveryStartEventArgs e)
        {
            TestCaseInfos = new List<Common.TestCaseInfo>();
        }

        private void Events_DiscoveredTests(object sender, DiscoveredTestsEventArgs e)
        {
            TestCaseInfos = TestCaseInfos.Concat(e.DiscoveredTestCases.Select(testCase =>
            {
                string name = testCase.DisplayName;

                string[] categories;

                try
                {
                    categories = testCase.GetPropertyValue<string[]>(TestProperty.Find("MSTestDiscoverer.TestCategory"), new string[] { });
                }
                catch
                {
                    categories = new string[] { };
                }

                string description;

                try
                {
                    description = testCase.GetPropertyValue<string>(TestProperty.Find("Description"), String.Empty);
                }
                catch
                {
                    description = String.Empty;
                }

                var testcaseToolTipBuilder = new StringBuilder();

                testcaseToolTipBuilder.Append(name);

                if (categories.Length > 0)
                {
                    testcaseToolTipBuilder.Append(Environment.NewLine + "Category:");

                    foreach (var category in categories)
                    {
                        testcaseToolTipBuilder.Append(Environment.NewLine + "  " + category);
                    }
                }
                if (!string.IsNullOrEmpty(description))
                {
                    testcaseToolTipBuilder.Append(Environment.NewLine + "Description:");

                    testcaseToolTipBuilder.Append(Environment.NewLine + "  " + description);
                }

                return new TestCaseInfo
                {
                    Category = categories,
                    Description = description,
                    FullName = testCase.FullyQualifiedName,
                    Name = testCase.DisplayName,
                    ToolTipOnUI = testcaseToolTipBuilder.ToString(),
                };
            }));
        }

        private void Events_DiscoveryComplete(object sender, DiscoveryCompleteEventArgs e)
        {
            var content = JsonSerializer.Serialize(TestCaseInfos.ToArray());

            string path = Path.Combine(TestRunDirectory, "TestCaseInfo.json");

            File.WriteAllText(path, content);
        }
    }
}
