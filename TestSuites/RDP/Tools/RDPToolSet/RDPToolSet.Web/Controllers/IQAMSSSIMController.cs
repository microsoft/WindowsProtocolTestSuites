// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestSuites.Rdp.ImageQualityAccessment;
using Newtonsoft.Json;
using RDPToolSet.Web.Utils;
using System;
using System.Drawing;
using System.IO;

namespace RDPToolSet.Web.Controllers
{
    public class IQAMSSSIMController : IQABaseController
    {
        public IQAMSSSIMController(IWebHostEnvironment hostingEnvironment)
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
            double k1, k2;
            double[] alphas = new double[] { Double.NaN, 0, 0, 0, 0, 1 };
            double[] betas = new double[] { Double.NaN, 0.0448, 0.2856, 0.3001, 0.2363, 0.1333 };
            double[] gammas = new double[] { Double.NaN, 0.0448, 0.2856, 0.3001, 0.2363, 0.1333 };

            using (var bodyStream = new StreamReader(Request.Body))
            {
                var bodyText = bodyStream.ReadToEndAsync().GetAwaiter().GetResult();
                dynamic jsonObj = JsonConvert.DeserializeObject(bodyText);

                try
                {
                    k1 = Double.Parse(jsonObj.k1.ToString());
                    k2 = Double.Parse(jsonObj.k2.ToString());
                    alphas[1] = Double.Parse(jsonObj.alpha1.ToString());
                    alphas[2] = Double.Parse(jsonObj.alpha2.ToString());
                    alphas[3] = Double.Parse(jsonObj.alpha3.ToString());
                    alphas[4] = Double.Parse(jsonObj.alpha4.ToString());
                    alphas[5] = Double.Parse(jsonObj.alpha5.ToString());
                    betas[1] = Double.Parse(jsonObj.beta1.ToString());
                    betas[2] = Double.Parse(jsonObj.beta2.ToString());
                    betas[3] = Double.Parse(jsonObj.beta3.ToString());
                    betas[4] = Double.Parse(jsonObj.beta4.ToString());
                    betas[5] = Double.Parse(jsonObj.beta5.ToString());
                    gammas[1] = Double.Parse(jsonObj.gamma1.ToString());
                    gammas[2] = Double.Parse(jsonObj.gamma2.ToString());
                    gammas[3] = Double.Parse(jsonObj.gamma3.ToString());
                    gammas[4] = Double.Parse(jsonObj.gamma4.ToString());
                    gammas[5] = Double.Parse(jsonObj.gamma5.ToString());
                }
                catch (Exception e)
                {
                    var obj = new { info = "Error", value = e.Message };
                    return Json(obj);
                }
            }

            var msssimCalc = new Msssim();
            msssimCalc.SetParameters(k1, k2, alphas, betas, gammas);
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
                    result = msssimCalc.Assess(image1, image2, UseComponent.Luma | UseComponent.Cb | UseComponent.Cr);
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
