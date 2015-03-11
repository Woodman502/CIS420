﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using asp.netmvc5.Models;

namespace asp.netmvc5.Controllers
{
    public class VaccinesController : Controller
    {
        private VaccineDBContext db = new VaccineDBContext();

        // GET: Vaccines
        public ActionResult Index()
        {
            //var vaccines = db.Vaccines.Include(v => v.NDC_Lookup);
            var vaccines = from r in db.Vaccines.Include(v => v.NDC_Lookup)
                       where r.Administered.Equals(false)
                       select r;
            return View(vaccines.ToList());
        }
        public ActionResult Administered()
        {
            //var vaccines = db.Vaccines.Include(v => v.NDC_Lookup);
            var vaccines = from r in db.Vaccines.Include(v => v.NDC_Lookup)
                           where r.Administered.Equals(true)
                           select r;
            return View(vaccines.ToList());
        }

        public ActionResult TotalSales()
        {
            //var vaccines = db.Vaccines.Include(v => v.NDC_Lookup);
            List<Sales> vaccines = (from r in db.Vaccines.Include(v => v.NDC_Lookup) join p in db.Patient_Vaccinations on r.Id equals p.VaccineID
                            where r.Administered.Equals(true)
                           group p by new {r.Description} into g
                           select new Sales()
                           {
                               Description = g.Key.Description,
                               SumOfPrice = g.Sum(x => x.Price_Paid)
                               

                           }
                           ).ToList();
            return View(vaccines.ToList());
        }
       

        public ActionResult GetNDC(string term)
        {
             var result = 
                 from r in db.NDC_Lookup
                         where r.Barcode_NDC.ToString().Contains(term) 
                         select new { Description = r.Description_CVX, Barcode_NDC = r.Barcode_NDC.ToString()};

            return Json(result ,JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDescription(string term)
        {
            var result = (from r in db.NDC_Lookup
                          where r.Description_CVX.StartsWith(term)
                          select r.Description_CVX).Take(15);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Vaccines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vaccine vaccine = db.Vaccines.Find(id);
            if (vaccine == null)
            {
                return HttpNotFound();
            }
            return View(vaccine);
        }

        // GET: Vaccines/Create
        public ActionResult Create()
        {
            ViewBag.Barcode_NDC = new SelectList(db.NDC_Lookup, "Barcode_NDC", "Barcode_NDC");
            return View();
        }

        // POST: Vaccines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Barcode_NDC,Dose,Date_Added,Date_Expire,Price")] Vaccine vaccine)
        {
            if (ModelState.IsValid)
            {
                db.Vaccines.Add(vaccine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Barcode_NDC = new SelectList(db.NDC_Lookup, "Barcode_NDC", "Barcode_NDC", vaccine.Barcode_NDC);
            return View(vaccine);
        }

        // GET: Vaccines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vaccine vaccine = db.Vaccines.Find(id);
            if (vaccine == null)
            {
                return HttpNotFound();
            }
            ViewBag.Barcode_NDC = new SelectList(db.NDC_Lookup, "Barcode_NDC", "Barcode_NDC", vaccine.Barcode_NDC);
            return View(vaccine);
        }

        // POST: Vaccines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Barcode_NDC,Dose,Date_Added,Date_Expire,Price")] Vaccine vaccine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vaccine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Barcode_NDC = new SelectList(db.NDC_Lookup, "Barcode_NDC", "Barcode_NDC", vaccine.Barcode_NDC);
            return View(vaccine);
        }

        // GET: Vaccines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vaccine vaccine = db.Vaccines.Find(id);
            if (vaccine == null)
            {
                return HttpNotFound();
            }
            return View(vaccine);
        }

        // POST: Vaccines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vaccine vaccine = db.Vaccines.Find(id);
            db.Vaccines.Remove(vaccine);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
