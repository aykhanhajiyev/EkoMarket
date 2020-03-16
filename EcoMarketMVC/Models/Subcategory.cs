using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcoMarketMVC.Models
{
    public class Subcategory
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        [StringLength(100)]
        public string ImagePath { get; set; }
        public Category Category { get; set; }
        public int ProductId { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}