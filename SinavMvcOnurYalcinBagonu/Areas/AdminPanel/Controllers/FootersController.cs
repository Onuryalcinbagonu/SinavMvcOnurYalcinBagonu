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
    public class FootersController : Controller
    {
        private Onur_DbEntities db = new Onur_DbEntities();

        // GET: AdminPanel/Footers
        public ActionResult Index()
        {
            return View(db.Footer.ToList());
        }

        // GET: AdminPanel/Footers/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footer footer = db.Footer.Find(id);
            if (footer == null)
            {
                return HttpNotFound();
            }
            return View(footer);
        }

        // GET: AdminPanel/Footers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminPanel/Footers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FooterId,SocialMediaActive,FacebookUrl,TwiterUrl,SkypeUrl")] Footer footer)
        {
            if (ModelState.IsValid && db.Footer.Count() == 0)
            {
                string f = "#", t = "#", s = "#";
                if (footer.FacebookUrl == null || footer.FacebookUrl.Contains("http") != true)
                {
                    footer.FacebookUrl = f;
                }
                if (footer.TwiterUrl == null || footer.TwiterUrl.Contains("http") != true)
                {
                    footer.TwiterUrl = t;
                }
                if (footer.SkypeUrl == null || footer.SkypeUrl.Contains("http") != true)
                {
                    footer.SkypeUrl = s;
                }
              
                    ModelState.AddModelError("", "Lütfen Alanları Düzgün Giriniz");
                    db.Footer.Add(footer);
                    db.SaveChanges();
             




                return RedirectToAction("Index");
            }

            return View(footer);
        }

        // GET: AdminPanel/Footers/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footer footer = db.Footer.Find(id);
            if (footer == null)
            {
                return HttpNotFound();
            }
            return View(footer);
        }

        // POST: AdminPanel/Footers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FooterId,SocialMediaActive,FacebookUrl,TwiterUrl,SkypeUrl")] Footer footer)
        {
            if (ModelState.IsValid && db.Footer.Count() == 1)
            {

                string f = "#", t = "#", s = "#";
                if (footer.FacebookUrl == null)
                {
                    footer.FacebookUrl = f;
                }
                if (footer.TwiterUrl == null)
                {
                    footer.TwiterUrl = t;
                }
                if (footer.SkypeUrl == null)
                {
                    footer.SkypeUrl = s;
                }

                if (footer.FacebookUrl.ToString().StartsWith("http")==true)
                {
                    f = footer.FacebookUrl.ToString();
                }
                 if (footer.TwiterUrl.StartsWith("http") == true)
                {
                    t = footer.TwiterUrl.ToString();
                }
                 if (footer.SkypeUrl.StartsWith("http") == true)
                {
                    s = footer.SkypeUrl.ToString();
                }
                footer.FacebookUrl = f;
                footer.TwiterUrl = t;
                footer.SkypeUrl = s;
                db.Entry(footer).State = EntityState.Modified;
                    db.SaveChanges();
              
                return RedirectToAction("Index");
            }
            return View(footer);
        }

        // GET: AdminPanel/Footers/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footer footer = db.Footer.Find(id);
            if (footer == null)
            {
                return HttpNotFound();
            }
            return View(footer);
        }

        // POST: AdminPanel/Footers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Footer footer = db.Footer.Find(id);
            db.Footer.Remove(footer);
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
