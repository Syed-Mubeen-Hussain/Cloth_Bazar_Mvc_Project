using FinalClothBazarProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalClothBazarProjectMVC.ViewModels
{
    public class OrdersViewModel
    {
        public List<Order> Orders { get; set; }
        public Pager Pager { get; set; }
        public string Status { get; set; }
        public int UserID { get; set; }
    }
    public class OrderDetailsViewModel
    {
        public List<string> AvailableStatuses { get; set; }
        public Order Order { get; set; }
    }
}