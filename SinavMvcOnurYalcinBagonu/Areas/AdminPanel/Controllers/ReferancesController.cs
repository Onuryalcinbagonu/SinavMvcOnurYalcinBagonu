using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SinavMvcOnurYalcinBagonu.Helpers;
using SinavMvcOnurYalcinBagonu.Models;

namespace SinavMvcOnurYalcinBagonu.Areas.AdminPanel.Controllers
{
    public class ReferancesController : Controller
    {
        private Onur_DbEntities db = new Onur_DbEntities();

        // GET: AdminPanel/Referances
        public ActionResult Index()
        {
            return View(db.Referances.ToList());
        }

        // GET: AdminPanel/Referances/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referances referances = db.Referances.Find(id);
            if (referances == null)
            {
                return HttpNotFound();
            }
            return View(referances);
        }

        // GET: AdminPanel/Referances/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminPanel/Referances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RefId,Title,Image,RefName")] Referances referances,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    ImageUpload imageUpload = new ImageUpload();
                    referances.Image = imageUpload.ImageResize(file, 255, 237);
                }
                db.Referances.Add(referances);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(referances);
        }

        // GET: AdminPanel/Referances/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referances referances = db.Referances.Find(id);
            if (referances == null)
            {
                return HttpNotFound();
            }
            return View(referances);
        }

        // POST: AdminPanel/Referances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RefId,Title,Image,RefName")] Referances referances,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                ImageUpload imgUpload = new ImageUpload();
                var edited = db.Referances.Find(referances.RefId);
                if (file != null)
                {
                    //resim yükleme işlemi 
                    edited.Image = imgUpload.ImageResize(file, 240, 240);
                }

                edited.Title = referances.Title;
                edited.RefName = referances.RefName;
               
            
              
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(referances);
        }

        // GET: AdminPanel/Referances/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referances referances = db.Referances.Find(id);
            if (referances == null)
            {
                return HttpNotFound();
            }
            return View(referances);
        }

        // POST: AdminPanel/Referances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Referances referances = db.Referances.Find(id);
            db.Referances.Remove(referances);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult RemoveImage(int? id)
        {
            var refer = db.Referances.Find(id);
            refer.Image = "";
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = id });
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
