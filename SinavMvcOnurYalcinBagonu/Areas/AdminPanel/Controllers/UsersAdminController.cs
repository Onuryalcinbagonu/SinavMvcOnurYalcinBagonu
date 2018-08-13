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
    public class UsersAdminController : Controller
    {
        private Onur_DbEntities db = new Onur_DbEntities();


        // GET: AdminPanel/UsersAdmin
        public ActionResult Index()
        {
            return View(db.User.ToList());
        }

        // GET: AdminPanel/UsersAdmin/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: AdminPanel/UsersAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminPanel/UsersAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,User_Email,Password,Register_Date")] User user)
        {
            if (ModelState.IsValid)
            {
                if (db.User.Any(p => p.User_Email.Contains(user.User_Email)) == true)
                {
                    return RedirectToAction("Index");
                }
                user.Register_Date = DateTime.Now;
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: AdminPanel/UsersAdmin/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: AdminPanel/UsersAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,User_Email,Password,Register_Date")] User user)
        {
            if (ModelState.IsValid)
            {
                if (db.User.Any(p => p.User_Email.Contains(user.User_Email)) != true)
                {
                   
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                }
                else if (db.User.FirstOrDefault(p => p.UserId == user.UserId).Password != user.Password)
                {
                    db.User.FirstOrDefault(p => p.UserId == user.UserId).Password = user.Password.TrimEnd();
                    db.SaveChanges();

                }
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: AdminPanel/UsersAdmin/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: AdminPanel/UsersAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            User user = db.User.Find(id);
            if ( db.User.Count() > 1)
            {
                db.User.Remove(user);
                db.SaveChanges();
            }
           
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
