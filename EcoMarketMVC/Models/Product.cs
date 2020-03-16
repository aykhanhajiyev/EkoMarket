using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcoMarketMVC.Models
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(300)]
        public string Name { get; set; }
        [MaxLength(300)]
        public string ImageUrl { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public bool IsHot { get; set; }
        public bool IsSale { get; set; }
        public decimal Price { get; set; }
        public int SubcategoryId { get; set; }
        public virtual Subcategory Subcategory { get; set; }

    }
}