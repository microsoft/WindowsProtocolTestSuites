using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RDPToolSet.Web.Controllers
{
    public class GuestBookController : Controller
    {
        //
        // GET: /GuestBook/

        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult Save()
        {
            string json;
            using (var reader = new StreamReader(Request.InputStream))
            {
                json = reader.ReadToEnd();
            }
            dynamic obj = System.Web.Helpers.Json.Decode(json);

            if (obj.email == null || obj.message == null || ((string)obj.email).Equals("") || ((string)obj.message).Equals(""))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string guestbookPath = Server.MapPath("~/Static/GuestBook");
            if(!Directory.Exists(guestbookPath)){
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
                writer.WriteLine("Email: " + obj.email);
                writer.WriteLine("Message:");
                writer.WriteLine(obj.message);
                writer.WriteLine("}");
                writer.WriteLine();
                writer.WriteLine();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}
