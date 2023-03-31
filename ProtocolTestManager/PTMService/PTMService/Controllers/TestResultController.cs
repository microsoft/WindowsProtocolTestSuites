// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using Microsoft.Protocols.TestManager.PTMService.Common.Profile;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System;
using System.IO;
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

        public class ReportRequest
        {
            public string[] TestCases { get; set; }

            public ReportFormat Format { get; set; }
        }

        /// <summary>
        /// List test results.
        /// </summary>
        /// <param name="pageSize">Maximum count per page.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="query">The query phrase to search the test resutls.</param>
        /// <param name="showAll">Whether to show test results of all test suties including removed test suites.</param>
        /// <returns>The list response.</returns>
        [HttpGet]
        public ListResponse List(int pageSize, int pageNumber, string query = "", bool showAll = false)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            if (pageNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber));
            }

            Func<TestResult, bool> queryFunc = query switch
            {
                null or "" => (TestResult result) =>
                {
                    if (showAll)
                    {
                        return true;
                    }
                    else
                    {
                        var configuration = PTMKernelService.GetConfiguration(result.TestSuiteConfigurationId);
                        var testSuite = PTMKernelService.GetTestSuite(configuration.TestSuite.Id);
                        return true && !testSuite.Removed;
                    }
                },
                _ => (TestResult result) =>
                {
                    var containingQuery = false;
                    var configuration = PTMKernelService.GetConfiguration(result.TestSuiteConfigurationId);
                    if (configuration.Name.ToLower().Contains(query.ToLower()))
                    {
                        containingQuery = true;
                    }

                    var testSuite = PTMKernelService.GetTestSuite(configuration.TestSuite.Id);
                    if (testSuite.Name.ToLower().Contains(query.ToLower()))
                    {
                        containingQuery = true;
                    }

                    if (showAll)
                    {
                        return containingQuery;
                    }
                    else
                    {
                        return containingQuery && !testSuite.Removed;
                    }
                }
            };

            var items = PTMKernelService.QueryTestRuns(pageSize, pageNumber, queryFunc, out int totalPage).Select(testRun => new TestResultOverview
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
        /// Get test result report.
        /// </summary>
        /// <param name="id">The Id of test result.</param>
        /// <param name="request">The report export request.</param>
        /// <returns>The test result report.</returns>
        [Route("{id}/report")]
        [HttpPost]
        public IActionResult GetTestRunReport(int id, [FromBody] ReportRequest request)
        {
            var reportPath = PTMKernelService.GetTestRunReport(id, request.Format, request.TestCases);
            var reportStream = new FileStream(reportPath, FileMode.Open, FileAccess.Read, FileShare.Read);

            return new FileStreamResult(reportStream, GetMIMEType(request.Format))
            {
                FileDownloadName = Path.GetFileName(reportPath)
            };

            string GetMIMEType(ReportFormat format) => format switch
            {
                ReportFormat.Plain => "text/plain",
                ReportFormat.Json => "text/plain",
                ReportFormat.XUnit => "application/xml",
                _ => throw new InvalidOperationException($"\"{format}\" is not a valid report format.")
            };
        }

        /// <summary>
        /// Endpoint to save profile by test result id.
        /// </summary>
        /// <param name="id">Test result id.</param>
        /// <param name="request">The request from front end.</param>
        /// <returns>The profile generate by the test result id.</returns>
        [Route("{id}/profile")]
        [HttpPost]
        public IActionResult SaveProfile(int id, [FromBody] ProfileExportRequest request = null)
        {
            ProfileExportRequest profileRequest = null;
            if (request != null && request.SelectedTestCases != null)
            {
                var testRun = PTMKernelService.GetTestRun(id);
                profileRequest = new ProfileExportRequest()
                {
                    FileName = PTMKernelService.EnsureProfileName(null),
                    TestSuiteId = testRun.Configuration.TestSuite.Id,
                    ConfigurationId = testRun.Configuration.Id,
                    TestResultId = id,
                    SelectedTestCases = request.SelectedTestCases
                };
            }

            string profileLocation = profileRequest == null ? PTMKernelService.SaveProfileSettingsByTestResult(id) : PTMKernelService.SaveProfileSettings(profileRequest);

            var profileStream = new FileStream(profileLocation, FileMode.Open, FileAccess.Read, FileShare.Read);

            return new FileStreamResult(profileStream, System.Net.Mime.MediaTypeNames.Text.Xml)
            {
                FileDownloadName = Path.GetFileName(profileLocation)
            };
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

            var result = testRun.GetTestCaseResult(testCaseName);

            return result;
        }

        /// <summary>
        /// Remove an existing test result.
        /// </summary>
        /// <param name="id">The ID of the test result to be removed.</param>
        /// <returns>The action result.</returns>
        [Route("{id}")]
        [HttpDelete]
        public IActionResult Remove(int id)
        {
            PTMKernelService.RemoveTestRun(id);

            return Ok();
        }
    }
}
