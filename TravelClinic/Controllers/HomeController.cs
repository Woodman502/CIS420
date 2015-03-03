using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asp.netmvc5.Models;

namespace asp.netmvc5.Controllers
{
    public class HomeController : Controller
    {
        private VaccineDBContext db = new VaccineDBContext();
        public ActionResult Index()
        {
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
    }
}