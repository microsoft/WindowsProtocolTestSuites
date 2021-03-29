// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Common;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;

namespace Microsoft.Protocols.TestManager.PTMService.PTMService.Controllers
{
    public class TestSuite
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }

        public TestCaseInfo[] TestCases { get; set; }
    }

    public class Configuration
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public int TestSuiteId { get; set; }

        public string Description { get; set; }
    }

    public class Rule
    {
        public RuleType Type { get; set; }

        public string DisplayName { get; set; }

        public string Name { get; set; }

        public string[] Categories { get; set; }
    }

    public class RuleGroup
    {
        public string DisplayName { get; set; }

        public string Name { get; set; }

        public Rule[] Rules { get; set; }
    }

    public class PropertyGetItem
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string[] Choices { get; set; }

        public string Description { get; set; }
    }

    public class PropertyGetItemGroup
    {
        public string Name { get; set; }

        public PropertyGetItem[] Items { get; set; }
    }

    public class PropertySetItem
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }

    public class PropertySetItemGroup
    {
        public string Name { get; set; }

        public PropertySetItem[] Items { get; set; }
    }

    public class TestResultItem
    {
        public TestResultOverview Overview { get; set; }

        public TestCaseOverview[] Results { get; set; }
    }
}
