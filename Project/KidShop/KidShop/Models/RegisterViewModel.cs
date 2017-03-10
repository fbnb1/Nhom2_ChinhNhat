using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KidShop.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Họ tên bắt buộc")]
        [Display(Name = "Họ tên")]
        [MaxLength(256)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email bắt buộc")]
        [Display(Name = "Email")]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(64, ErrorMessage = "Mật khẩu phải từ 6 - 64 ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email bắt buộc")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu bắt buộc")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
    }
}