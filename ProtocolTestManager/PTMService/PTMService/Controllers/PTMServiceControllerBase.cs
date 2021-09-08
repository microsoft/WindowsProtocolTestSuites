// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;

namespace Microsoft.Protocols.TestManager.PTMService.PTMService.Controllers
{
    /// <summary>
    /// Base controller of PTM service.
    /// </summary>
    [ApiController]
    [PTMServiceExceptionFilter]
    public class PTMServiceControllerBase : ControllerBase
    {
        /// <summary>
        /// PTM kernel service.
        /// </summary>
        public IPTMKernelService PTMKernelService { get; private init; }

        /// <summary>
        /// The constructor of PTMServiceControllerBase.
        /// </summary>
        /// <param name="ptmKernelService">The PTM kernel service.</param>
        public PTMServiceControllerBase(IPTMKernelService ptmKernelService)
        {
            PTMKernelService = ptmKernelService;
        }
    }
}
