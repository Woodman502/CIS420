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
    [Authorize(Roles = "Admin, Executive, CanEdit, Researcher, Program Staff, Medical Staff")]
    public class VaccinesController : Controller
    {
        private VaccineDBContext db = new VaccineDBContext();

        // GET: Vaccines
        public ActionResult Index()
        {
            var vaccines = db.Vaccines.Include(v => v.NDC_Lookup);
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
        [Authorize(Roles = "Admin, Executive, CanEdit, Researcher, Program Staff, Medical Staff")]
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
        [Authorize(Roles = "Admin, Executive, CanEdit, Program Staff")]
        public ActionResult Create()
        {
            ViewBag.Barcode_NDC = new SelectList(db.NDC_Lookup, "Barcode_NDC", "Barcode_NDC");
            return View();
        }

        // POST: Vaccines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Executive, CanEdit, Program Staff")]
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
        [Authorize(Roles = "Admin, Executive, CanEdit, Program Staff")]
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
        [Authorize(Roles = "Admin, Executive, CanEdit, Program Staff")]
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
        [Authorize(Roles = "Admin, Executive, CanEdit, Program Staff")]
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
        [Authorize(Roles = "Admin, Executive, CanEdit, Program Staff")]
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
