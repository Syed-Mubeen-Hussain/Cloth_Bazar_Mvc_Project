using FinalClothBazarProjectMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalClothBazarProjectMVC.Models;
using System.Data.Entity;

namespace FinalClothBazarProjectMVC.Controllers
{
    public class OrderController : Controller
    {
        cloth_bazarDBEntities1 db = new cloth_bazarDBEntities1();

        public ActionResult Index(string status, int? pageNo)
        {
            OrdersViewModel model = new OrdersViewModel();
            model.Status = status;

            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            var pageSize = 5;

            var orders = db.Orders.ToList();
            

            if (!string.IsNullOrEmpty(status))
            {
                orders = orders.Where(x => x.o_status.ToLower().Contains(status.ToLower())).ToList();
            }
            model.Orders = orders.OrderByDescending(x=>x.o_id).Skip((pageNo.Value - 1) * pageSize).Take(pageSize).ToList();
            var totalRecords = orders.Count();

            model.Pager = new Pager(totalRecords, pageNo, pageSize);

            return View(model);
        }

        public ActionResult Details(int ID)
        {
            OrderDetailsViewModel model = new OrderDetailsViewModel();

            model.Order = db.Orders.Where(x => x.o_id == ID).FirstOrDefault();
            

            model.AvailableStatuses = new List<string>() { "Pending", "In Progress", "Delivered" };

            return View(model);
        }
        public bool UpdateOrderStatus(int ID,string status)
        {
            var order = db.Orders.Find(ID);

            order.o_status = status;

            db.Entry(order).State = EntityState.Modified;

            return db.SaveChanges() > 0;
        }
        public JsonResult ChangeStatus(string status, int ID)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            result.Data = new { Success = UpdateOrderStatus(ID, status) };

            return result;
        }

    }
}