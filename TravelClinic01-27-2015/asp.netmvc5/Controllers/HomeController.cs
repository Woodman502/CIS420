using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asp.netmvc5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Vaccines()
        {
            ViewBag.Message = "Vaccine Inventory page.";

            return View();
        }
        public ActionResult Calendar()
        {
            ViewBag.Message = "Calendar page.";

            return View();
        }
    }
}