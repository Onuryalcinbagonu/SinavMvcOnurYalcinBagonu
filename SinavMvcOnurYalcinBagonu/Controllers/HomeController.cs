using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SinavMvcOnurYalcinBagonu.Models;
namespace SinavMvcOnurYalcinBagonu.Controllers
{
    public class HomeController : Controller
    {
        Onur_DbEntities db = new Onur_DbEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View(db.Home.FirstOrDefault());
        }
        public ActionResult About()
        {
            return View(db.About.FirstOrDefault());
        }
        public ActionResult Referance()
        {
            return View(db.Referances.ToList());
        }
        public ActionResult Contact()
        {
            return View(db.Contact.FirstOrDefault());
        }

        public ActionResult Product()
        {
            return View(db.Categories.ToList());
        }
        public ActionResult Productt(int? id)
        {
            return View(db.Products.Where(a => a.CategoryId == id).ToList());
        }
        public PartialViewResult ListFooter()
        {
            return PartialView(new YenFooter { Footer = db.Footer.ToList() });
        }
        public PartialViewResult ListSlider()
        {
            return PartialView(new YenSlider { Slider = db.Slider.ToList() });
        }
        public PartialViewResult ListHeader()
        {
            return PartialView(new YenHeader
            {
                Home = db.Home.ToList(),
             
            });
        }
    }
}