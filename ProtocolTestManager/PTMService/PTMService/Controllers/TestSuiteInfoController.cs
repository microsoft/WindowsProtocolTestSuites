// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using System.Linq;

namespace Microsoft.Protocols.TestManager.PTMService.PTMService.Controllers
{
    /// <summary>
    /// Test suite info controller.
    /// </summary>
    [Route("api/testsuite")]
    [ApiController]
    public class TestSuiteInfoController : PTMServiceControllerBase
    {
        /// <summary>
        /// Constructor of test suite info controller.
        /// </summary>
        /// <param name="ptmKernelService">The PTM kernel service.</param>
        public TestSuiteInfoController(IPTMKernelService ptmKernelService)
            : base(ptmKernelService)
        {
        }

        /// <summary>
        /// Get test suites.
        /// </summary>
        /// <returns>An array containing test suites.</returns>
        [HttpGet]
        public TestSuite[] GetTestSuites()
        {
            var result = PTMKernelService.QueryTestSuites().Select(item => new TestSuite
            {
                Id = item.Id,
                Name = item.Name,
                Version = item.Version,
                Description = item.Description,
            }).ToArray();

            return result;
        }

        /// <summary>
        /// Get detail of a specific test suite.
        /// </summary>
        /// <param name="id">The test suite Id.</param>
        /// <returns>The detail of test suite.</returns>
        [Route("{id}")]
        [HttpGet]
        public TestSuite GetTestSuiteDetail(int id)
        {
            var testSuite = PTMKernelService.GetTestSuite(id);

            var testCases = testSuite.GetTestCases(null);

            var result = new TestSuite
            {
                Id = testSuite.Id,
                Name = testSuite.Name,
                Version = testSuite.Version,
                Description = testSuite.Description,
                TestCases = testCases.ToArray(),
            };

            return result;
        }
    }
}
