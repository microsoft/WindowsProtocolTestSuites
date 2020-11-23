// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestSuites.Rdp.ImageQualityAccessment;
using Newtonsoft.Json;
using RDPToolSet.Web.Utils;

namespace RDPToolSet.Web.Controllers
{
    public class IQAGSSIMController : IQABaseController
    {
        public IQAGSSIMController(IWebHostEnvironment hostingEnvironment)
            :base(hostingEnvironment)
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
            double k1, k2, alpha, beta, gamma;
            using (var bodyStream = new StreamReader(Request.Body))
            {
                var bodyText = bodyStream.ReadToEndAsync().GetAwaiter().GetResult();
                dynamic jsonObj = JsonConvert.DeserializeObject(bodyText);

                try
                {
                    k1 = Double.Parse(jsonObj.k1.ToString());
                    k2 = Double.Parse(jsonObj.k2.ToString());
                    alpha = Double.Parse(jsonObj.alpha.ToString());
                    beta = Double.Parse(jsonObj.beta.ToString());
                    gamma = Double.Parse(jsonObj.gamma.ToString());
                }
                catch (Exception e)
                {
                    var obj = new { info = "Error", value = e.Message };
                    return Json(obj);
                }
            }
            
            var gssimCalc = new Gssim();
            gssimCalc.SetParameters(k1, k2, alpha, beta, gamma);

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
                    result = gssimCalc.Assess(image1, image2, UseComponent.Luma | UseComponent.Cb | UseComponent.Cr);
                }
                catch (ArgumentException e)
                {
                    var obj = new { info = "Error", value = e.Message };
                    return Json(obj);
                }

                if (result.Luma == 1 && result.Cb == 1 && result.Cr == 1)
                {
                    var obj = new { info = "Same", value = "These two images are exactly the same. SSIM values for Luma, Cb and Cr components are all <Strong>1</Strong>." };
                    return Json(obj);
                }
                else
                {
                    var obj = new
                    {
                        info = "Different",
                        value = "SSIM values for Luma, Cb and Cr component are " +
                                "<Strong>" + result.Luma.ToString() + "</Strong>, " +
                                "<Strong>" + result.Cb.ToString() + "</Strong>, " +
                                "<Strong>" + result.Cr.ToString() + "</Strong> respectively"
                    };
                    return Json(obj);
                }
            }
        }
    }
}
