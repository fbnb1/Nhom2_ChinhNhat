using KidShop.Areas.Admin.Models.BusinessModel;
using KidShop.Areas.Admin.Models.DataModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KidShop.Controllers
{
    public class ProductController : Controller
    {
        private KidShopDbContext db = new KidShopDbContext();
        // GET: Product
        public ActionResult Index(int? id, string sortOrder, int? page)
        {
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "date-desc" : sortOrder;
            ViewBag.CurrentSort = sortOrder;

            var products = from s in db.Product
                           select s;

            int categoryId = id ?? 0;
            ViewBag.CategoryId = id;


            if (id != null) // id = null thi lay tat ca san pham
            {
                if (id == 0) // id = 0 la san pham khuyen mai
                {
                    products = products.Where(s => (int)s.Sale != 0);
                    ViewBag.CategoryName = "Khuyến mại";
                }
                else
                {
                    Category c = db.Category.Find(id);
                    if (c == null)
                    {
                        return null;
                    }
                    else
                    {
                        List<int> listChildId = Common.findChild(categoryId);
                        listChildId.Add(categoryId);
                        products = products.Where(s => listChildId.Contains((int)s.CategoryId));
                        ViewBag.CategoryName = c.CategoryName;
                    }
                }
            }
            else
            {
                ViewBag.CategoryName = "Tất cả sản phẩm";
            }

            switch (sortOrder)
            {
                case "price":
                    products = products.OrderBy(s => s.Price);
                    break;

                case "price-desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;

                case "name":
                    products = products.OrderBy(s => s.ProductName);
                    break;
                case "name-desc":
                    products = products.OrderByDescending(s => s.ProductName);
                    break;

                case "date":
                    products = products.OrderBy(s => s.CreateDate);
                    break;

                case "date-desc":
                    products = products.OrderByDescending(s => s.CreateDate);
                    break;

                default:  // Date ascending 
                    products = products.OrderByDescending(s => s.CreateDate);
                    break;
            }

            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var list = db.ProductDetail.Where(r => r.ProductId == id);
            ViewBag.ListSizeColor = list.ToList();

            List<string> listSize = list.GroupBy(r => r.Size).Select(r => r.Key).Distinct().ToList();
            ViewBag.ListSize = listSize;

            List<string> listColor = list.GroupBy(r => r.Color).Select(r => r.Key).Distinct().ToList();
            ViewBag.ListColor = listColor;

            ViewBag.ListProduct = db.Product.Where(r => r.CategoryId == product.CategoryId).Take(4).ToList();
            return View(product);
        }

        public PartialViewResult PartialProductUrl(int? id, int? product)
        {
            List<Category> list = new List<Category>();
            if (id == null)
            {
                ViewBag.tt = "Tất cả sản phẩm";
            }
            else if (id == 0)
            {
                ViewBag.tt = "Khuyến mại";
            }
            else
            {
                int categoryId = 0;
                if (product == null)
                {
                    categoryId = id ?? 0;
                }
                else
                {
                    Product p = db.Product.Find(id);
                    if (p != null)
                    {
                        ViewBag.tt = p.ProductName;
                        categoryId = (int)p.CategoryId;
                    }
                }
                while (true)
                {
                    Category c = db.Category.Find(categoryId);
                    if (c != null)
                    {
                        list.Add(c);
                        categoryId = (int)c.ParentId;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return PartialView(list);
        }

        public PartialViewResult PartialCategory(int? id)
        {
            if (id != null || id != 0)
            {
                Category cat = db.Category.Find(id);
                if (cat != null)
                {
                    var listChild = db.Category.Where(r => r.ParentId == id);
                    if (listChild.Count() != 0)
                    {
                        ViewBag.ParentName = cat.CategoryName;
                        return PartialView(listChild.ToList());
                    }
                }
            }
            ViewBag.ParentName = "Danh Mục";
            return PartialView(db.Category.Where(r => r.ParentId == 0).ToList());
        }

        public PartialViewResult PartialProductImage(int? id)
        {
            int productId = id ?? 0;
            var image = db.ProductImage.Where(r => r.ProductId == productId);
            return PartialView(image.ToList());
        }


    }
}