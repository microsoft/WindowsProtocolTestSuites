using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.Protocols.TestSuites.Rdp.ImageQualityAccessment;

namespace RDPToolSet.Web.Controllers
{
    public class IQAMSEController : IQABaseController
    {
        //
        // GET: /IQAMSE/

        public override ActionResult Index()
        {
            Session[Image1] = null;
            Session[Image2] = null;
            return View();
        }

        public override ActionResult Compare()
        {
            var mseCalc = new Mse();
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
