using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoMarketMVC.Models
{
    public class SubCategoryProductViewModel
    {
        public IEnumerable<Subcategory> Subcategories { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}