// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestManager.Kernel
{
    public sealed class TestExecutionPlan
    {
        public TestExecutionPlan(IEnumerable<TestExecutionStage> stages, int maxDegreeOfParallelism)
        {
            Stages = stages?.ToList() ?? throw new ArgumentNullException(nameof(stages));
            MaxDegreeOfParallelism = maxDegreeOfParallelism > 0
                ? maxDegreeOfParallelism
                : throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParallelism));
        }

        public IReadOnlyList<TestExecutionStage> Stages { get; }

        public int MaxDegreeOfParallelism { get; }
    }

    public sealed class TestExecutionStage
    {
        public TestExecutionStage(string name, IEnumerable<TestExecutionUnit> units)
        {
            Name = string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentException("A stage name is required.", nameof(name))
                : name;
            Units = units?.ToList() ?? throw new ArgumentNullException(nameof(units));

            if (Units.Count == 0)
            {
                throw new ArgumentException("A stage must contain at least one execution unit.", nameof(units));
            }
        }

        public string Name { get; }

        public IReadOnlyList<TestExecutionUnit> Units { get; }
    }

    public sealed class TestExecutionUnit
    {
        public TestExecutionUnit(string id, IEnumerable<string> assemblies, IEnumerable<TestCase> testCases)
        {
            Id = string.IsNullOrWhiteSpace(id)
                ? throw new ArgumentException("An execution unit identifier is required.", nameof(id))
                : id;
            Assemblies = assemblies?.Distinct(StringComparer.OrdinalIgnoreCase).ToList()
                ?? throw new ArgumentNullException(nameof(assemblies));
            TestCases = testCases?.ToList() ?? throw new ArgumentNullException(nameof(testCases));

            if (Assemblies.Count == 0)
            {
                throw new ArgumentException("An execution unit must contain at least one assembly.", nameof(assemblies));
            }

            if (TestCases.Count == 0)
            {
                throw new ArgumentException("An execution unit must contain at least one test case.", nameof(testCases));
            }
        }

        public string Id { get; }

        public IReadOnlyList<string> Assemblies { get; }

        public IReadOnlyList<TestCase> TestCases { get; }
    }
}
