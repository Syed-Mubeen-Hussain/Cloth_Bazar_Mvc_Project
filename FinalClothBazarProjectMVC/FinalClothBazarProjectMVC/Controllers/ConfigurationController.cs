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
    public class ConfigurationController : Controller
    {
        cloth_bazarDBEntities1 db = new cloth_bazarDBEntities1();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConfigurationTable(string search, int? pageNo)
        {
            ConfigurationViewModel model = new ConfigurationViewModel();

            pageNo = pageNo.HasValue ? pageNo.Value : 1;

            var pageSize = int.Parse(db.Configurations.Where(x => x.key == "ListingPageSize").FirstOrDefault().value);

            var totalItems = db.Configurations.Count();


            if (!string.IsNullOrEmpty(search))
            {
                model.Configurations = db.Configurations.Where(x => x.key.ToLower().Contains(search.ToLower())).OrderBy(x => x.key).Skip((pageNo.Value - 1) * pageSize).Take(pageSize).ToList();
                totalItems = db.Configurations.Where(x => x.key.ToLower().Contains(search.ToLower())).Count();
                model.searchTerm = search;
            }
            else
            {
                model.Configurations = db.Configurations.OrderBy(x => x.key).Skip((pageNo.Value - 1) * pageSize).Take(pageSize).ToList();
            }


            model.page = new Pager(totalItems, pageNo, pageSize);
            

            return PartialView(model);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(Configuration con)
        {
            db.Configurations.Add(con);
            db.SaveChanges();
            return RedirectToAction("ConfigurationTable");
        }

        public ActionResult Edit(string key)
        {
            var data = db.Configurations.Find(key);
            return PartialView(data);
        }

        [HttpPost]
        public ActionResult Edit(Configuration con)
        {
            db.Entry(con).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ConfigurationTable");
        }

        [HttpPost]
        public ActionResult Delete(string key)
        {
            var data = db.Configurations.Find(key);
            db.Entry(data).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("ConfigurationTable");
        }
    }
}