// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.PTMService.UnitTest.Kernel
{
    [TestClass]
    public class TestExecutionPlanRunnerTests
    {
        [TestMethod]
        public async Task RunAsync_StagesExecuteSequentially()
        {
            var firstStageRelease = new TaskCompletionSource<bool>(
                TaskCreationOptions.RunContinuationsAsynchronously);
            bool secondStageStarted = false;
            var plan = CreatePlan(
                2,
                CreateStage("first", "first-1", "first-2"),
                CreateStage("second", "second-1"));

            var runTask = new TestExecutionPlanRunner().RunAsync(
                plan,
                async (unit, _) =>
                {
                    if (unit.Id.StartsWith("first", StringComparison.Ordinal))
                    {
                        await firstStageRelease.Task;
                    }
                    else
                    {
                        secondStageStarted = true;
                    }
                },
                CancellationToken.None);

            await Task.Delay(50);
            Assert.IsFalse(secondStageStarted);

            firstStageRelease.SetResult(true);
            await runTask;

            Assert.IsTrue(secondStageStarted);
        }

        [TestMethod]
        public async Task RunAsync_RespectsMaximumDegreeOfParallelism()
        {
            int active = 0;
            int maximumActive = 0;
            var plan = CreatePlan(
                2,
                CreateStage("parallel", "unit-1", "unit-2", "unit-3", "unit-4", "unit-5"));

            await new TestExecutionPlanRunner().RunAsync(
                plan,
                async (_, _) =>
                {
                    int current = Interlocked.Increment(ref active);
                    UpdateMaximum(ref maximumActive, current);
                    await Task.Delay(25);
                    Interlocked.Decrement(ref active);
                },
                CancellationToken.None);

            Assert.AreEqual(2, maximumActive);
        }

        [TestMethod]
        public async Task RunAsync_CollectsFailureAndContinuesRemainingStages()
        {
            var executed = new List<string>();
            var plan = CreatePlan(
                2,
                CreateStage("first", "failure", "success"),
                CreateStage("second", "later"));

            var result = await new TestExecutionPlanRunner().RunAsync(
                plan,
                (unit, _) =>
                {
                    lock (executed)
                    {
                        executed.Add(unit.Id);
                    }

                    return unit.Id == "failure"
                        ? Task.FromException(new InvalidOperationException("Expected failure."))
                        : Task.CompletedTask;
                },
                CancellationToken.None);

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(1, result.Failures.Count);
            Assert.AreEqual("failure", result.Failures[0].UnitId);
            CollectionAssert.AreEquivalent(
                new[] { "failure", "success", "later" },
                executed);
        }

        [TestMethod]
        public async Task RunAsync_CancellationPreventsLaterStages()
        {
            using var cancellation = new CancellationTokenSource();
            bool secondStageStarted = false;
            var plan = CreatePlan(
                1,
                CreateStage("first", "cancel"),
                CreateStage("second", "later"));

            await Assert.ThrowsExceptionAsync<OperationCanceledException>(
                () => new TestExecutionPlanRunner().RunAsync(
                    plan,
                    (unit, token) =>
                    {
                        if (unit.Id == "cancel")
                        {
                            cancellation.Cancel();
                            token.ThrowIfCancellationRequested();
                        }

                        secondStageStarted = true;
                        return Task.CompletedTask;
                    },
                    cancellation.Token));

            Assert.IsFalse(secondStageStarted);
        }

        private static TestExecutionPlan CreatePlan(
            int maxDegreeOfParallelism,
            params TestExecutionStage[] stages)
        {
            return new TestExecutionPlan(stages, maxDegreeOfParallelism);
        }

        private static TestExecutionStage CreateStage(string name, params string[] unitIds)
        {
            return new TestExecutionStage(
                name,
                unitIds.Select(unitId => new TestExecutionUnit(
                    unitId,
                    new[] { $"{unitId}.dll" },
                    new[]
                    {
                        new TestCase
                        {
                            Name = unitId,
                            FullName = $"Tests.{unitId}",
                            Assembly = $"{unitId}.dll",
                            Category = new List<string>(),
                        },
                    })));
        }

        private static void UpdateMaximum(ref int maximum, int value)
        {
            int current;
            do
            {
                current = maximum;
                if (value <= current)
                {
                    return;
                }
            }
            while (Interlocked.CompareExchange(ref maximum, value, current) != current);
        }
    }
}
