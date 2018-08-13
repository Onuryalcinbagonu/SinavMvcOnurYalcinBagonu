using SinavMvcOnurYalcinBagonu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace SinavMvcOnurYalcinBagonu.Areas.AdminPanel.Controllers
{
    public class UsersController : Controller
    {
        private Onur_DbEntities db = new Onur_DbEntities();
   
        // GPasswordET: AdminPanel/Users
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(User user)
        {

         
            var login = db.User.Where(u => u.User_Email == user.User_Email).SingleOrDefault();
            var email = Session["user_email"];
            var Password = Session["user_password"];
            if (login == null)
            {
                ModelState.AddModelError("", "Lütfen Doğru Bir Email veya Şifre Giriniz");
                return View(user);
            }

            if (email == null && Password == null)
            {
                email = login.User_Email;
                Password= login.Password;
            }

            if ( login==null)
            {
                ModelState.AddModelError("", "Lütfen E-mail / Şifre Kontrol Deneyiniz");
                return View(user);
            }
            else if (email.ToString() == login.User_Email && Password.ToString() == login.Password.TrimEnd())
            {
                Session["user_email"] = login.User_Email;
                Session["user_password"] = login.Password;
                email = login.User_Email;
                Password = login.Password;
                return RedirectToAction("Index", "Homes");
            }
            else if (email.ToString() != null && email.ToString() != login.User_Email)
            {
                ModelState.AddModelError("", "Lütfen 20 DK Sonra Tekrar Deneyiniz");
                return View(user);
            }
            else if (login.User_Email == user.User_Email && login.Password.TrimEnd() == user.Password  )
            {
                
                   
                Session["user_email"] = login.User_Email;
                Session["user_password"] = login.Password;
                email = login.User_Email;
                Password = login.Password;
                db.User.Where(u=>u.UserId==login.UserId).First().Register_Date= DateTime.Now;
                db.SaveChanges();
               

                return RedirectToAction("Index", "Homes");
            }
            else
            {
                ModelState.AddModelError("", "Lütfen E-mail / Şifre Kontrol Deneyiniz");
                return View(user);
            }
            
        }
        [HttpGet]
        public ActionResult Logout()
        {

            Session.Abandon();
            return RedirectToAction("Index", "Users");
        }


        [HttpPost]
        public ActionResult PasswordNull(User user)
        {


            var login = db.User.Where(u => u.User_Email == user.User_Email).SingleOrDefault();

            
            if (user.User_Email == "null")
            {
                return View();
            }
            else
            {

            foreach (var item in db.User.ToList())
            {
                if (item.User_Email == user.User_Email)
                {
                        MailMessage mesgm = new MailMessage(); //Mesajı türetiyoruz yani miras alıyoruz kütüphaneden
                        mesgm.Subject = ("ŞİFRE HATIRLATMA");//Mesaj Konusu

                        mesgm.From = new MailAddress("uzmansistem74@gmail.com"); //Kendi Mail Adresimiz girilerek gönderen kişi belirlenir
                        mesgm.To.Add(user.User_Email); //Mesajı Göndereceğimiz Kişinin Emaili email.Txt Textbox'ına Aktarılarak Gönderiliyor
                        mesgm.Body = user.User_Email + "  Şifreniz = "  + item.Password ; //Mesaj

                        SmtpClient server = new SmtpClient("smtp.gmail.com"); //Kendi mailimiz için mail serverımızı yazıyoruz
                        server.Credentials = new System.Net.NetworkCredential("uzmansistem74@gmail.com", "uzmansistem7474"); // Mailimizin Kullanıcı Adı ve Şifresi Server Girişi İçin Gereklidir
                        server.Port = 587;//Servere Bağlantımız İçin  Gmail Port Giriş Numarasını Giriyoruz
                        server.EnableSsl = true; //SMTP posta sunucusuna erişmek için SSL kullanılıp kullanılmayacağını belirtir.
                        server.Send(mesgm);//Promosyon Mesajı Gönderildi

                        ModelState.AddModelError("", "Email Adresinize Şifre Gönderilmiştir ");
                        return View();
                    }
                  
                }
            }
            ModelState.AddModelError("", "Lütfen Doğru Bir Email veya Şifre Giriniz");
            return View();

        }


        [HttpGet]
        public ActionResult PasswordNull()
        {                 
            return View();
        }
    }
}