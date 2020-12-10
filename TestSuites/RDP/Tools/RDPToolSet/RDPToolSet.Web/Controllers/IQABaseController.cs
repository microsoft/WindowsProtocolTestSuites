// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RDPToolSet.Web.Models;
using RDPToolSet.Web.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace RDPToolSet.Web.Controllers
{
    public class IQABaseController : Controller
    {
        protected readonly IWebHostEnvironment _hostingEnvironment;
        protected const string Image1 = "ImageCompare1";
        protected const string Image2 = "ImageCompare2";

        public IQABaseController(IWebHostEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            SetViewBag();
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Compare()
        {
            return null;
        }

        [HttpPost]
        public ActionResult Upload(IFormFile file, string imageType)
        {
            // file type
            if (file != null && file.Length > 0)
            {
                string[] filenames = file.FileName.Split(new[] { '.' });
                string filename = String.Format("{0}_{1}.{2}", filenames[0], DateTime.Now.Ticks, filenames.Length >= 2 ? filenames[1] : "");

                string uploadPath = Path.Combine(this._hostingEnvironment.WebRootPath, ActionHelper.ImageUploadFolder);
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var path = Path.Combine(uploadPath, filename);

                using (var fileStream = file.OpenReadStream())
                {
                    using (var newfileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.CopyTo(newfileStream);
                    }
                }

                if (imageType.Equals(Image1))
                {
                    this.HttpContext.Session.SetObject(Image1, path);
                }
                else if (imageType.Equals(Image2))
                {
                    this.HttpContext.Session.SetObject(Image2, path);
                }
            }
            return Json(ReturnResult<string>.Success("Success"));
        }

        private void SetViewBag()
        {
            var commonMenu = new Menu("MSE", "MSE", "IQAMSE", "PSNR", "IQAPSNR");
            var ssimMenu = new Menu("SSIM", "SSIM", "IQASSIM", "MS-SSIM", "IQAMSSSIM", "G-SSIM", "IQAGSSIM");
            var layoutModel = new LayoutViewModel(
                "Image Quality Assessment",
                "Index",
                "IQABase",
                 "Image Quality Assessment",
                 "Microsoft Protocol Test Suite Development Team",
                 "2.0",
                 commonMenu,
                 ssimMenu
                 );
            ViewBag.model = layoutModel;
        }
    }
}
