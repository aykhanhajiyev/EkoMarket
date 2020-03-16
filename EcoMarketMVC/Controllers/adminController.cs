using EcoMarketMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EcoMarketMVC.Controllers
{
    [Authorize]
    public class adminController : Controller
    {
        private DataContext db;
        public adminController()
        {
            db = new DataContext();
        }
        // GET: admin
        public async Task<ActionResult> index()
        {
            return View(await db.Categories.OrderByDescending(i => i.Id).ToListAsync());
        }
        [HttpGet]
        public async Task<ActionResult> newcategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> newcategory(Category category)
        {
            db.Categories.Add(category);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat əlavə edildi.";
            return RedirectToAction("index");
        }
        [HttpGet]
        public async Task<ActionResult> editcategory(int? id)
        {
            if (id == null) return HttpNotFound();
            var category = await db.Categories.FindAsync(id);
            if (category == null) return HttpNotFound();
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> editcategory(int? id, Category model)
        {
            if (id == null) return HttpNotFound();
            var category = await db.Categories.FindAsync(id);
            if (category == null) return HttpNotFound();
            category.Name = model.Name;
            TempData["Success"] = "Məlumat düzəlişi edildi.";
            await db.SaveChangesAsync();
            return RedirectToAction("index");
        }
        public async Task<ActionResult> deletecategory(int? id)
        {
            if (id == null) return HttpNotFound();
            var category = await db.Categories.FindAsync(id);
            if (category == null) return HttpNotFound();
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat silindi.";
            return RedirectToAction("index");
        }
        //SUBCATEGORIES
        public async Task<ActionResult> subcategories()
        {
            return View(await db.Subcategories.Include(c => c.Category).OrderByDescending(i => i.Id).ToListAsync());
        }
        [HttpGet]
        public async Task<ActionResult> newsubcategory()
        {
            ViewBag.Categories = await db.Categories.OrderBy(i => i.Name).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> newsubcategory(Subcategory subcategory,HttpPostedFileBase file)
        {
            if (subcategory == null)
            {
                TempData["Error"] = "Xəta baş verdi.";
                return View();
            }
            if (file == null)
            {
                TempData["Error"] += "\nŞəkil xətası baş verdi.";
                return View();
            }
            if(file!=null && file.ContentLength > 0)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/assetsadmin/images/subcategory"), pic);
                file.SaveAs(path);
                subcategory.ImagePath = file.FileName;
            }
            db.Subcategories.Add(subcategory);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat əlavə edildi.";
            return RedirectToAction("subcategories");
        }
        [HttpGet]
        public async Task<ActionResult> editsubcategory(int? id)
        {
            if (id == null) return HttpNotFound();
            var subcategory = await db.Subcategories.FindAsync(id);
            if (subcategory == null) return HttpNotFound();
            ViewBag.Categories = await db.Categories.OrderBy(i => i.Name).ToListAsync();
            return View(subcategory);
        }
        [HttpPost]
        public async Task<ActionResult> editsubcategory(int? id, Subcategory model)
        {
            if (id == null) return HttpNotFound();
            var subcategory = await db.Subcategories.FindAsync(id);
            if (subcategory == null) return HttpNotFound();
            subcategory.Name = model.Name;
            subcategory.CategoryId = model.CategoryId;
            TempData["Success"] = "Məlumat düzəlişi edildi.";
            await db.SaveChangesAsync();
            return RedirectToAction("subcategories");
        }
        public async Task<ActionResult> deletesubcategory(int? id)
        {
            if (id == null) return HttpNotFound();
            var subcategory = await db.Subcategories.FindAsync(id);
            if (subcategory == null) return HttpNotFound();
            db.Subcategories.Remove(subcategory);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat silindi.";
            return RedirectToAction("subcategories");
        }
        //Products
        public async Task<ActionResult> products()
        {
            ViewBag.Categories = await db.Categories.ToListAsync();
            return View(await db.Products.Include(s=>s.Subcategory).ToListAsync());
        }
        [HttpGet]
        public async Task<ActionResult> newproduct()
        {
            ViewBag.Categories = await db.Categories.OrderBy(i => i.Name).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> newproduct(Product product,HttpPostedFileBase files)
        {
            if (product == null || files==null )
            {
                TempData["Error"] = "Xəta baş verdi.";
                return View();
            }
            string pic = System.IO.Path.GetFileName(files.FileName);
            string path = System.IO.Path.Combine(Server.MapPath("~/assets/upload_images"), pic);
            files.SaveAs(path);
            product.ImageUrl = files.FileName;
            db.Products.Add(product);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat əlavə edildi.";
            return RedirectToAction("products");
        }
        //Return Json Get By Subcategory
        public async Task<ActionResult> GetSubcategoriesByCategory(int? id)
        {
            if (id == null) return HttpNotFound();
            List<CategorySubCategoryJson> json = new List<CategorySubCategoryJson>();
            foreach(var item in await db.Subcategories.Where(i => i.CategoryId == id).ToListAsync())
            {
                CategorySubCategoryJson model = new CategorySubCategoryJson()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                json.Add(model);
            }   
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> editproduct(int? id)
        {
            if (id == null) return HttpNotFound();
            var product = await db.Products.FindAsync(id);
            if (product == null) return HttpNotFound();
            ViewBag.Categories = await db.Categories.OrderBy(i => i.Name).ToListAsync();
            return View(product);
        }
        [HttpPost]
        public async Task<ActionResult> editproduct(int? id, Product model,HttpPostedFileBase files)
        {
            if (id == null) return HttpNotFound();
            var product = await db.Products.FindAsync(id);
            if (product == null) return HttpNotFound();
            if (files!=null && files.ContentLength>0)
            {
                string pic = System.IO.Path.GetFileName(files.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/assets/upload_images"), pic);
                files.SaveAs(path);
                product.ImageUrl = files.FileName;
            }
            product.Name = model.Name;
            product.Price = model.Price;
            product.Description = model.Description;
            product.IsHot = model.IsHot;
            product.IsSale = model.IsSale;
            product.SubcategoryId = model.SubcategoryId;
            TempData["Success"] = "Məlumat düzəlişi edildi.";
            await db.SaveChangesAsync();
            return RedirectToAction("products");
        }
        public async Task<ActionResult> deleteproduct(int? id)
        {
            if (id == null) return HttpNotFound();
            var product = await db.Products.FindAsync(id);
            if (product == null) return HttpNotFound();
            db.Products.Remove(product);
            string path = System.IO.Path.Combine(Server.MapPath("~/assets/upload_images"), product.ImageUrl);
            System.IO.File.Delete(path);
            await db.SaveChangesAsync();
            TempData["Success"] = "Məlumat silindi.";
            return RedirectToAction("products");
        }
    }
}