using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assiginment.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter username")]
        public string Username { set; get; }

        [Required(ErrorMessage = "Please enter password")]
        public string Password { set; get; }

        public bool RememberMe { set; get; }
    }
}