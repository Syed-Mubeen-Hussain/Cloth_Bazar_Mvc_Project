using FinalClothBazarProjectMVC.Models;
using FinalClothBazarProjectMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalClothBazarProjectMVC.Controllers
{
    public class ProductController : Controller
    {
        cloth_bazarDBEntities1 db = new cloth_bazarDBEntities1();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductTable(string search, int? pageNo)
        {
            ProductViewModel model = new ProductViewModel();

            pageNo = pageNo.HasValue ? pageNo.Value : 1;

            var pageSize = int.Parse(db.Configurations.Where(x => x.key == "ListingPageSize").FirstOrDefault().value);

            var totalItems = db.Products.Count();


            if (!string.IsNullOrEmpty(search))
            {
                model.Products = db.Products.Where(x => x.pro_name.ToLower().Contains(search.ToLower())).OrderBy(x => x.pro_id).Skip((pageNo.Value - 1) * pageSize).Take(pageSize).ToList();
                totalItems = db.Products.Where(x => x.pro_name.ToLower().Contains(search.ToLower())).Count();
                model.searchTerm = search;
            }
            else
            {
                model.Products = db.Products.OrderBy(x => x.pro_id).Skip((pageNo.Value - 1) * pageSize).Take(pageSize).ToList();
            }


            model.page = new Pager(totalItems, pageNo, pageSize);

            ViewBag.categories = db.Categories.ToList();

            return PartialView(model);
        }

        public ActionResult Create()
        {
            var cat_list = db.Categories.ToList();
            ViewBag.categories = new SelectList(cat_list, "cat_id", "cat_name");
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(Product pro)
        {
            var cat_list = db.Categories.ToList();
            ViewBag.categories = new SelectList(cat_list, "cat_id", "cat_name");

            if (string.IsNullOrEmpty(pro.pro_image))
            {
                pro.pro_image = "/Content/images/87d4b9ea-653b-458d-a6f9-68ac6c2f33d5.png";
            }
            db.Products.Add(pro);
            db.SaveChanges();
            return RedirectToAction("ProductTable");
        }

        public ActionResult Edit(int Id)
        {
            var cat_list = db.Categories.ToList();
            ViewBag.categories = new SelectList(cat_list, "cat_id", "cat_name");
            var data = db.Products.Find(Id);
            return PartialView(data);
        }

        [HttpPost]
        public ActionResult Edit(Product pro)
        {
            var cat_list = db.Categories.ToList();
            ViewBag.categories = new SelectList(cat_list, "cat_id", "cat_name");

            var p = db.Products.Where(x => x.pro_id == pro.pro_id).FirstOrDefault();
            p.pro_id = pro.pro_id;
            p.pro_name = pro.pro_name;
            p.pro_price = pro.pro_price;
            p.pro_description = pro.pro_description;

            if (!string.IsNullOrEmpty(pro.pro_image))
            {
                p.pro_image = pro.pro_image;
            }


            p.pro_fk_cat = pro.pro_fk_cat;
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ProductTable");
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            var data = db.Products.Find(Id);
            db.Entry(data).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("ProductTable");
        }

        public ActionResult SingleProduct(int id)
        {
            var data = db.Products.Where(x => x.pro_id == id).FirstOrDefault();
            var cat = db.Categories.Where(x =>x.cat_id == data.pro_fk_cat).FirstOrDefault();
            ViewBag.category = cat;

            var relatedPro = db.Products.Where(x => x.pro_fk_cat == cat.cat_id).Take(4).ToList();
            ViewBag.relatedProducts = relatedPro;


            return View(data);
        }
    }
}