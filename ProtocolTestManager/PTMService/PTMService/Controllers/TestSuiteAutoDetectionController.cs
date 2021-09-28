// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestManager.PTMService.PTMService.Controllers
{
    /// <summary>
    /// Test suite auto detection controller.
    /// </summary>
    [Route("api/configuration")]
    [ApiController]
    public class TestSuiteAutoDetectionController : PTMServiceControllerBase
    {
        /// <summary>
        /// Constructor of test suite auto detection controller.
        /// </summary>
        /// <param name="ptmKernelService">The PTM kernel service.</param>
        public TestSuiteAutoDetectionController(IPTMKernelService ptmKernelService)
            : base(ptmKernelService)
        {
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
        /// <param name="properties">List of PrerequisiteProperty.</param>
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
        /// Apply auto detection result into profile rules and ptfconfig files.
        /// </summary>
        /// <param name="configurationId">Test suite configuration Id.</param>
        /// <returns>Action result.</returns>
        [Route("{configurationId}/autodetect/apply")]
        [HttpPost]
        public IActionResult ApplyAutoDetectionResult(int configurationId)
        {
            // Apply Detection Result.
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

            return Ok(new
            {
                ResultItemMapList = response
            });
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