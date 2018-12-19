using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Protocols.TestSuites.Rdp.ImageQualityAccessment;

namespace RDPToolSet.Web.Controllers
{
    public class IQAGSSIMController : IQABaseController
    {
        //
        // GET: /IQAGSSIM/

        public override ActionResult Index()
        {
            Session[Image1] = null;
            Session[Image2] = null;
            return View();
        }

        public override ActionResult Compare()
        {
            dynamic jsonObj = GetJsonObject(Request.InputStream);

            double k1, k2, alpha, beta, gamma;

            try
            {
                k1 = Double.Parse(jsonObj.k1);
                k2 = Double.Parse(jsonObj.k2);
                alpha = Double.Parse(jsonObj.alpha);
                beta = Double.Parse(jsonObj.beta);
                gamma = Double.Parse(jsonObj.gamma);
            }
            catch (Exception e)
            {
                var obj = new { info = "Error", value = e.Message };
                return Json(obj);
            }

            var gssimCalc = new Gssim();
            gssimCalc.SetParameters(k1, k2, alpha, beta, gamma);
            if (Session[Image1] == null || Session[Image2] == null)
            {
                var obj = new { info = "Error", value = "Before comparing please input two images." };
                return Json(obj);
            }
            else
            {
                var image1 = (Bitmap)Bitmap.FromFile((string)Session[Image1]);
                var image2 = (Bitmap)Bitmap.FromFile((string)Session[Image2]);
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
