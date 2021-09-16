using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class RegisterModel
    {
        [Key]
        public long ID { set; get; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter username")]

        public string UserName { set; get; }

        [Display(Name = "Password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password has at least 6 characters.")]
        [Required(ErrorMessage = "Please enter password")]
        public string Password { set; get; }

        [Display(Name = "Re Password")]
        [Compare("Password", ErrorMessage = "Password was wrong.")]
        public string ConfirmPassword { set; get; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { set; get; }

        [Display(Name = "Address")]
        public string Address { set; get; }

        [Required(ErrorMessage = "Email entry required")]
        [Display(Name = "Email")]
        public string Email { set; get; }

        [Display(Name = "Phone")]
        public string Phone { set; get; }

        [Display(Name = "City")]
        public string ProvinceID { set; get; }


        [Display(Name = "District")]
        public string DistrictID { set; get; }
    }
}