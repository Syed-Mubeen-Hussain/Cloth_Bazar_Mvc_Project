using FinalClothBazarProjectMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalClothBazarProjectMVC.ViewModels
{
    public class ConfigurationViewModel
    {
            public string searchTerm { get; set; }
            public List<Configuration> Configurations { get; set; }
            public Pager page { get; set; }
    }
}