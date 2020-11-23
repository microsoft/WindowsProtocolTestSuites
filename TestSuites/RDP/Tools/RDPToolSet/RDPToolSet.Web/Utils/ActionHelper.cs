// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json.Linq;
using RDPToolSet.Web.Controllers;
using System;
using System.Threading.Tasks;

namespace RDPToolSet.Web.Utils
{
    public static class ActionHelper
    {
        public const string ImageUploadFolder = "Images/Uploads";

        public static async Task<ReturnResult<string>> ExecuteWithCatchException(Action action)
        {
            try
            {
                action();
                return ReturnResult<string>.Success("Success");
            }
            catch (Exception ex)
            {
                return ReturnResult<string>.Fail(ex.Message);
            }
        }

        public static string TrimEnter(this JToken s)
        {
            return s.ToString().Replace("\r",string.Empty);
        }
    }
}
