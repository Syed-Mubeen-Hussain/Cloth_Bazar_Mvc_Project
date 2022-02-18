using FinalClothBazarProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using FinalClothBazarProjectMVC.ViewModels;

namespace FinalClothBazarProjectMVC.Controllers
{
    public class CategoryController : Controller
    {
        cloth_bazarDBEntities1 db = new cloth_bazarDBEntities1();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CategoryTable(string search,int? pageNo)
        {
            CategoryViewModel model = new CategoryViewModel();

            pageNo = pageNo.HasValue ? pageNo.Value : 1;

            var pageSize = int.Parse(db.Configurations.Where(x => x.key == "ListingPageSize").FirstOrDefault().value);

            var totalItems = db.Categories.Count();


            if (!string.IsNullOrEmpty(search))
            {
                model.Category = db.Categories.Where(x => x.cat_name.ToLower().Contains(search.ToLower())).OrderBy(x=>x.cat_id).Skip((pageNo.Value - 1)*pageSize).Take(pageSize).ToList();
                totalItems = db.Categories.Where(x => x.cat_name.ToLower().Contains(search.ToLower())).Count();
                model.searchTerm = search;
            }
            else
            {
                model.Category = db.Categories.OrderBy(x=>x.cat_id).Skip((pageNo.Value - 1) * pageSize).Take(pageSize).ToList();
            }


            model.page = new Pager(totalItems, pageNo, pageSize);

            return PartialView(model);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(Category cat)
        {
            if (string.IsNullOrEmpty(cat.cat_image))
            {
                cat.cat_image = "/Content/images/87d4b9ea-653b-458d-a6f9-68ac6c2f33d5.png";
            }
            db.Categories.Add(cat);
            db.SaveChanges();
            return RedirectToAction("CategoryTable");
        }

        public ActionResult Edit(int Id)
        {
            var data = db.Categories.Find(Id);
            return PartialView(data);
        }

        [HttpPost]
        public ActionResult Edit(Category cat)
        {
            var c = db.Categories.Where(x => x.cat_id == cat.cat_id).FirstOrDefault();
            c.cat_id = cat.cat_id;
            c.cat_name = cat.cat_name;
            c.cat_description = cat.cat_description;

            if (!string.IsNullOrEmpty(cat.cat_image))
            {
                c.cat_image = cat.cat_image;
            }


            c.cat_isFeatured = cat.cat_isFeatured;
            db.Entry(c).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("CategoryTable");
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            var data = db.Categories.Find(Id);
            db.Entry(data).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("CategoryTable");
        }
    }
}