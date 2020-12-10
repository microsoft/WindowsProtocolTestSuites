// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace RDPToolSet.Web.Controllers
{
    public class ReturnResult<T>
    {
        public bool status { get; set; }

        public string message { get; set; }

        public T data { get; set; }

        public static ReturnResult<T> Success(T data, string message = "success")
        {
            return new ReturnResult<T>()
            {
                status = true,
                message = message,
                data = data
            };
        }

        public static ReturnResult<T> Fail(T data, string message = "fail")
        {
            return new ReturnResult<T>()
            {
                status = false,
                message = message,
                data = data
            };
        }
    }
}
