using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Protocols.TestSuites.Rdp.ImageQualityAccessment;

namespace RDPToolSet.Web.Controllers
{
    public class IQAPSNRController : IQABaseController
    {
        //
        // GET: /IQAPSNR/

        public override ActionResult Index()
        {
            Session[Image1] = null;
            Session[Image2] = null;
            return View();
        }

        public override ActionResult Compare()
        {
            var psnrCalc = new Psnr();
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
                    result = psnrCalc.Assess(image1, image2, UseComponent.Luma | UseComponent.Cb | UseComponent.Cr);
                }
                catch (ArgumentException e)
                {
                    var obj = new { info = "Error", value = e.Message };
                    return Json(obj);
                }

                if (Double.IsInfinity(result.Luma))
                {
                    var obj = new { info = "Same", value = "These two images are exactly the same. SSIM value is <Strong>Infinity</Strong>." };
                    return Json(obj);
                }
                else
                {
                    var obj = new { info = "Different", value = "PSNR value of these two images is <Strong>" + result.Luma.ToString() + " dB</Strong>." };
                    return Json(obj);
                }
            }
        }

    }
}
