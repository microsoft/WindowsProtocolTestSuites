// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System;

namespace PTMService.Controllers
{
    public class TestCase
    {
        public string Name { get; set; }

        public string[] Categories { get; set; }
    }

    public class TestSuite
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }

        public TestCase[] TestCases { get; set; }
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

    public enum TestCaseState
    {
        Passed,
        Failed,
        Inconclusive,
    }

    public class TestCaseResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TestCaseState State { get; set; }

        public string Output { get; set; }
    }

    public enum TestResultState
    {
        Created,
        Running,
        Failed,
        Finished,
    }

    public class TestResult
    {
        public int Id { get; set; }

        public TestResultState Status { get; set; }

        public int ConfigurationId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? Total { get; set; }

        public int? NotRun { get; set; }

        public int? Passed { get; set; }

        public int? Failed { get; set; }

        public int? Inconclusive { get; set; }

        public TestCaseResult Results { get; set; }
    }
}
