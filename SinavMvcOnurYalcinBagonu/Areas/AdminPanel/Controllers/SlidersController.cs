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
    public class SlidersController : Controller
    {
        private Onur_DbEntities db = new Onur_DbEntities();

        // GET: AdminPanel/Sliders
        public ActionResult Index()
        {
            return View(db.Slider.ToList());
        }

        // GET: AdminPanel/Sliders/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: AdminPanel/Sliders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminPanel/Sliders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SliderId,ImageUrl,RegisterDate")] Slider slider, IEnumerable<HttpPostedFileBase> files)
        {
            ImageUpload imageUpload = new ImageUpload();
            if (ModelState.IsValid )
            {
                if (files.FirstOrDefault() != null )//ikinci olarak kontrol yazılır ÇOKLU image için
                {
                    foreach (var item in files)
                    {
                        var image = imageUpload.ImageResize(item, 84, 84, 1140, 475);
                        db.Slider.Add(new  Slider
                        {
                            ImageUrl= image.Item1,
                         
                            RegisterDate = DateTime.Now

                        });
                    }
                }
         
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(slider);
        }

        // GET: AdminPanel/Sliders/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: AdminPanel/Sliders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SliderId,ImageUrl,RegisterDate")] Slider slider, HttpPostedFileBase file)
        {
            ImageUpload imageUpload = new ImageUpload();
            if (ModelState.IsValid )
            {
                if (file != null)//ikinci olarak kontrol yazılır ÇOKLU image için
                {
                  
                        var image = imageUpload.ImageResize(file, 84, 84, 1140, 475);
                    slider.ImageUrl = image.Item1;
                        slider.RegisterDate = DateTime.Now;
                    
                }
                slider.RegisterDate = DateTime.Now;
                db.Entry(slider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        // GET: AdminPanel/Sliders/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Slider.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: AdminPanel/Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Slider slider = db.Slider.Find(id);
            db.Slider.Remove(slider);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult RemoveImage(int? id)
        {
            var slider = db.Slider.Find(id);
            slider.ImageUrl = "";
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
