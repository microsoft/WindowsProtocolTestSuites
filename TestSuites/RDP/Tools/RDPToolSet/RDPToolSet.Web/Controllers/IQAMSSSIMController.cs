using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Protocols.TestSuites.Rdp.ImageQualityAccessment;

namespace RDPToolSet.Web.Controllers
{
    public class IQAMSSSIMController : IQABaseController
    {
        //
        // GET: /IQAMSSSIM/

        public override ActionResult Index()
        {
            Session[Image1] = null;
            Session[Image2] = null;
            return View();
        }

        public override ActionResult Compare()
        {
            dynamic jsonObj = GetJsonObject(Request.InputStream);

            double k1, k2;
            double[] alphas = new double[] { Double.NaN, 0, 0, 0, 0, 1 };
            double[] betas = new double[] { Double.NaN, 0.0448, 0.2856, 0.3001, 0.2363, 0.1333 };
            double[] gammas = new double[] { Double.NaN, 0.0448, 0.2856, 0.3001, 0.2363, 0.1333 };


            try
            {
                k1 = Double.Parse(jsonObj.k1);
                k2 = Double.Parse(jsonObj.k2);
                alphas[1] = Double.Parse(jsonObj.alpha1);
                alphas[2] = Double.Parse(jsonObj.alpha2);
                alphas[3] = Double.Parse(jsonObj.alpha3);
                alphas[4] = Double.Parse(jsonObj.alpha4);
                alphas[5] = Double.Parse(jsonObj.alpha5);
                betas[1] = Double.Parse(jsonObj.beta1);
                betas[2] = Double.Parse(jsonObj.beta2);
                betas[3] = Double.Parse(jsonObj.beta3);
                betas[4] = Double.Parse(jsonObj.beta4);
                betas[5] = Double.Parse(jsonObj.beta5);
                gammas[1] = Double.Parse(jsonObj.gamma1);
                gammas[2] = Double.Parse(jsonObj.gamma2);
                gammas[3] = Double.Parse(jsonObj.gamma3);
                gammas[4] = Double.Parse(jsonObj.gamma4);
                gammas[5] = Double.Parse(jsonObj.gamma5);
            }
            catch (Exception e)
            {
                var obj = new { info = "Error", value = e.Message };
                return Json(obj);
            }

            var msssimCalc = new Msssim();
            msssimCalc.SetParameters(k1, k2, alphas, betas, gammas);
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
