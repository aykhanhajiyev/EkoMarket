using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoMarketMVC.Identity
{
    public class ContextDb:IdentityDbContext<ApplicationUser>
    {
        public ContextDb():base("dbConnection")
        {

        }
    }
}