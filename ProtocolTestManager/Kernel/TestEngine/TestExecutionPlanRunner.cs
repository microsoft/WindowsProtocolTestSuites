// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.Kernel
{
    public sealed class TestExecutionPlanRunner
    {
        public async Task<TestExecutionPlanResult> RunAsync(
            TestExecutionPlan plan,
            Func<TestExecutionUnit, CancellationToken, Task> runUnitAsync,
            CancellationToken cancellationToken)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            if (runUnitAsync == null)
            {
                throw new ArgumentNullException(nameof(runUnitAsync));
            }

            var failures = new ConcurrentBag<TestExecutionFailure>();

            foreach (var stage in plan.Stages)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using var concurrencyGate = new SemaphoreSlim(plan.MaxDegreeOfParallelism);
                var tasks = stage.Units.Select(async unit =>
                {
                    await concurrencyGate.WaitAsync(cancellationToken).ConfigureAwait(false);
                    try
                    {
                        await runUnitAsync(unit, cancellationToken).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                    {
                        throw;
                    }
                    catch (Exception exception)
                    {
                        failures.Add(new TestExecutionFailure(stage.Name, unit.Id, exception));
                    }
                    finally
                    {
                        concurrencyGate.Release();
                    }
                });

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }

            return new TestExecutionPlanResult(failures);
        }
    }

    public sealed class TestExecutionPlanResult
    {
        public TestExecutionPlanResult(IEnumerable<TestExecutionFailure> failures)
        {
            Failures = failures?
                .OrderBy(failure => failure.StageName, StringComparer.Ordinal)
                .ThenBy(failure => failure.UnitId, StringComparer.Ordinal)
                .ToList()
                ?? throw new ArgumentNullException(nameof(failures));
        }

        public IReadOnlyList<TestExecutionFailure> Failures { get; }

        public bool Succeeded => Failures.Count == 0;
    }

    public sealed class TestExecutionFailure
    {
        public TestExecutionFailure(string stageName, string unitId, Exception exception)
        {
            StageName = stageName ?? throw new ArgumentNullException(nameof(stageName));
            UnitId = unitId ?? throw new ArgumentNullException(nameof(unitId));
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

        public string StageName { get; }

        public string UnitId { get; }

        public Exception Exception { get; }
    }
}
