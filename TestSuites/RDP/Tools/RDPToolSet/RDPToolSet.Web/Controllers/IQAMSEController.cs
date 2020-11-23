// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestSuites.Rdp.ImageQualityAccessment;
using RDPToolSet.Web.Utils;
using System;
using System.Drawing;

namespace RDPToolSet.Web.Controllers
{
    public class IQAMSEController : IQABaseController
    {
        public IQAMSEController(IWebHostEnvironment hostingEnvironment)
            : base(hostingEnvironment)
        {
        }

        public override ActionResult Index()
        {
            this.HttpContext.Session.SetObject(Image1, null);
            this.HttpContext.Session.SetObject(Image2, null);
            return View();
        }

        public override ActionResult Compare()
        {
            var mseCalc = new Mse();
            if (string.IsNullOrEmpty(this.HttpContext.Session.Get<string>(Image1)) || string.IsNullOrEmpty(this.HttpContext.Session.Get<string>(Image2)))
            {
                var obj = new { info = "Error", value = "Before comparing please input two images." };
                return Json(obj);
            }
            else
            {
                var image1 = (Bitmap)Bitmap.FromFile(this.HttpContext.Session.Get<string>(Image1));
                var image2 = (Bitmap)Bitmap.FromFile(this.HttpContext.Session.Get<string>(Image2));
                AssessResult result;
                try
                {
                    result = mseCalc.Assess(image1, image2, UseComponent.Luma | UseComponent.Cb | UseComponent.Cr);
                }
                catch (ArgumentException e)
                {
                    var obj = new { info = "Error", value = e.Message };
                    return Json(obj);
                }
                var mse = (result.Luma + result.Cb + result.Cr) / 3;
                if (mse.Equals(0.0))
                {
                    var obj = new { info = "Same", value = "These two images are exactly the same. MSE value is <Strong>Zero</Strong>." };
                    return Json(obj);
                }
                else
                {
                    var obj = new { info = "Different", value = "MSE value of these two images is <Strong>" + mse.ToString() + "</Strong>." };
                    return Json(obj);
                }
            }
        }
    }
}
