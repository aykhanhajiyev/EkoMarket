using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcoMarketMVC.Identity
{
    public class LoginModel
    {
        [Required(ErrorMessage ="İstifadəçi adı daxil edin")]
        public string Username { get; set; }
        [Required(ErrorMessage ="Şifrəni daxil edin")]
        public string Password { get; set; }
    }
}