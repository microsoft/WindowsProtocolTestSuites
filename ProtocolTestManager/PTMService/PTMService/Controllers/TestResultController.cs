// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System;
using System.Linq;

namespace Microsoft.Protocols.TestManager.PTMService.PTMService.Controllers
{
    /// <summary>
    /// Test result controller.
    /// </summary>
    [Route("api/testresult")]
    [ApiController]
    public class TestResultController : PTMServiceControllerBase
    {
        /// <summary>
        /// Constructor of test result controller.
        /// </summary>
        /// <param name="ptmKernelService">The PTM kernel service.</param>
        public TestResultController(IPTMKernelService ptmKernelService)
            : base(ptmKernelService)
        {
        }

        /// <summary>
        /// List response.
        /// </summary>
        public class ListResponse
        {
            /// <summary>
            /// The total page count.
            /// </summary>
            public int PageCount { get; set; }

            /// <summary>
            /// The test results.
            /// </summary>
            public TestResultOverview[] TestResults { get; set; }
        }

        /// <summary>
        /// List test results.
        /// </summary>
        /// <param name="pageSize">Maximum count per page.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>The list response.</returns>
        [HttpGet]
        public ListResponse List(int pageSize, int pageNumber)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            if (pageNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber));
            }

            var items = PTMKernelService.QueryTestRuns(pageSize, pageNumber, out int totalPage).Select(testRun => new TestResultOverview
            {
                Id = testRun.Id,
                ConfigurationId = testRun.Configuration.Id,
                Status = testRun.State,
                Total = testRun.Total,
                NotRun = testRun.NotRun,
                Running = 0,
                Passed = testRun.Passed,
                Failed = testRun.Failed,
                Inconclusive = testRun.Inconclusive,
            }).ToArray();

            var result = new ListResponse
            {
                PageCount = totalPage,
                TestResults = items,
            };

            return result;
        }

        /// <summary>
        /// Get detail of a specific test result.
        /// </summary>
        /// <param name="id">The Id of test result.</param>
        /// <returns>The test result.</returns>
        [Route("{id}")]
        [HttpGet]
        public TestResultItem Get(int id)
        {
            var testRun = PTMKernelService.GetTestRun(id);

            var status = testRun.GetRunningStatus();

            var result = new TestResultItem
            {
                Overview = new TestResultOverview
                {
                    Id = id,
                    ConfigurationId = testRun.Configuration.Id,
                    Status = testRun.State,
                    Total = status.Count(),
                    NotRun = status.Where(i => i.Value.State == TestCaseState.NotRun).Count(),
                    Running = status.Where(i => i.Value.State == TestCaseState.Running).Count(),
                    Passed = status.Where(i => i.Value.State == TestCaseState.Passed).Count(),
                    Failed = status.Where(i => i.Value.State == TestCaseState.Failed).Count(),
                    Inconclusive = status.Where(i => i.Value.State == TestCaseState.Inconclusive).Count(),
                },
                Results = status.Values.ToArray(),
            };

            return result;
        }

        /// <summary>
        /// Get result of a specific test case.
        /// </summary>
        /// <param name="id">The Id of test result.</param>
        /// <param name="testCaseName">The name of test case.</param>
        /// <returns>The test case detail.</returns>
        [Route("{id}/testcase")]
        [HttpGet]
        public TestCaseResult GetTestCaseResult(int id, string testCaseName)
        {
            var testRun = PTMKernelService.GetTestRun(id);

            var result = testRun.GetTestCaseDetail(testCaseName);

            return result;
        }
    }
}
