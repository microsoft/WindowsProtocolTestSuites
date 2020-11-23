// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RDPToolSet.Web.Models;
using System;
using System.IO;

namespace RDPToolSet.Web.Controllers
{
    public class GuestBookController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public GuestBookController(IWebHostEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult Save([FromBody] GuestBookRequestModel request)
        {
            try
            {
                if(string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.message))
                {
                    return Json(ReturnResult<string>.Fail("Reqeust data is invalide"));
                }

                string guestbookPath = Path.Combine(this._hostingEnvironment.WebRootPath, "GuestBook");
                if (!Directory.Exists(guestbookPath))
                {
                    Directory.CreateDirectory(guestbookPath);
                }
                string guestbook = Path.Combine(guestbookPath, "gb-7E96D408-835C-474B-AED5-0214B3709DFB.txt");
                if (!System.IO.File.Exists(guestbook))
                {
                    System.IO.File.CreateText(guestbook).Close();
                }
                using (var writer = new StreamWriter(guestbook, true))
                {
                    writer.WriteLine("{");
                    writer.WriteLine("Email: " + request.email);
                    writer.WriteLine("Message:");
                    writer.WriteLine(request.message);
                    writer.WriteLine("}");
                    writer.WriteLine();
                    writer.WriteLine();
                }
                return Json(ReturnResult<string>.Success("Success"));
            }
            catch (Exception ex)
            {
                return Json(ReturnResult<string>.Fail(ex.Message));
            }
        }
    }
}
