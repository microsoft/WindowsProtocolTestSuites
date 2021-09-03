// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Profile;
using Microsoft.Protocols.TestManager.PTMService.PTMKernelService;
using System;
using System.IO;
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
                Removed = item.Removed
            }).ToArray();

            return result;
        }

        /// <summary>
        /// Check User Guide of a specific test suite.
        /// </summary>
        /// <param name="testSuiteId">The test suite Id.</param>
        /// <returns>The action result.</returns>
        [Route("{testSuiteId}/userguide")]
        [HttpGet]
        public IActionResult GetTestSuiteDocs(int testSuiteId)
        {
            var testSuite = PTMKernelService.GetTestSuite(testSuiteId);
            string sourceUserGudiePath = Path.Combine(testSuite.StorageRoot.AbsolutePath, TestSuiteConsts.PluginFolderName, TestSuiteConsts.UserGuideFolderName);
            DirectoryInfo dir = new DirectoryInfo(sourceUserGudiePath);
            if (!dir.Exists)
            {
                return NotFound("Missing user guide in package.");
            }

            return Ok();
        }

        /// <summary>
        /// Get User Guide of a specific test suite.
        /// </summary>
        /// <param name="testSuiteId">The test suite Id.</param>
        /// <param name="fileName">file.</param>
        /// <returns>The action result.</returns>
        [Route("{testSuiteId}/userguide/{fileName}")]
        [HttpGet]
        public IActionResult GetTestSuiteDocs(int testSuiteId, string fileName)
        {
            var testSuite = PTMKernelService.GetTestSuite(testSuiteId);
            string sourceUserGudiePath = Path.Combine(testSuite.StorageRoot.AbsolutePath, TestSuiteConsts.PluginFolderName, TestSuiteConsts.UserGuideFolderName);

            var file = Path.Combine(sourceUserGudiePath, fileName);
            var fileInfo = new FileInfo(file);
            var extension = fileInfo.Extension;

            string GetMIMEType(string fileExtension)
            {
                return fileExtension switch
                {
                    ".html" => "text/html",
                    ".png" => "image/png"
                };
            }

            return File(System.IO.File.ReadAllBytes(file), GetMIMEType(extension));
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
        /// Loads an existing profile.
        /// </summary>
        /// <param name="package">Upload request.</param>
        /// <param name="testSuiteId">Test suite id.</param>
        /// <param name="configurationId">Configuration id.</param>
        /// <returns></returns>
        [Route("{testSuiteId}/profile")]
        [HttpPost]
        public bool LoadProfile([FromForm] IFormFile package, int testSuiteId, [FromQuery] int configurationId)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            var profileRequest = new ProfileRequest()
            {
                FileName = $"{Guid.NewGuid()}{TestSuiteConsts.ProfileExtension}",
                TestSuiteId = testSuiteId,
                ConfigurationId = configurationId,
                Stream = package.OpenReadStream()
            };

            PTMKernelService.LoadProfileSettings(profileRequest);
            return true;
        }
    }
}
