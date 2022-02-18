using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalClothBazarProjectMVC.Models;
using FinalClothBazarProjectMVC.ViewModels;

namespace FinalClothBazarProjectMVC.Controllers
{
    public class ShopController : Controller
    {
        cloth_bazarDBEntities1 db = new cloth_bazarDBEntities1();

        public int GetProductsCount(int? sortBy, string search, int? categoryID, int? pageNo, int? maximumPrice)
        {

            pageNo = pageNo.HasValue ? pageNo.Value : 1;

            int pageSize = int.Parse(db.Configurations.Where(x => x.key == "ShopPageSize").FirstOrDefault().value);
            var products = db.Products.ToList();
            if (categoryID.HasValue)
            {
                products = products.Where(x => x.pro_fk_cat == categoryID).OrderByDescending(x => x.pro_id).ToList();
            }

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(x => x.pro_name.ToLower().Contains(search.ToLower())).OrderBy(x => x.pro_id).ToList();
            }

            if (sortBy.HasValue)
            {
                var SortBy = sortBy.Value;
                switch (SortBy)
                {
                    case 2:
                        products = products.OrderByDescending(x => x.pro_id).ToList();
                        break;
                    case 3:
                        products = products.OrderBy(x => x.pro_price).ToList();
                        break;
                    case 4:
                        products = products.OrderByDescending(x => x.pro_price).ToList();
                        break;
                    default:
                        products = products.OrderByDescending(x => x.pro_id).ToList();
                        break;
                }
            }
            else
            {
                products = products.ToList();
            }

            return products.Count();
        }

        public ActionResult Index(int? sortBy, string search, int? categoryID, int? pageNo, int? maximumPrice, int? minimumPrice)
        {
            ShopViewModel model = new ShopViewModel();

            pageNo = pageNo.HasValue ? pageNo.Value : 1;

            model.featuredCategories = db.Categories.Where(x => x.cat_isFeatured && x.cat_image != null).ToList();

            model.MaximumPrice = db.Products.Max(x => x.pro_price);

            int pageSize = int.Parse(db.Configurations.Where(x => x.key == "ShopPageSize").FirstOrDefault().value);

            var products = db.Products.ToList();

            if (categoryID.HasValue)
            {
                products = products.Where(x => x.pro_fk_cat == categoryID).OrderByDescending(x => x.pro_id).ToList();
            }

            if (maximumPrice.HasValue)
            {
                products = products.Where(x => x.pro_price < maximumPrice).ToList();
            }

            if (minimumPrice.HasValue)
            {
                products = products.Where(x => x.pro_price > minimumPrice).ToList();
            }

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(x => x.pro_name.ToLower().Contains(search.ToLower())).OrderBy(x => x.pro_id).ToList();
            }

            if (sortBy.HasValue)
            {
                var SortBy = sortBy.Value;
                switch (SortBy)
                {
                    case 2:
                        products = products.OrderByDescending(x => x.pro_id).ToList();
                        break;
                    case 3:
                        products = products.OrderBy(x => x.pro_price).ToList();
                        break;
                    case 4:
                        products = products.OrderByDescending(x => x.pro_price).ToList();
                        break;
                    default:
                        products = products.OrderByDescending(x => x.pro_id).ToList();
                        break;
                }
            }



            model.sortBy = sortBy;
            model.SearchTerm = search;
            model.categoryID = categoryID;

            model.Products = products.Skip((pageNo.Value - 1) * pageSize).Take(pageSize).ToList();
            var totalProducts = GetProductsCount(sortBy, search, categoryID, pageNo, maximumPrice);
            model.pager = new Pager(totalProducts, pageNo, pageSize);
            return View(model);
        }

        public ActionResult FilterProducts(int? sortBy, string search, int? categoryID, int? pageNo, int? maximumPrice, int? minimumPrice)
        {
            FilterProductViewModel model = new FilterProductViewModel();

            pageNo = pageNo.HasValue ? pageNo.Value : 1;

            int pageSize = int.Parse(db.Configurations.Where(x => x.key == "ShopPageSize").FirstOrDefault().value);

            var products = db.Products.ToList();

            if (categoryID.HasValue)
            {
                products = products.Where(x => x.pro_fk_cat == categoryID).OrderByDescending(x => x.pro_id).ToList();
            }

            if (maximumPrice.HasValue)
            {
                products = products.Where(x => x.pro_price < maximumPrice).ToList();
            }

            if (minimumPrice.HasValue)
            {
                products = products.Where(x => x.pro_price > minimumPrice).ToList();
            }


            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(x => x.pro_name.ToLower().Contains(search.ToLower())).OrderBy(x => x.pro_id).ToList();
            }

            if (sortBy.HasValue)
            {
                var SortBy = sortBy.Value;
                switch (SortBy)
                {
                    case 2:
                        products = products.OrderByDescending(x => x.pro_id).ToList();
                        break;
                    case 3:
                        products = products.OrderBy(x => x.pro_price).ToList();
                        break;
                    case 4:
                        products = products.OrderByDescending(x => x.pro_price).ToList();
                        break;
                    default:
                        products = products.OrderByDescending(x => x.pro_id).ToList();
                        break;
                }
            }

            model.sortBy = sortBy;
            model.SearchTerm = search;
            model.categoryID = categoryID;

            model.Products = products.Skip((pageNo.Value - 1) * pageSize).Take(pageSize).ToList();
            var totalProducts = GetProductsCount(sortBy, search, categoryID, pageNo, maximumPrice);
            model.pager = new Pager(totalProducts, pageNo, pageSize);

            return PartialView(model);
        }

        public ActionResult Checkout()
        {
            CheckoutViewModel model = new CheckoutViewModel();

            if (Session["u_id"] != null)
            {
                var CartProducts = Request.Cookies["CartProducts"];
                if (CartProducts != null)
                {
                    model.Product_ids = CartProducts.Value.Split('-').Select(x => int.Parse(x)).ToList();
                    model.Products = db.Products.Where(x => model.Product_ids.Contains(x.pro_id)).ToList();
                    var id = int.Parse(Session["u_id"].ToString());
                    model.user = db.Users.Where(x => x.u_id == id).FirstOrDefault();
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

            return View();

        }

        public ActionResult PlaceOrder(string productIDs)
        {

            var productIds = productIDs.Split('-').Select(x => int.Parse(x)).ToList();

            var product = db.Products.Where(x => productIds.Contains(x.pro_id)).Distinct().ToList();

            var id = int.Parse(Session["u_id"].ToString());

            Order order = new Order();
            order.o_fk_user = id;
            order.o_ordered_At = DateTime.Now;
            order.o_status = "Pending";
            order.o_totalAmount = product.Sum(x=>x.pro_price * productIds.Where(productid => productid == x.pro_id).Count());

            order.OrderItems.AddRange(product.Select(x => new OrderItem() { oi_fk_pro = x.pro_id, oi_quantity = productIds.Where(a => a == x.pro_id).Count() }));

            db.Orders.Add(order);
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }

    }
}