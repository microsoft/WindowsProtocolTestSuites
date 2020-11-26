// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json.Linq;

namespace RDPToolSet.Web.Utils
{
    public static class ActionHelper
    {
        public const string ImageUploadFolder = "Images/Uploads";

        public static string TrimEnter(this JToken s)
        {
            return s.ToString().Replace("\r",string.Empty);
        }
    }
}
