using KidShop.Areas.Admin.Models.BusinessModel;
using KidShop.Areas.Admin.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KidShop.Controllers
{
    public class HomeController : Controller
    {
        KidShopDbContext db = new KidShopDbContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult PartialMenu()
        {
            return PartialView(db.Category.Where(r => r.ParentId == 0).ToList());
        }

        public PartialViewResult PartialNewProduct()
        {
            List<Category> ListCategory = db.Category.Where(r => r.ParentId == 0).ToList();
            List<List<Product>> list = new List<List<Product>>();
            foreach (var item in ListCategory)
            {
                List<int> listChildId = Common.findChild(item.CategoryId);
                listChildId.Add(item.CategoryId);
                List<Product> listProduct = db.Product.Where(s => listChildId.Contains((int)s.CategoryId)).OrderByDescending(r => r.CreateDate).Take(4).ToList();
                list.Add(listProduct);
            }
            ViewBag.listProduct = list;
            return PartialView(ListCategory);
        }

        public PartialViewResult PartialSale()
        {
            return PartialView(db.Product.Where(r => r.Sale > 0).Take(8).ToList());
        }
    }
}