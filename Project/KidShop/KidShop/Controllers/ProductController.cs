using KidShop.Areas.Admin.Models.BusinessModel;
using KidShop.Areas.Admin.Models.DataModel;
using KidShop.Areas.Admin.Models.ViewModel;
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
        public ActionResult Index(int? id, string searchString, string sortOrder, int page = 1)
        {
            int categoryId = id ?? 0;
            

            var products = from s in db.Product select s;

            //tất cả các danh mục id có id cha là categoryId
            List<int> categoryIds = KidShop.Controllers.Common.findChild(categoryId);
            categoryIds.Add(categoryId);

            products = products.Where(s => categoryIds.Contains((int)s.CategoryId));

            if (!String.IsNullOrEmpty(searchString))
            {
                products = db.Product.Where(p => p.ProductName.ToLower().Contains(searchString));
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

                default:  // Date ascending 
                    products = products.OrderByDescending(s => s.CreateDate);
                    break;
            }

            
            int pageSize = 12;

            if (Request.IsAjaxRequest())
            {
                return (ActionResult)PartialView("ProductList", products.ToPagedList(page, pageSize));
            }
            else
            {
                Category category = new Category();
                if (categoryId == 0)
                {
                    category.CategoryId = 0;
                    category.CategoryName = "Tất cả sản phẩm";
                }
                else
                {
                    category = db.Category.Find(categoryId);
                    if (category == null)
                    {
                        return HttpNotFound();
                    }
                }
                ViewBag.Category = category;
                return View(products.ToPagedList(page, pageSize));
            }
        }


        public PartialViewResult ProductList(int? id, string sortOrder, int? page)
        {
            int categoryId = id ?? 0;
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "date-desc" : sortOrder;

            var products = from s in db.Product select s;
            List<int> listChildId = KidShop.Controllers.Common.findChild(categoryId);
            products = products.Where(s => listChildId.Contains((int)s.CategoryId));

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
            return PartialView(products.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductDetail productDetail = db.ProductDetail.Find(id);
            if (productDetail == null)
            {
                return HttpNotFound();
            }

            List<string> listColor = db.ProductDetail.Where(r => r.ProductId == productDetail.ProductId).Select(r => r.Color).Distinct().ToList();
            ViewBag.ListColor = listColor;

            List<string> listSize = db.ProductDetail.Where(r => r.ProductId == productDetail.ProductId && r.Color == productDetail.Color).Select(r => r.Size).Distinct().ToList();
            ViewBag.listSize = listSize;

            //sản phẩm liên quan
            var ListProduct = db.Product.Where(r => r.CategoryId == productDetail.Product.CategoryId && r.ProductId != productDetail.ProductId).Take(4).ToList();
            ViewBag.ListProduct = ListProduct;
            
            return View(productDetail);
        }

        [HttpPost]
        public ActionResult ChangeColor(int id, string color)
        {
            List<ProductDetail> list = db.ProductDetail.Where(r => r.ProductId == id && r.Color == color).ToList();
            if(list.Count() != 0)
            {
                var productDetail = list.FirstOrDefault();
                var results = new ColorSizeViewModel
                {
                    listSize = list.Select(r => r.Size).Distinct().ToList(),
                    productDetailId = productDetail.ProductDetailId,
                    OldPrice = productDetail.Price.ToString(),
                    SalePrime = (productDetail.Product.Sale > 0) ? (productDetail.Price - (productDetail.Price * productDetail.Product.Sale / 100)).ToString() : productDetail.Price.ToString(),
                    StatusQty = (productDetail.Qty>0)?"Còn hàng":"Hết hàng"
                };
                return Json(results);
            }
            return null;
        }

        [HttpPost]
        public ActionResult ChangeSize(int id, string color, string size)
        {
            ProductDetail productDetail = db.ProductDetail.FirstOrDefault(r => r.ProductId == id && r.Color == color&&r.Size == size);
            if (productDetail != null)
            {
                var results = new ColorSizeViewModel
                {
                    productDetailId = productDetail.ProductDetailId,
                    OldPrice = productDetail.Price.ToString(),
                    SalePrime = (productDetail.Product.Sale > 0) ? (productDetail.Price - (productDetail.Price * productDetail.Product.Sale / 100)).ToString() : productDetail.Price.ToString(),
                    StatusQty = (productDetail.Qty > 0) ? "Còn hàng" : "Hết hàng"
                };
                return Json(results);
            }
            return null;
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