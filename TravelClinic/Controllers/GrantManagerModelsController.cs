﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using asp.netmvc5.Models;
using System.IO;
using asp.netmvc5.Models.Helpers;

namespace asp.netmvc5.Controllers
{
    public class GrantManagerModelsController : Controller
    {
        private VaccineDBContext db = new VaccineDBContext();

        // GET: GrantManagerModels
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.GrantManagers.ToList());
        }

        // GET: GrantManagerModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantManagerModel grantManagerModel = db.GrantManagers.Find(id);
            if (grantManagerModel == null)
            {
                return HttpNotFound();
            }
            Session["DetailsGrantID"] = id;
            Session["FileViewType"] = "Download";
            return View(grantManagerModel);
        }

        // GET: GrantManagerModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GrantManagerModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,Grant_Name,Grant_Description,Type")] GrantManagerModel model)
        {
            if (ModelState.IsValid)
            {
                db.GrantManagers.Add(model);
                db.SaveChanges();
               
            }
            foreach (string upload in Request.Files)
            {
                HttpPostedFileBase uploadreq = Request.Files[upload];
                if (uploadreq != null && uploadreq.ContentLength == 0) continue;
                string pathToSave = Server.MapPath("~/Files/");

                DirectoryInfo newDir = Directory.CreateDirectory(pathToSave + model.Grant_Description.ToString()); 
                if (uploadreq != null)
                {
                    string filename = Path.GetFileName(uploadreq.FileName);
                    if (filename != null) uploadreq.SaveAs(Path.Combine(newDir.FullName, filename));

                }
            }

            return RedirectToAction("Index");
        }

        // GET: GrantManagerModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantManagerModel grantManagerModel = db.GrantManagers.Find(id);
            if (grantManagerModel == null)
            {
                return HttpNotFound();
            }
            return View(grantManagerModel);
        }

        // POST: GrantManagerModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Grant_Name,Grant_Description,Type")] GrantManagerModel grantManagerModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grantManagerModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grantManagerModel);
        }

        // GET: GrantManagerModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantManagerModel grantManagerModel = db.GrantManagers.Find(id);
            if (grantManagerModel == null)
            {
                return HttpNotFound();
            }
            Session["DetailsGrantID"] = id;
            Session["FileViewType"] = "Delete";
            return View(grantManagerModel);
        }

        // POST: GrantManagerModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            GrantManagerModel grantManagerModel = db.GrantManagers.Find(id);
            db.GrantManagers.Remove(grantManagerModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GrantFilesPartial(String GrantDescription)
        {
            var files = Directory.GetFiles(Server.MapPath("~/Files/" + GrantDescription));

            var uploadedFiles = files.Select(file => new FileInfo(file)).ToList();

            return PartialView(uploadedFiles);
        }

        public ActionResult FileDelete(int id, String FileName)
        {
            GrantManagerModel grantManagerModel = db.GrantManagers.Find(id);
            var files = Directory.GetFiles(Server.MapPath("~/Files/" + grantManagerModel.Grant_Description));

            foreach (string fileName in files)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Name == FileName)
                {
                    fileInfo.Delete();
                    Session["GrantMessage"] = string.Format("{0} Deleted!", FileName);
                    break;
                }
            }

            return RedirectToAction("Delete", new{id});
        }
        
        public ActionResult FileDownload(int id, String FileName)
        {
            GrantManagerModel grantManagerModel = db.GrantManagers.Find(id);
            var files = Directory.GetFiles(Server.MapPath("~/Files/" + grantManagerModel.Grant_Description));

            foreach (string fileName in files)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Name == FileName)
                    return File(fileName, MimeTypes.GetMimeType(fileInfo.Name), fileInfo.Name);
            }

            return RedirectToAction("Details", new { id });
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
