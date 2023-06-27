// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using System;
using static Microsoft.Protocols.TestManager.PTMService.PTMService.Controllers.TestSuiteManagementController;

namespace Microsoft.Protocols.TestManager.PTMService.PTMService.Controllers
{
    /// <summary>
    /// Capabilities files management controller.
    /// </summary>
    [Route("api/management/capabilities")]
    [ApiController]
    public class CapabilitiesManagementController : PTMServiceControllerBase
    {

        /// <summary>
        /// Constructor of test suite management controller.
        /// </summary>
        /// <param name="ptmKernelService"></param>
        public CapabilitiesManagementController(IPTMKernelService ptmKernelService)
            : base(ptmKernelService)
        {

        }

        /// <summary>
        /// Request model for creating a capabilities file.
        /// </summary>
        public class CreateCapabilitiesFileRequest
        {
            /// <summary>
            /// Capabilities file name.
            /// </summary>
            public string CapabilitiesFileName { get; set; }

            /// <summary>
            /// Capabilities file description.
            /// </summary>
            public string CapabilitiesFileDescription { get; set; }

            /// <summary>
            /// The Id of the test suite to use.
            /// </summary>
            public int? TestSuiteId { get; set; }
        }

        /// <summary>
        /// Request model for updating a capabilities file.
        /// </summary>
        public class UpdateCapabilitiesFileRequest
        {
            /// <summary>
            /// Capabilities file name.
            /// </summary>
            public string CapabilitiesFileName { get; set; }

            /// <summary>
            /// Capabilities file description.
            /// </summary>
            public string CapabilitiesFileDescription { get; set; }
        }

        /// <summary>
        /// Create a new capabilities file.
        /// </summary>
        /// <param name="request">The create request.</param>
        /// <returns>The Id of the capabilities file.</returns>
        [HttpPost]
        public int Create([FromForm] CreateCapabilitiesFileRequest request)
        {
            if (string.IsNullOrEmpty(request.CapabilitiesFileName))
            {
                throw new ArgumentNullException(nameof(request.CapabilitiesFileName));
            }

            if (request.TestSuiteId == null || request.TestSuiteId == 0)
            {
                throw new ArgumentNullException(nameof(request.TestSuiteId));
            }

            int id = PTMKernelService.CreateCapabilitiesFile(request.CapabilitiesFileName, 
                                        request.CapabilitiesFileDescription,
                                        request.TestSuiteId.Value);

            return id;
        }


        /// <summary>
        /// Update metadata for an existing capabilities file.
        /// </summary>
        /// <param name="id">The Id of the capabilities file to update.</param>
        /// <param name="request">The update request.</param>
        /// <returns>The action result.</returns>
        [Route("{id}")]
        [HttpPost]
        public IActionResult Update(int id, [FromForm] UpdateCapabilitiesFileRequest request)
        {
            if (string.IsNullOrEmpty(request.CapabilitiesFileName))
            {
                throw new ArgumentNullException(nameof(request.CapabilitiesFileName));
            }

            PTMKernelService.UpdateCapabilitiesFileMetadata(id, request.CapabilitiesFileName, request.CapabilitiesFileDescription);

            return Ok();
        }

        /// <summary>
        /// Remove an existing capabilities file.
        /// </summary>
        /// <param name="id">The Id of the capabilities file to remove.</param>
        /// <returns>The action result.</returns>
        [Route("{id}")]
        [HttpDelete]
        public IActionResult Remove(int id)
        {
            PTMKernelService.RemoveCapabilitiesFile(id);

            return Ok();
        }
    }
}