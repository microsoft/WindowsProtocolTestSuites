// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    public static class PTMKernelServiceExtensions
    {
        public static IServiceCollection AddPTMKernelService(this IServiceCollection services)
        {
            services.AddSingleton<IPTMKernelService, PTMKernelService>();

            return services;
        }
    }
}
