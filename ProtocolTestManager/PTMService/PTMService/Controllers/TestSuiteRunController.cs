// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using System;

namespace Microsoft.Protocols.TestManager.PTMService.PTMService.Controllers
{
    /// <summary>
    /// The test suite run controller.
    /// </summary>
    [Route("api/run")]
    [ApiController]
    public class TestSuiteRunController : PTMServiceControllerBase
    {
        /// <summary>
        /// Constructor of test suite run controller.
        /// </summary>
        /// <param name="ptmKernelService">The PTM kernel service.</param>
        public TestSuiteRunController(IPTMKernelService ptmKernelService)
            : base(ptmKernelService)
        {
        }

        /// <summary>
        /// Run request.
        /// </summary>
        public class RunRequest
        {
            /// <summary>
            /// Configuration Id.
            /// </summary>
            public int ConfigurationId { get; set; }

            /// <summary>
            /// Selected test cases.
            /// </summary>
            public string[] SelectedTestCases { get; set; }
        }

        /// <summary>
        /// Run test suite.
        /// </summary>
        /// <param name="request">The run request.</param>
        /// <returns>The test result Id.</returns>
        [HttpPost]
        public int Run(RunRequest request)
        {
            if (PTMKernelService.IsTestSuiteRunning())
            {
                throw new NotSupportedException("PTMService does not support parallel run right now, please wait for the current running job to be completed and then try again.");
            }

            var result = PTMKernelService.CreateTestRun(request.ConfigurationId, request.SelectedTestCases);

            return result;
        }

        /// <summary>
        /// Abort a running test suite.
        /// </summary>
        /// <param name="id">The test result Id.</param>
        /// <returns>The action result.</returns>
        [Route("{id}")]
        [HttpPut]
        public IActionResult Abort(int id)
        {
            var testRun = PTMKernelService.GetTestRun(id);

            testRun.Abort();

            return Ok();
        }

        /// <summary>
        /// Check if any test suite is running
        /// </summary>
        /// <returns></returns>
        [Route("check")]
        [HttpGet]
        public IActionResult Check()
        {
            return Ok(PTMKernelService.IsTestSuiteRunning());
        }
    }
}
