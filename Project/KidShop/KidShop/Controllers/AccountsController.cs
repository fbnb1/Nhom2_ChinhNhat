using KidShop.Areas.Admin.Models.BusinessModel;
using KidShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KidShop.Controllers
{
    public class AccountsController : Controller
    {
        private KidShopDbContext db = new KidShopDbContext();

        public ActionResult Index()
        {
            return View();
        }

        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async ActionResult Register(RegisterViewModel register)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (db.Users.SingleOrDefault(r => r.userName == register.Email) == null)
        //        {
        //            bool ad = false;
        //            if (db.Users.Count() == 0)
        //            {
        //                ad = true;
        //            }
        //            string passwordMD5 = KidShop.Areas.Admin.Models.BusinessModel.Common.EncryptMD5(register.UserName + register.Password);
        //            User user = new User { userName = register.UserName, password = passwordMD5, fullName = register.FullName, email = register.Email, admin = ad };
        //            db.Users.Add(user);
        //            db.SaveChanges();
        //            return View("RegisterThanhCong");
        //        }
        //        ViewBag.error = "Tên đang nhập đã tồn tại";
        //    }
        //    return View(register);
        //}
    }
}
