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
    public class ProductsController : Controller
    {
        private Onur_DbEntities db = new Onur_DbEntities();

        // GET: AdminPanel/Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Categories);
            return View(products.ToList());
        }

        // GET: AdminPanel/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: AdminPanel/Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: AdminPanel/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]//ckeditör için  html kod doğrulama isteği

        public ActionResult Create([Bind(Include = "ProductId,CategoryId,Name,Price,Description,Image")] Products products, HttpPostedFileBase file, string editor1)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    ImageUpload imageUpload = new ImageUpload();
                    products.Image = imageUpload.ImageResize(file, 255, 237);
                }
                products.Description = editor1;
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", products.CategoryId);
            return View(products);
        }

        // GET: AdminPanel/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", products.CategoryId);
            return View(products);
        }

        // POST: AdminPanel/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]//ckeditör için  html kod doğrulama isteği

        public ActionResult Edit([Bind(Include = "ProductId,CategoryId,Name,Price,Description,Image")] Products products,HttpPostedFileBase file,string editor1)
        {
            if (ModelState.IsValid)
            {

                ImageUpload imgUpload = new ImageUpload();
                var product = db.Products.Find(products.ProductId);
                if (file != null)
                {
                    //resim yükleme işlemi 
                   product.Image = imgUpload.ImageResize(file, 240, 240);
                }

                product.Categories = products.Categories;
                product.CategoryId = products.CategoryId;
                product.Description = editor1;
                product.Name = products.Name;
                product.Price = products.Price;

           
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", products.CategoryId);
            return View(products);
        }

        // GET: AdminPanel/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: AdminPanel/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RemoveImage(int? id)
        {
            var products = db.Products.Find(id);
            products.Image = "";
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
