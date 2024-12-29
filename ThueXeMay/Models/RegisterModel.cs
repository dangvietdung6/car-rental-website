using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThueXeMay.Models
{
    public class RegisterModel
    {
        [Key]
        public int id_user { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Yêu cầu nhập email")]
        public string email { get; set; }

        [Display(Name = "Mật khẩu")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 ký tự.")]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        public string password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("password", ErrorMessage = "Xác nhận mật khẩu không đúng")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Họ")]
        public string firstName { get; set; }

        [Display(Name = "Tên")]
        public string lastName { get; set; }

        [Display(Name = "Địa chỉ")]
        public string address { get; set; }

        [Display(Name = "Giới tính")]
        public string gender { get; set; }

        [Display(Name = "Số điện thoại")]

        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại")]
        public string phoneNumber { get; set; }


    }

}