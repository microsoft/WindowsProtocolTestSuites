// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using Microsoft.Protocols.TestManager.PTMService.Common.Profile;
using Microsoft.Protocols.TestManager.PTMService.PTMKernelService;
using System;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Microsoft.Protocols.TestManager.PTMService.PTMService.Controllers
{
    file static class CapabilitiesTestCasesFilterType
    {
        public const string ByName = "Name";
        public const string ByTestCategory = "TestCategory";
        public const string ByClass = "Class";
    }

    /// <summary>
    /// Filter parameters for filtering capabilities test cases.
    /// </summary>
    file class FilterParams
    {
        /// <summary>
        /// Test cases to filter.
        /// </summary>
        public string[] TestCases { get; set; }

        /// <summary>
        /// Type to filter by.
        /// </summary>
        public string FilterType { get; set; }

        /// <summary>
        /// Filter to apply.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Name of the test suite the test cases belong to.
        /// </summary>
        public string TestSuiteName { get; set; }

        /// <summary>
        /// Version of the test suite the test cases belong to.
        /// </summary>
        public string TestSuiteVersion { get; set; }
    }

    /// <summary>
    /// Filter result for filtered capabilities test cases.
    /// </summary>
    file class FilterResult
    {
        /// <summary>
        /// Filtered test cases.
        /// </summary>
        public string[] TestCases { get; set; }

        /// <summary>
        /// Type to filter by.
        /// </summary>
        public string FilterType { get; set; }

        /// <summary>
        /// Filter to apply.
        /// </summary>
        public string Filter { get; set; }
    }

    /// <summary>
    /// Json details for a capabilities config file.
    /// </summary>
    file class JsonInfo
    {
        /// <summary>
        /// Name of the file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Json content for the file.
        /// </summary>
        public string Json { get; set; }
    }

    /// <summary>
    /// Capabilities files info controller.
    /// </summary>
    [Route("api/capabilities")]
    [ApiController]
    public class CapabilitiesInfoController : PTMServiceControllerBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="CapabilitiesInfoController"/>
        /// </summary>
        /// <param name="ptmKernelService">The PTM kernel service.</param>
        public CapabilitiesInfoController(IPTMKernelService ptmKernelService)
            : base(ptmKernelService)
        {
        }

        /// <summary>
        /// Request model for filtering capabilities test cases.
        /// </summary>
        public class FilterCapabilitiesTestCasesRequest
        {
            /// <summary>
            /// Json request containing the filter parameters.
            /// </summary>
            public string RequestJson { get; set; }
        }

        /// <summary>
        /// Get capabilities files.
        /// </summary>
        /// <returns>An array containing capabilities files.</returns>
        [HttpGet]
        public CapabilitiesConfig[] GetCapabilitiesConfig()
        {
            var result = PTMKernelService.QueryCapabilitiesConfigAndCleanUp();

            return result;
        }

        /// <summary>
        /// Get a capabilities config file.
        /// </summary>
        /// <returns>Json document representing the capabilities config.</returns>
        [Route("{id}")]
        [HttpGet]
        public IActionResult GetCapabilitiesConfigJson(int id)
        {
            var (name, location) =
                PTMKernelService.GetCapabilitiesConfigInfo(id);

            if (location == null)
            {
                return new NotFoundResult();
            }

            var json = CapabilitiesConfigReader.Parse(new FileInfo(location)).Json;

            return Ok(new JsonInfo()
            {
                Name = name,
                Json = json.ToJsonString()
            });
        }

        /// <summary>
        /// Endpoint to download a capabilities json file.
        /// </summary>
        /// <param name="id">Id of the capabilities file to download.</param>
        /// <returns>A stream to the capabilities file returned.</returns>
        [Route("download/{id}")]
        [HttpGet]
        public IActionResult DownloadCapabilitiesConfig(int id)
        {
            var (name, location) =
                PTMKernelService.GetCapabilitiesConfigInfo(id);

            if (location == null)
            {
                return new NotFoundResult();
            }

            using var stream = new FileStream(location, FileMode.Open, FileAccess.Read, FileShare.Read);
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(memoryStream, "application/json")
            {
                FileDownloadName = $"{name}-{Path.GetFileName(location)}"
            };
        }

        /// <summary>
        /// Endpoint to download a capabilities json file with test cases not in any category filtered out.
        /// </summary>
        /// <param name="id">Id of the capabilities file to download.</param>
        /// <returns>A stream to the capabilities file returned.</returns>
        [Route("download/filtered/{id}")]
        [HttpGet]
        public IActionResult DownloadFilteredCapabilitiesConfig(int id)
        {
            var (name, location) =
                PTMKernelService.GetCapabilitiesConfigInfo(id);

            if (location == null)
            {
                return new NotFoundResult();
            }

            var json = CapabilitiesConfigReader.Parse(new FileInfo(location))
                        .GetJson(skipTestsWithNoCategory: true);

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json.ToJsonString()));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(memoryStream, "application/json")
            {
                FileDownloadName = $"{name}-filtered-{Path.GetFileName(location)}"
            };
        }

        /// <summary>
        /// Filters test cases and returns the filtered list.
        /// </summary>
        /// <param name="request">The filter request.</param>
        /// <returns>Filtered list of test cases.</returns>
        [Route("filter")]
        [HttpPost]
        public IActionResult FilterCapabilitiesTestCases(
            [FromForm] FilterCapabilitiesTestCasesRequest request)
        {
            var parameters = JsonSerializer.Deserialize<FilterParams>(request.RequestJson);

            if(parameters.FilterType == CapabilitiesTestCasesFilterType.ByName)
            {
                var testCases =
                    PTMKernelService.FilterCapabilitiesTestCasesByName(
                        parameters.TestCases, parameters.Filter);

                return Ok(new FilterResult()
                {
                    TestCases = testCases,
                    Filter = parameters.Filter,
                    FilterType = parameters.FilterType
                });
            }
            else
            {
                var testSuite =
                    PTMKernelService.QueryTestSuites()
                        .Where(t => t.Name == parameters.TestSuiteName &&
                                    t.Version == parameters.TestSuiteVersion
                         ).FirstOrDefault();
                if (testSuite == null)
                {
                    throw new InvalidOperationException($"Test suite with name, {parameters.TestSuiteName} and version, {parameters.TestSuiteVersion} not found.");
                }

                var testCases = parameters.FilterType switch
                {
                    CapabilitiesTestCasesFilterType.ByTestCategory
                        => PTMKernelService.FilterCapabilitiesTestCasesByCategory(
                            parameters.TestCases, parameters.Filter, testSuite),
                    CapabilitiesTestCasesFilterType.ByClass
                        => PTMKernelService.FilterCapabilitiesTestCasesByClass(
                            parameters.TestCases, parameters.Filter, testSuite),
                    _ => parameters.TestCases
                };

                return Ok(new FilterResult()
                {
                    TestCases = testCases,
                    Filter = parameters.Filter,
                    FilterType = parameters.FilterType
                });
            }
        }
    }
}