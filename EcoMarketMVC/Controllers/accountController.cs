using EcoMarketMVC.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EcoMarketMVC.Controllers
{
    public class accountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        public accountController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ContextDb()));
            var user = userManager.FindByName("parviz.jafarov");
            if (user == null)
            {
                ApplicationUser appuser = new ApplicationUser
                {
                    Name = "Parviz Jafarov",
                    UserName = "parviz.jafarov",
                };
                userManager.Create(appuser, "parviz557");
            }
        }
        // GET: account
        public ActionResult index()
        {
            return View("login");
        }
        [HttpGet]
        public async Task<ActionResult> login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/admin/index");
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> login(LoginModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindAsync(model.Username, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "İstifadəçi adı və yaxud şifrə yanlışdır.");
                }
                else
                {
                    var authManager = HttpContext.GetOwinContext().Authentication;
                    var identity = await userManager.CreateIdentityAsync(user, "ApplicationCookie");
                    var authPropties = new AuthenticationProperties()
                    {
                        IsPersistent = true
                    };
                    authManager.SignOut();
                    authManager.SignIn(authPropties, identity);
                    return Redirect(string.IsNullOrEmpty(returnUrl) ? "/admin/index" : returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }
        public ActionResult logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("login");
        }
    }
}