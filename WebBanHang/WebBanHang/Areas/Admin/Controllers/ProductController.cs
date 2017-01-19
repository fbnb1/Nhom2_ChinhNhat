using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Areas.Admin.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        // GET: Admin/Product
        public ActionResult List()
        {
            AdminDbContext db = new AdminDbContext();
            return View(db.SANPHAMs.ToList());
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}