using FinalClothBazarProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalClothBazarProjectMVC.ViewModels
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; }

        public List<Product> Products { get; set; }
    }
}