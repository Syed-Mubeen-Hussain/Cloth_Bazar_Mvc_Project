using FinalClothBazarProjectMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalClothBazarProjectMVC.Models;

namespace FinalClothBazarProjectMVC.Controllers
{
    public class HomeController : Controller
    {
        cloth_bazarDBEntities1 db = new cloth_bazarDBEntities1();

        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            model.Categories = db.Categories.Where(x => x.cat_isFeatured == true && x.cat_image != null).OrderByDescending(x=>x.cat_id).ToList();
            
            return View(model);
        }
        public ActionResult Widget(int NumberOfProducts)
        {
            var data = db.Products.OrderByDescending(x => x.pro_id).Take(NumberOfProducts).ToList();
            return PartialView(data);
        }
    }
}