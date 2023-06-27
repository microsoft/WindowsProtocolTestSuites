// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Entities;
using Microsoft.Protocols.TestManager.PTMService.Common.Profile;
using Microsoft.Protocols.TestManager.PTMService.PTMKernelService;
using System;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace Microsoft.Protocols.TestManager.PTMService.PTMService.Controllers
{
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
        /// Endpoint to download a capabilities json file.
        /// </summary>
        /// <param name="id">Id of the capabilities file to download.</param>
        /// <returns>A stream to the capabilities file returned.</returns>
        [Route("download/{id}")]
        [HttpGet]
        public IActionResult DownloadCapabilitiesConfig(int id)
        {
            var location =
                PTMKernelService.GetCapabilitiesConfigJsonPath(id);

            using var stream = new FileStream(location, FileMode.Open, FileAccess.Read, FileShare.Read);
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(memoryStream, "application/json")
            {
                FileDownloadName = $"{id}-{Path.GetFileName(location)}"
            };
        }
    }
}
