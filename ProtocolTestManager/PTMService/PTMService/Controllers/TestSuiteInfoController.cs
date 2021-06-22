// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using System.Linq;
using System;
using System.IO;
using Microsoft.Protocols.TestManager.PTMService.PTMKernelService;
using Microsoft.AspNetCore.Http;
using Microsoft.Protocols.TestManager.PTMService.Common.Profile;

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

        /// <summary>
        /// Endpoint to save profile by test result id
        /// </summary>
        /// <param name="testResultId">Test result id</param>
        /// <returns></returns>
        [Route("{testResultId}/profile/export")]
        [HttpPost]
        public IActionResult SaveProfile(int testResultId)
        {

            string profileLocation = PTMKernelService.SaveProfileSettingsByTestResult(testResultId);

            var profileStream = new FileStream(profileLocation, FileMode.Open, FileAccess.Read, FileShare.Read);
            
            return new FileStreamResult(profileStream, System.Net.Mime.MediaTypeNames.Text.Xml)
            {
                FileDownloadName = Path.GetFileName(profileLocation)
            };
        }

        /// <summary>
        /// Endpoint to save profile.
        /// </summary>
        /// <param name="request">Profile request instance</param>
        /// <returns>Profile stream</returns>
        [Route("profile/export")]
        [HttpPost]
        public IActionResult SaveProfile(ProfileExportRequest request)
        {

            request.FileName = PTMKernelService.EnsureProfileName(request.FileName);

            string profileLocation = PTMKernelService.SaveProfileSettings(request);

            var profileStream = new FileStream(profileLocation, FileMode.Open, FileAccess.Read, FileShare.Read);
            
            return new FileStreamResult(profileStream, System.Net.Mime.MediaTypeNames.Text.Xml)
            {
                FileDownloadName = Path.GetFileName(profileLocation)
            };
        }

        /// <summary>
        /// Loads an existing profile.
        /// </summary>
        /// <param name="package">Upload request.</param>
        /// <param name="testSuiteId">Test suite id</param>
        /// <param name="configurationId">Configuration id</param>
        /// <returns></returns>
        [Route("{testSuiteId}/profile/{configurationId}")]
        [HttpPost]
        public bool LoadProfile([FromForm] IFormFile package, [FromForm] int testSuiteId, [FromForm] int configurationId)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            var profileRequest = new ProfileRequest()
            {
                FileName = $"{Guid.NewGuid().ToString()}{TestSuiteConsts.ProfileExtension}",
                TestSuiteId = testSuiteId,
                ConfigurationId = configurationId,
                Stream = package.OpenReadStream()
            };

            PTMKernelService.LoadProfileSettings(profileRequest);
            return true;
        }
    }
}
