using EcoMarketMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EcoMarketMVC.Controllers
{
    public class homeController : Controller
    {
        private DataContext db;
        public homeController()
        {
            db = new DataContext();
            ViewBag.Categories = db.Categories.OrderBy(i => i.Name).ToList();
            var list = (List<Cart>)System.Web.HttpContext.Current.Session["Cart"];
            if (list != null)
            {
                ViewData["CountOfCart"] = list.Count;
                ViewData["SumOfCart"] = list.Sum(i => i.Product.Price * i.Quantity);
            }
            else
            {
                ViewData["CountOfCart"] = 0;
                ViewData["SumOfCart"] = 0;
            }
        }
        public async Task<ActionResult> index()
        {
            return View(await db.Products.OrderByDescending(i => i.Id).Take(5).ToListAsync());
        }
        public async Task<ActionResult> category(int? id)
        {
            if (id == null) return HttpNotFound();
            var category = await db.Categories.FindAsync(id);
            if (category == null) return HttpNotFound();
            var subcategories = db.Subcategories.Where(i => i.CategoryId == id);
            var subcategories_id = await subcategories.Select(i => i.Id).ToListAsync();
            List<Product> products = new List<Product>();
            for (int i = 0; i < subcategories_id.Count; i++)
            {
                List<Product> temp = new List<Product>();
                int count = subcategories_id[i];
                temp = await db.Products.Where(m => m.SubcategoryId == count).ToListAsync();
                products.AddRange(temp);
            }
            SubCategoryProductViewModel model = new SubCategoryProductViewModel
            {
                Subcategories = subcategories.Include(s => s.Category),
                Products = products
            };
            return View(model);
        }
        public async Task<ActionResult> products(int? id, string q)
        {
            if (q != null)
            {
                switch (q)
                {
                    case "gununfurseti":
                        ViewBag.Q = "Günün Fürsəti";
                        ViewBag.ActiveQ1 = "active";
                        return View(await db.Products.Where(i => i.IsHot == true).ToListAsync());
                    case "sizinucun":
                        ViewBag.Q = "Sizin Üçün";
                        ViewBag.ActiveQ2 = "active";
                        return View(await db.Products.OrderBy(i => i.Id).Take(8).ToListAsync());
                    case "munasibqiymetler":
                        ViewBag.ActiveQ3 = "active";
                        ViewBag.Q = "Münasib Qiymətlər";
                        return View(await db.Products.Where(i => i.IsSale == true).ToListAsync());
                    default: return View(await db.Products.OrderByDescending(i => i.Id).ToListAsync());
                }
            }
            else
            {
                if (id == null) return HttpNotFound();
                var subcategory = await db.Subcategories.FindAsync(id);
                if (subcategory == null) return HttpNotFound();
                ViewBag.CategoryId = db.Categories.FirstOrDefault(i => i.Id == subcategory.CategoryId).Id;
                ViewBag.CategoryName = db.Categories.FirstOrDefault(i => i.Id == subcategory.CategoryId).Name;
                return View(await db.Products.Include(s => s.Subcategory).Where(i => i.SubcategoryId == id).ToListAsync());
            }
        }
        //Product Details
        public async Task<ActionResult> product(int? id)
        {
            if (id == null) return HttpNotFound();
            var product = await db.Products.FindAsync(id);
            if (product == null) return HttpNotFound();
            var subcategory = await db.Subcategories.FindAsync(product.SubcategoryId);
            ViewBag.CategoryId = db.Categories.FirstOrDefault(i => i.Id == subcategory.CategoryId).Id;
            ViewBag.CategoryName = db.Categories.FirstOrDefault(i => i.Id == subcategory.CategoryId).Name;
            //Send true enabling styles 
            ViewBag.StyleofProduct = true;
            return View(product);
        }
        public async Task<ActionResult> shoppingcart()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> addtocart(int? id)
        {
            if (id == null) return HttpNotFound();
            if (Session["Cart"] == null)
            {
                var product = await db.Products.FindAsync(id);
                List<Cart> lscart = new List<Cart>() { new Cart(product, 1) };
                Session["Cart"] = lscart;
            }
            else
            {
                var prod = await db.Products.FindAsync(id);
                List<Cart> lscart = (List<Cart>)Session["Cart"];
                if (IsExistingProduct(id) == -1)
                {
                    lscart.Add(new Cart(prod,1));
                }
                else
                {
                    lscart[IsExistingProduct(id)].Quantity++;
                }
                Session["Cart"] = lscart;
            }
            return RedirectToAction("shoppingcart");
        }
        [NonAction]
        private int IsExistingProduct(int? id)
        {
            if (id == null) return -2;
            List<Cart> products = (List<Cart>)Session["Cart"];
            for(int i = 0; i < products.Count; i++)
            {
                if (products[i].Product.Id == id) return i;
            }
            return -1;
        }
        public async Task<ActionResult> deletefromshoppingcart(int? id)
        {
            List<Cart> lscart = (List<Cart>)Session["Cart"];
            var prod = lscart.Where(i => i.Product.Id == id).FirstOrDefault();
            if (prod != null)
            {
                lscart.Remove(prod);
                Session["Cart"] = lscart;
                return RedirectToAction("shoppingcart","home");
            }
            else
            {
                return HttpNotFound();
            }
            
        }
    }
}