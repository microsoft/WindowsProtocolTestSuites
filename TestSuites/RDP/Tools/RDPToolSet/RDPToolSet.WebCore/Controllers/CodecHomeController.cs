using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RDPToolSet.WebCore.Models;

namespace RDPToolSet.WebCore.Controllers
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
