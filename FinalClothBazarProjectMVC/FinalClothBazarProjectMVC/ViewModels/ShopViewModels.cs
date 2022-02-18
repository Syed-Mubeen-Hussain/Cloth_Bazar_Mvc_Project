using FinalClothBazarProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalClothBazarProjectMVC.ViewModels
{
    public class CheckoutViewModel
    {
        public User user { get; set; }
        public List<int> Product_ids { get; set; }
        public List<Product> Products { get; set; }
        public int quantity { get; set; }
    }

    public class ShopViewModel
    {
        public List<Category> featuredCategories { get; set; }
        public List<Product> Products { get; set; }
        public int? sortBy { get; set; }
        public int? categoryID { get; set; }
        public string SearchTerm { get; set; }
        public Pager pager { get; set; }
        public int? MaximumPrice { get; set; }
    }

    public class FilterProductViewModel
    {
        public List<Product> Products { get; set; }
        public Pager pager { get; set; }
        public int? categoryID { get; set; }
        public int? sortBy { get; set; }
        public string SearchTerm { get; set; }
    }

}