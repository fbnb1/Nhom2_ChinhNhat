using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KidShop.Areas.Admin.Models.BusinessModel
{
    public class UniqueUsername : ValidationAttribute
    {
        KidShopDbContext db = new KidShopDbContext();
        protected override ValidationResult IsValid(object username, ValidationContext validationcontext)
        {
            if (db.Account.Any(user => user.FirstName == username.ToString()))
            {
                return new ValidationResult("Tài khoản này đã được đăng ký. Vui lòng thử lại.");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
