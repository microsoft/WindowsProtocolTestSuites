// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Microsoft.Protocols.TestManager.PTMService.PTMService.Controllers
{
    public class PTMServiceExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            context.Result = new JsonResult(exception.Message)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }
    }
}
