using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asp.netmvc5.Models;

namespace asp.netmvc5.Controllers
{
    public class GrantManagerController : Controller
    {
        // GET: GrantManager
        [HttpGet]
        public ActionResult GrantManager()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GrantManager(GrantManagerModel model)
        {
            foreach (string upload in Request.Files)
            {
                var uploadreq = Request.Files[upload];
                if (uploadreq != null && uploadreq.ContentLength == 0) continue;
                string pathToSave = Server.MapPath("~/Files");
                if (uploadreq != null)
                {
                    string filename = Path.GetFileName(uploadreq.FileName);
                    if (filename != null) uploadreq.SaveAs(Path.Combine(pathToSave, filename));
                }
            }
            return View();
        }
    }
}