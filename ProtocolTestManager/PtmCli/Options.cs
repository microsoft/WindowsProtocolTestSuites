// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using CommandLine;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.CLI
{
    class Options
    {
        [Option('p', "profile",
            Required = true,
            HelpText = "Specifies the path of the test profile to run.")]
        public string Profile { get; set; }
        [Option('t', "testsuite",
            Required = true,
            HelpText = "Specifies the path of the testsuite to run.")]
        public string TestSuite { get; set; }

        [Option('s', "selected",
            Default = false,
            HelpText = "When specified, only the selected test cases will be executed.\nOtherwise, all the test cases in the profile will be executed.")]
        public bool SelectedOnly { get; set; }

        [Option("filter",
            Required = false,
            HelpText = "Specifies the filter expression of test cases to run.\nThis parameter overrides the test cases in profile.")]
        public string FilterExpression { get; set; }

        [Option("config",
            Required = false,
            HelpText = "Specifies the configuration items which will override the values in profile.\n" + "" +
                "Each configuration should be in format {property_name}={property_value}, and multiple items should be separated by whitespace.\n" +
                "For example: --config \"RDP.ServerPort=3389\" \"RDP.Security.Protocol=TLS\"")]
        public IEnumerable<string> Configuration { get; set; }

        [Option('r', "report",
            Required = false,
            HelpText = "Specifies the result file which will be written to.\nIf not specified, test results will be written to stdout.")]
        public string ReportFile { get; set; }

        [Option('f', "format",
            Required = false,
            Default = ReportFormat.Plain,
            HelpText = "Specifies the report format.\nValid values are: plain, json, xunit.")]
        public ReportFormat ReportFormat { get; set; }

        [Option("outcome",
            Separator = ',',
            Required = false,
            Default = new[] {
                CLI.Outcome.Pass,
                CLI.Outcome.Fail,
                CLI.Outcome.Inconclusive,
            },
            HelpText = "Specifies the outcome of the test cases to be included in the report file.\nValue should be separated by comma or space.\nValid values are: pass, fail, inconclusive.")]
        public IEnumerable<Outcome> Outcome { get; set; }

        [Option('d', "debug",
            Required = false,
            Default = false,
            HelpText = "Enable debugging of PTMCli.")]
        public bool EnableDebugging { get; set; }

        public override string ToString()
        {
            return "Input Options:" + Environment.NewLine +
                $"Profile: {Profile}; TestSuite: {TestSuite}; SelectedOnly: {SelectedOnly}; " +
                $"{nameof(FilterExpression)}: {FilterExpression} " +
                $"{nameof(Configuration)}: {String.Join(" ", Configuration)}; " +
                $"ReportFile: {ReportFile}; " +
                $"ReportFormat: {ReportFormat}; Outcome: {string.Join(", ", Outcome)}; Debug: {EnableDebugging}";
        }
    }

    public enum ReportFormat
    {
        Plain,
        Json,
        XUnit,
    }

    public enum Outcome
    {
        Pass,
        Fail,
        Inconclusive,
    }
}
