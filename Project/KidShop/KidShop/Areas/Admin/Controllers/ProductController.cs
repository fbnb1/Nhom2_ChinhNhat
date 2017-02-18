using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kidshop.Areas.Admin.Models.DataModel;
using Kidshop.Models.BusinessModel;
using System.IO;

namespace Kidshop.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private KidShopDbContext db = new KidShopDbContext();

        public ActionResult Index()
        {
            var product = db.Product.Include(p => p.Category);
            return View(product.ToList());
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
            return View(product);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Description,CategoryId,Price,Qty,Status")] Product product, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {

                if (Image != null && Image.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Areas/Admin/Content/Images/Product"), Path.GetFileName(Image.FileName));
                    Image.SaveAs(path);
                    product.Image = Image.FileName;
                }

                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        public ActionResult Edit(int? id)
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
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Description,CategoryId,Price,Image,Qty,Status")] Product product, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                //Nếu người dùng thay đổi Image
                if (Image != null && Image.ContentLength > 0)
                {
                    //Lấy tên file image cũ
                    KidShopDbContext db2 = new KidShopDbContext();
                    var currentImage = db2.Product.FirstOrDefault(x => x.ProductId == product.ProductId).Image;
                    db2.Dispose();

                    //Xóa image cũ
                    string fullpath = Request.MapPath("~/Areas/Admin/Content/Images/Product/" + currentImage);
                    if (System.IO.File.Exists(fullpath))
                    {
                        System.IO.File.Delete(fullpath);
                    }

                    //Lưu file image mới vào folder
                    string path = Path.Combine(Server.MapPath("~/Areas/Admin/Content/Images/Product"), Path.GetFileName(Image.FileName));
                    Image.SaveAs(path);
                    product.Image = Image.FileName;
                }
                else
                {
                    //Nếu người dùng ko chọn Image thì Image sẽ giữ nguyên ko thay đổi
                    db.Entry(product).Property(e => e.Image).IsModified = false;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        public ActionResult Delete(int id)
        {
            Product product = db.Product.Find(id);

            //Xóa file image
            string fullpath = Request.MapPath("~/Areas/Admin/Content/Images/Product/" + product.Image);
            if (System.IO.File.Exists(fullpath))
            {
                System.IO.File.Delete(fullpath);
            }

            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
