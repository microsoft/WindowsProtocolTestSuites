// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using System.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Protocols.TestManager.PTMService.PTMKernelService;
using Microsoft.AspNetCore.Http;
using Microsoft.Protocols.TestManager.PTMService.Common.Profile;
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;

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
                Removed = item.Removed
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

            if (testSuite.Removed)
            {
                throw new InvalidOperationException($"The test suite with the ID {id} is removed.");
            }

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

        /// <summary>
        /// Initialize IAutoDetection instance.
        /// </summary>
        /// <param name="configurationId">Test suite configuration Id.</param>
        /// <returns>The action result.</returns>
        [Route("{configurationId}/autodetect/initialize")]
        [HttpPost]
        public IActionResult InitializeDetector(int configurationId)
        {
            PTMKernelService.CreateAutoDetector(configurationId);

            return Ok();
        }

        /// <summary>
        /// Gets auto detection prerequisites.
        /// </summary>
        /// <param name="configurationId">Test suite configuration Id.</param>
        /// <returns>Prerequisite View.</returns>
        [Route("{configurationId}/autodetect/prerequisites")]
        [HttpGet]
        public PrerequisiteView GetPrerequisites(int configurationId)
        {
            var response = PTMKernelService.GetPrerequisites(configurationId);

            return response;
        }

        /// <summary>
        /// Get auto detection steps.
        /// </summary>
        /// <param name="configurationId">Test suite configuration Id.</param>
        /// <returns>List of detecting steps including steps required for auto detection.</returns>
        [Route("{configurationId}/autodetect/detectionsteps")]
        [HttpGet]
        public IActionResult GetDetectionSteps(int configurationId)
        {
            var detectResult = PTMKernelService.GetDetectionOutcome(configurationId);
            return Ok(new
            {
                Result = new
                {
                    Status = detectResult.Status,
                    Exception = detectResult.Exception == null ? string.Empty : detectResult.Exception.ToString(),
                },
                DetectionSteps = PTMKernelService.GetDetectedSteps(configurationId)
            });
        }

        /// <summary>
        /// Start auto detection.
        /// </summary>
        /// <param name="properties">List of PrerequisitProperty.</param>
        /// <param name="configurationId">Test suite configuration Id.</param>
        /// <returns>The action result.</returns>
        [Route("{configurationId}/autodetect/start")]
        [HttpPost]
        public IActionResult StartAutoDetection(List<Property> properties, int configurationId)
        {
            PTMKernelService.Reset(configurationId);
            var setPrerequisite = PTMKernelService.SetPrerequisites(properties, configurationId);
            if (setPrerequisite)
            {
                PTMKernelService.StartDetection(configurationId, (o) => { });

                return Ok();
            }
            else
            {
                return BadRequest("There's errors when set prerequisites");
            }
        }

        /// <summary>
        /// Stop auto detection.
        /// </summary>
        /// <param name="configurationId">Test suite configuration Id.</param>
        /// <returns>Action result.</returns>
        [Route("{configurationId}/autodetect/stop")]
        [HttpPost]
        public IActionResult StopAutoDetection(int configurationId)
        {
            PTMKernelService.StopDetection(configurationId, () => { });

            return Ok();
        }

        /// <summary>
        /// Apply auto detection result.
        /// </summary>
        /// <param name="configurationId">Test suite configuration Id.</param>
        /// <returns>Action result.</returns>
        [Route("{configurationId}/autodetect/apply")]
        [HttpPost]
        public IActionResult ApplyAutoDetectionResult(int configurationId)
        {
            PTMKernelService.ApplyDetectionResult(configurationId);

            return Ok();
        }

        /// <summary>
        /// Get auto detection summary.
        /// </summary>
        /// <param name="configurationId">Test suite configuration Id.</param>
        /// <returns>List of result item map containing auto.</returns>
        [Route("{configurationId}/autodetect/summary")]
        [HttpGet]
        public IActionResult GetDetectionSummary(int configurationId)
        {
            var response = PTMKernelService.GetDetectionSummary(configurationId);

            return Ok(response);
        }

        /// <summary>
        /// Get Auto Detection Log
        /// </summary>
        /// <param name="configurationId">Test suite configuration Id.</param>
        /// <returns>Auto Detection Log</returns>
        [Route("{configurationId}/autodetect/log")]
        [HttpGet]
        public IActionResult GetDetectionLog(int configurationId)
        {
            var response = PTMKernelService.GetDetectionLog(configurationId);

            return Ok(response);
        }
    }
}
