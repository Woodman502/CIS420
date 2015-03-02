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

        public ActionResult About()
        {
            IQueryable<VaccineNDCGroup> data = from vaccine in db.Vaccines
                                               group vaccine by vaccine.Barcode_NDC into numGroup
                                               select new VaccineNDCGroup()
                                               {

                                                   Barcode_NDC = numGroup.Key,
                                                   VaccineCount = numGroup.Count()
                                               };
            return View(data.ToList());
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