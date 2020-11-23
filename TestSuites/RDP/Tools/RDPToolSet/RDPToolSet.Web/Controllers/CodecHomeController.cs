// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RDPToolSet.Web.Models;

namespace RDPToolSet.Web.Controllers
{
    public class CodecHomeController : Controller
    {
        public ActionResult Index()
        {
            var rfxMenu = new Menu("RemoteFX", "Encode", "RFXEncode", "Decode", "RFXDecode");
            var rfxpMenu = new Menu("RemoteFXProg", "Encode", "RFXPEncode", "Decode", "RFXPDecode");
            var layoutModel = new LayoutViewModel(
                "RDP Protocol Tool Set",
                "Index",
                "CodecHome",
                 "RDP Codec Tool",
                 "Microsoft Protocol Test Suite Development Team",
                 "2.0",
                 rfxMenu,
                 rfxpMenu
                 );
            ViewBag.model = layoutModel;

            return View();
        }
    }
}
