using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SinavMvcOnurYalcinBagonu.Models;

namespace SinavMvcOnurYalcinBagonu.Areas.AdminPanel.Controllers
{
    public class AboutsController : Controller
    {
        private Onur_DbEntities db = new Onur_DbEntities();

        // GET: AdminPanel/Abouts
        public ActionResult Index()
        {
            return View(db.About.ToList());
        }

        // GET: AdminPanel/Abouts/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.About.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        // GET: AdminPanel/Abouts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminPanel/Abouts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]//ckeditör için  html kod doğrulama isteği
        public ActionResult Create([Bind(Include = "AboutId,AboutTitle,Description")] About about,string editor1)
        {
            if (ModelState.IsValid && db.About.Count()==0)
            {
                about.Description = editor1;
                db.About.Add(about);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
          
            return View(about);
        }

        // GET: AdminPanel/Abouts/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.About.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        // POST: AdminPanel/Abouts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]//ckeditör için  html kod doğrulama isteği
        public ActionResult Edit([Bind(Include = "AboutId,AboutTitle,Description")] About about,string editor1)
        {
           
            if (ModelState.IsValid && db.About.Count() ==1 )
            {
                about.Description = editor1;
                db.Entry(about).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(about);
        }

        // GET: AdminPanel/Abouts/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.About.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        // POST: AdminPanel/Abouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            About about = db.About.Find(id);
            db.About.Remove(about);
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
