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
    public class RefugeesController : Controller
    {
        private VaccineDBContext db = new VaccineDBContext();

        // GET: Refugees
        public ActionResult Index()
        {
            return View(db.Refugees.ToList());
        }

        // GET: Refugees/Details/5
        [Authorize(Roles = "Admin, Executive, CanEdit, Researcher, Program Staff, Medical Staff")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Refugee refugee = db.Refugees.Find(id);
            if (refugee == null)
            {
                return HttpNotFound();
            }
            return View(refugee);
        }

        // GET: Refugees/Create
        [Authorize(Roles = "Admin, Executive, CanEdit, Program Staff")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Refugees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Executive, CanEdit, Program Staff")]
        public ActionResult Create([Bind(Include = "RefugeeId,Patient_BarCode,Date_Added,FirstName,LastName,Laungage,OriginCountry,Gender")] Refugee refugee)
        {
            if (ModelState.IsValid)
            {
                db.Refugees.Add(refugee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(refugee);
        }

        // GET: Refugees/Edit/5
        [Authorize(Roles = "Admin, Executive, CanEdit, Program Staff")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Refugee refugee = db.Refugees.Find(id);
            if (refugee == null)
            {
                return HttpNotFound();
            }
            return View(refugee);
        }

        // POST: Refugees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Executive, CanEdit, Program Staff")]
        public ActionResult Edit([Bind(Include = "RefugeeId,Patient_BarCode,Date_Added,FirstName,LastName,Laungage,OriginCountry,Gender")] Refugee refugee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(refugee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(refugee);
        }

        // GET: Refugees/Delete/5
        [Authorize(Roles = "Admin, Executive, CanEdit, Program Staff")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Refugee refugee = db.Refugees.Find(id);
            if (refugee == null)
            {
                return HttpNotFound();
            }
            return View(refugee);
        }

        // POST: Refugees/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, Executive, CanEdit, Program Staff")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Refugee refugee = db.Refugees.Find(id);
            db.Refugees.Remove(refugee);
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
