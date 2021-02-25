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

    public enum RuleType
    {
        Remover,
        Selector,
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

    public class Property
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string[] Choices { get; set; }

        public string Description { get; set; }
    }

    public class PropertyGroup
    {
        public string Name { get; set; }

        public Property[] Items { get; set; }
    }

    public enum AdapterKind
    {
        Managed,
        PowerShell,
        Shell,
        Interactive,
    }

    public class Adapter
    {
        public string Name { get; set; }

        public string AdapterType { get; set; }

        public AdapterKind Kind { get; set; }

        public string ScriptDirectory { get; set; }
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
