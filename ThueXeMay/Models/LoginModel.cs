using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ThueXeMay.Models
{
    public class LoginModel
    {
        [Key]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Bạn phải nhập email")]
        public string email { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        public string password { get; set; }
    }
}