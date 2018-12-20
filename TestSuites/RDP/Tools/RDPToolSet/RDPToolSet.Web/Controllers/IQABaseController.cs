using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RDPToolSet.Web.Models;

namespace RDPToolSet.Web.Controllers
{
    public class IQABaseController : Controller
    {
        //
        // GET: /IQA/

        protected const string Image1 = "ImageCompare1";
        protected const string Image2 = "ImageCompare2";

        public IQABaseController()
        {
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
        public ActionResult Upload(HttpPostedFileBase file, string imageType)
        {
            // file type
            if (file != null && file.ContentLength > 0)
            {
                string[] filenames = file.FileName.Split(new[] { '.' });
                string filename = String.Format("{0}_{1}.{2}", filenames[0], DateTime.Now.Ticks, filenames.Length >= 2 ? filenames[1] : "");

                string uploadPath = Server.MapPath("~/Images/Uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var path = Path.Combine(uploadPath, filename);
                file.SaveAs(path);

                if (imageType.Equals(Image1))
                {
                    Session[Image1] = path;
                }
                else if (imageType.Equals(Image2))
                {
                    Session[Image2] = path;
                }
                
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        internal static dynamic GetJsonObject(Stream jsonRequest)
        {
            string json;
            using (var reader = new StreamReader(jsonRequest))
                json = reader.ReadToEnd();
            dynamic obj = System.Web.Helpers.Json.Decode(json);
            return obj;
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
