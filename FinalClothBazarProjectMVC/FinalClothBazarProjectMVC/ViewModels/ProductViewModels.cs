﻿using FinalClothBazarProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalClothBazarProjectMVC.ViewModels
{
    public class ProductViewModel
    {
        public string searchTerm { get; set; }
        public List<Product> Products { get; set; }
        public Pager page { get; set; }
    }

}