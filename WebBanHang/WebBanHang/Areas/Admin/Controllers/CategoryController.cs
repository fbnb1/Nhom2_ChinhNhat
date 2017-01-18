using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
    }
}