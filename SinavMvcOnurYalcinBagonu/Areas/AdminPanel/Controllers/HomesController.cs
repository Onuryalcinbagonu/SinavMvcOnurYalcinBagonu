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
    public class HomesController : Controller
    {
        private Onur_DbEntities db = new Onur_DbEntities();

        // GET: AdminPanel/Homes
        public ActionResult Index()
        {
            return View(db.Home.ToList());
        }

        // GET: AdminPanel/Homes/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Home home = db.Home.Find(id);
            if (home == null)
            {
                return HttpNotFound();
            }
            return View(home);
        }

        // GET: AdminPanel/Homes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminPanel/Homes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HomeId,Name,DescriptionOneTitle,DescriptionOne,DescriptionOneUrl,DescriptionTwoTitle,DescriptionTwo")] Home home, HttpPostedFileBase file, string editor1, string editor2)
        {
            if (ModelState.IsValid && db.Home.Count() == 0)
            {
                if (file != null)
                {
                    ImageUpload imageUpload = new ImageUpload();
                    home.LogoImgUrl = imageUpload.ImageResize(file, 255, 237);
                }
                home.DescriptionOne = editor1;
                home.DescriptionTwo = editor2;
                db.Home.Add(home);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(home);
        }

        // GET: AdminPanel/Homes/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Home home = db.Home.Find(id);
            if (home == null)
            {
                return HttpNotFound();
            }
            return View(home);
        }

        // POST: AdminPanel/Homes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]//ckeditör için  html kod doğrulama isteği

        public ActionResult Edit([Bind(Include = "HomeId,LogoImgUrl,DescriptionOneTitle,DescriptionOne,DescriptionTwoTitle,DescriptionTwo")] Home home, HttpPostedFileBase file,string editor1, string editor2)
        {
            if (ModelState.IsValid && db.Home.Count() == 1)
            {
                ImageUpload imgUpload = new ImageUpload();
                var editedModel = db.Home.Find(home.HomeId);
                if (file != null)
                {
                    //resim yükleme işlemi 
                    editedModel.LogoImgUrl = imgUpload.ImageResize(file, 240, 240);

                }



                editedModel.DescriptionOne = editor1;
                editedModel.DescriptionOneTitle = home.DescriptionOneTitle.ToString();
                editedModel.DescriptionTwoTitle = home.DescriptionTwoTitle.ToString();
                editedModel.DescriptionTwo = editor2;
   

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(home);
        }

        // GET: AdminPanel/Homes/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Home home = db.Home.Find(id);
            if (home == null)
            {
                return HttpNotFound();
            }
            return View(home);
        }

        // POST: AdminPanel/Homes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            Home home = db.Home.Find(id);
            db.Home.Remove(home);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult RemoveImage(int? id)
        {

            var home = db.Home.Find(id);
            home.LogoImgUrl = "";
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
