// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.Protocols.TestManager.Kernel
{
    public sealed class FileServerExecutionPlanBuilder
    {
        public const int DefaultMaxDegreeOfParallelism = 5;
        public const int DefaultSmb2ModelShardCount = 4;

        private const string Smb2Assembly = "MS-SMB2_ServerTestSuite.dll";
        private const string Smb2ModelAssembly = "MS-SMB2Model_ServerTestSuite.dll";
        private const string FsaAssembly = "MS-FSA_ServerTestSuite.dll";
        private const string FsaModelAssembly = "MS-FSAModel_ServerTestSuite.dll";

        private static readonly HashSet<string> AppInstanceIdTests = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase)
        {
            "AppInstanceId_DirectoryLeasing_NoLeaseInReOpen",
            "AppInstanceId_Encryption",
            "AppInstanceId_FileLeasing_NoLeaseInReOpen",
            "AppInstanceId_Negative_EncryptionInInitialOpen_NoEncryptionInReOpen",
            "AppInstanceId_Negative_NoEncryptionInInitialOpen_EncryptionInReOpen",
        };

        private readonly int maxDegreeOfParallelism;
        private readonly int smb2ModelShardCount;

        public FileServerExecutionPlanBuilder(
            int maxDegreeOfParallelism = DefaultMaxDegreeOfParallelism,
            int smb2ModelShardCount = DefaultSmb2ModelShardCount)
        {
            this.maxDegreeOfParallelism = maxDegreeOfParallelism > 0
                ? maxDegreeOfParallelism
                : throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParallelism));
            this.smb2ModelShardCount = smb2ModelShardCount > 0
                ? smb2ModelShardCount
                : throw new ArgumentOutOfRangeException(nameof(smb2ModelShardCount));
        }

        public TestExecutionPlan Build(IEnumerable<TestCase> selectedTestCases, bool parallelEnabled)
        {
            var tests = selectedTestCases?.ToList()
                ?? throw new ArgumentNullException(nameof(selectedTestCases));

            if (tests.Count == 0)
            {
                return new TestExecutionPlan(Array.Empty<TestExecutionStage>(), maxDegreeOfParallelism);
            }

            ValidateAssemblies(tests);

            if (!parallelEnabled)
            {
                var sequentialUnit = new TestExecutionUnit(
                    "fileserver-sequential",
                    tests.Select(test => test.Assembly),
                    tests);

                return new TestExecutionPlan(
                    new[] { new TestExecutionStage("FileServer sequential run", new[] { sequentialUnit }) },
                    1);
            }

            var stages = new List<TestExecutionStage>();
            var remaining = new List<TestCase>(tests);

            AddSmb2ModelStage(stages, remaining);
            AddAssemblyStage(
                stages,
                remaining,
                Smb2Assembly,
                "SMB2",
                test => !IsAppInstanceIdTest(test));
            AddAppInstanceIdStage(stages, remaining);
            AddFsaStage(stages, remaining);
            AddRemainingAssembliesStage(stages, remaining);

            return new TestExecutionPlan(stages, maxDegreeOfParallelism);
        }

        private void AddSmb2ModelStage(List<TestExecutionStage> stages, List<TestCase> remaining)
        {
            var modelTests = TakeTests(
                remaining,
                test => IsAssembly(test, Smb2ModelAssembly) && !IsAppInstanceIdTest(test));

            if (modelTests.Count == 0)
            {
                return;
            }

            int shardCount = Math.Min(smb2ModelShardCount, modelTests.Count);
            var shards = Enumerable.Range(0, shardCount)
                .Select(_ => new List<TestCase>())
                .ToArray();

            for (int index = 0; index < modelTests.Count; index++)
            {
                shards[index % shardCount].Add(modelTests[index]);
            }

            var units = shards.Select((shard, index) => new TestExecutionUnit(
                $"smb2model-shard-{index}",
                new[] { shard[0].Assembly },
                shard));

            stages.Add(new TestExecutionStage("SMB2Model shards", units));
        }

        private static void AddAssemblyStage(
            List<TestExecutionStage> stages,
            List<TestCase> remaining,
            string assemblyName,
            string stageName,
            Func<TestCase, bool> additionalPredicate = null)
        {
            var tests = TakeTests(
                remaining,
                test => IsAssembly(test, assemblyName)
                        && (additionalPredicate == null || additionalPredicate(test)));
            if (tests.Count == 0)
            {
                return;
            }

            stages.Add(new TestExecutionStage(
                stageName,
                new[]
                {
                    new TestExecutionUnit(
                        Path.GetFileNameWithoutExtension(assemblyName).ToLowerInvariant(),
                        new[] { tests[0].Assembly },
                        tests),
                }));
        }

        private static void AddAppInstanceIdStage(
            List<TestExecutionStage> stages,
            List<TestCase> remaining)
        {
            var tests = TakeTests(remaining, IsAppInstanceIdTest);
            if (tests.Count == 0)
            {
                return;
            }

            var units = tests
                .GroupBy(test => test.Assembly, StringComparer.OrdinalIgnoreCase)
                .Select((group, index) => new TestExecutionUnit(
                    $"appinstanceid-{index}",
                    new[] { group.Key },
                    group));

            stages.Add(new TestExecutionStage("AppInstanceId", units));
        }

        private static void AddFsaStage(
            List<TestExecutionStage> stages,
            List<TestCase> remaining)
        {
            var tests = TakeTests(
                remaining,
                test => IsAssembly(test, FsaAssembly) || IsAssembly(test, FsaModelAssembly));

            if (tests.Count == 0)
            {
                return;
            }

            var units = tests
                .GroupBy(test => test.Assembly, StringComparer.OrdinalIgnoreCase)
                .Select(group => new TestExecutionUnit(
                    Path.GetFileNameWithoutExtension(group.Key).ToLowerInvariant(),
                    new[] { group.Key },
                    group));

            stages.Add(new TestExecutionStage("FSA and FSAModel", units));
        }

        private static void AddRemainingAssembliesStage(
            List<TestExecutionStage> stages,
            List<TestCase> remaining)
        {
            if (remaining.Count == 0)
            {
                return;
            }

            var units = remaining
                .GroupBy(test => test.Assembly, StringComparer.OrdinalIgnoreCase)
                .OrderBy(group => group.Key, StringComparer.OrdinalIgnoreCase)
                .Select(group => new TestExecutionUnit(
                    Path.GetFileNameWithoutExtension(group.Key).ToLowerInvariant(),
                    new[] { group.Key },
                    group));

            stages.Add(new TestExecutionStage("Remaining FileServer assemblies", units));
            remaining.Clear();
        }

        private static List<TestCase> TakeTests(
            List<TestCase> remaining,
            Func<TestCase, bool> predicate)
        {
            var selected = remaining.Where(predicate).ToList();
            remaining.RemoveAll(test => selected.Contains(test));
            return selected;
        }

        private static bool IsAppInstanceIdTest(TestCase test)
        {
            return test.Category.Any(category =>
                       string.Equals(category, "AppInstanceId", StringComparison.OrdinalIgnoreCase))
                   || AppInstanceIdTests.Contains(test.Name);
        }

        private static bool IsAssembly(TestCase test, string assemblyName)
        {
            return string.Equals(
                Path.GetFileName(test.Assembly),
                assemblyName,
                StringComparison.OrdinalIgnoreCase);
        }

        private static void ValidateAssemblies(IEnumerable<TestCase> tests)
        {
            var missingAssembly = tests.FirstOrDefault(test => string.IsNullOrWhiteSpace(test.Assembly));
            if (missingAssembly != null)
            {
                throw new ArgumentException(
                    $"Test case '{missingAssembly.FullName}' does not identify its source assembly.",
                    nameof(tests));
            }
        }
    }
}
