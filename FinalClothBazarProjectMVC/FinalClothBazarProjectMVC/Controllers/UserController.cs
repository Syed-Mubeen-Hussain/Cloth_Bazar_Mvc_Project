using FinalClothBazarProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalClothBazarProjectMVC.Controllers
{
    public class UserController : Controller
    {
        cloth_bazarDBEntities1 db = new cloth_bazarDBEntities1();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User u)
        {
            if (ModelState.IsValid)
            {
                var data = db.Users.Where(x => x.u_username == u.u_username && x.u_password == u.u_password).FirstOrDefault();
                if (data != null)
                {
                    Session["u_id"] = data.u_id;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Invalid Username or Password";
                }
            }
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User u)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(u.u_image))
                {
                    u.u_image = "/Content/images/65a68f79-5951-4469-8396-1159a24b8864.png";
                }
                db.Users.Add(u);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.error = "Please Fill All Fields";
            }
            return View();
        }
    }
}