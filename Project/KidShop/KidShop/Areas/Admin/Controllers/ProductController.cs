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
using Kidshop.Areas.Admin.Models.ViewModel;

namespace Kidshop.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private KidShopDbContext db = new KidShopDbContext();
        static List<string> fileNameTemp = new List<string>();
        public List<HttpPostedFileBase> fileUpload = new List<HttpPostedFileBase>();


        /*--------------------------------INDEX-----------------------------------*/
        public ActionResult Index()
        {
            var product = db.Product.Include(p => p.Category);
            return View(product.ToList());
        }


        /*--------------------------------CREATE-GET-----------------------------------*/
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName");
            return View();
        }


        /*--------------------------------CREATE-POST-----------------------------------*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Description,CategoryId,Price,Qty,Status")] Product product, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                //Lưu ảnh đại diện sản phẩm
                if (Image != null && Image.ContentLength > 0)
                {
                    string extensionFile = Image.FileName.Substring(Image.FileName.LastIndexOf("."));
                    string newfilename = Common.EncryptMD5(DateTime.Now.ToBinary().ToString()) + extensionFile;
                    string path = Path.Combine(Server.MapPath("~/Areas/Admin/Content/Images/Product"), newfilename);
                    Image.SaveAs(path);
                    product.Image = newfilename;
                }

                //Lưu ảnh detail sản phẩm vào server
                List<string> listFileName = new List<string>();
                if (Session["fileUpload"] != null)
                {
                    fileUpload = (List<HttpPostedFileBase>)Session["fileUpload"];
                    foreach (var item in fileUpload)
                    {
                        if (item != null && item.ContentLength > 0)
                        {
                            string extensionFile2 = item.FileName.Substring(item.FileName.LastIndexOf("."));
                            string newfilename2 = Common.EncryptMD5(DateTime.Now.ToBinary().ToString()) + extensionFile2;
                            string path2 = Path.Combine(Server.MapPath("~/Areas/Admin/Content/Images/ProductImages"), newfilename2);
                            item.SaveAs(path2);
                            listFileName.Add(newfilename2);
                        }
                    }
                }
                Session["fileUpload"] = null;
                db.Product.Add(product);
                db.SaveChanges();

                //Lấy id sản phẩm
                int lastProductId = db.Product.Max(x => x.ProductId);

                //Lưu ảnh detail sản phẩm vào CSDL
                foreach (var item in listFileName)
                {
                    ProductImage a = new ProductImage();
                    a.ProductId = lastProductId;
                    a.ImageName = item;
                    db.ProductImage.Add(a);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }


        /*--------------------------------EDIT-GET-----------------------------------*/
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


        /*--------------------------------EDIT-POST-----------------------------------*/
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

                //Lưu ảnh detail sản phẩm
                List<string> listFileName = new List<string>();
                if (Session["fileUpload"] != null)
                {
                    fileUpload = (List<HttpPostedFileBase>)Session["fileUpload"];
                    foreach (var item in fileUpload)
                    {
                        if (item != null && item.ContentLength > 0)
                        {
                            string extensionFile2 = item.FileName.Substring(item.FileName.LastIndexOf("."));
                            string newfilename2 = Common.EncryptMD5(DateTime.Now.ToBinary().ToString()) + extensionFile2;
                            db.ProductImage.Add(new ProductImage()
                            {
                                 ImageName = newfilename2,
                                 ProductId = product.ProductId
                            });
                            string path2 = Path.Combine(Server.MapPath("~/Areas/Admin/Content/Images/ProductImages"), newfilename2);
                            item.SaveAs(path2);
                            listFileName.Add(newfilename2);
                        }
                    }
                }
                Session["fileUpload"] = null;

                //Xóa ảnh detail người dùng xóa
                if (fileNameTemp != null)
                {
                    foreach (var item in fileNameTemp)
                    {
                        var rs = db.ProductImage.FirstOrDefault(x => x.ImageName == item);
                        if (rs != null)
                        {
                            db.ProductImage.Remove(rs);
                        }
                        string fullpath = Request.MapPath("~/Areas/Admin/Content/Images/ProductImages/" + item);
                        if (System.IO.File.Exists(fullpath))
                        {
                            System.IO.File.Delete(fullpath);
                        }
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }


        /*--------------------------------DELETE-----------------------------------*/
        public bool Delete(int id)
        {
            //Xóa file image detail
            var find = db.ProductImage.Where(x => x.ProductId == id).ToList();
            foreach (var item in find)
            {
                string path = Request.MapPath("~/Areas/Admin/Content/Images/ProductImages/" + item.ImageName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            
            //Xóa file image
            Product product = db.Product.Find(id);
            string fullpath = Request.MapPath("~/Areas/Admin/Content/Images/Product/" + product.Image);
            if (System.IO.File.Exists(fullpath))
            {
                System.IO.File.Delete(fullpath);
            }
            db.Product.Remove(product);
            db.SaveChanges();
            return true;
        }


        // UploadFile function
        // Mỗi khi chọn ảnh tại DropzoneJS, function này sẽ đc thực thi
        // và lưu vào Session['fileUpload'], List fileUpload
        // Khi nào người dùng Submit form thì mới thực lưu file vào CSDL
        [HttpPost]
        public void UploadFile()
        {
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                if (file != null && file.ContentLength > 0)
                {
                    if (Session["fileUpload"] == null)
                    {
                        fileUpload.Add(file);
                    }
                    else
                    {
                        fileUpload = (List<HttpPostedFileBase>)Session["fileUpload"];
                        fileUpload.Add(file);
                    }
                }
            }
            Session["fileUpload"] = fileUpload;
        }


        // Tại View Create nếu người dùng xóa bỏ file image ko muốn upload nữa (DropzoneJS)
        // thì function này sẽ đc thực thi
        // loại bỏ file đó ra khỏi List<HttpPostFileBase> fileUpload, Session['fileUpload']
        public bool DeleteFileImage(string id)
        {
            fileUpload = (List<HttpPostedFileBase>)Session["fileUpload"];
            foreach (var file in fileUpload)
            {
                if (file.FileName == id)
                {
                    fileUpload.Remove(file);
                    break;
                }
            }
            Session["fileUpload"] = fileUpload;
            return true;
        }


        // Tại View Edit nếu người dùng muốn xóa file image (DropzoneJS)
        // Ta sẽ lưu tên file vào List<string> fileNameTemp
        // Khi người dùng submit Lưu thì mới xóa file ra khỏi CSDL, server
        public void DeleteFileImage_Edit(string id)
        {
            fileNameTemp.Add(id);
        }


        // Method này dùng để lấy tất cả tên ảnh detail của ProductId truyền vào
        // Return Json
        // Dùng để hiển thị trong DropzoneJS, View Edit
        public ActionResult GetListImageDetail(int id)
        {
            var model = db.ProductImage.Where(x => x.ProductId == id).ToList();
            return Json(new { Data = model.Select(x => new { FileName = x.ImageName, ProductImageId = x.ProductImageId }) }, JsonRequestBehavior.AllowGet);
        }


        // Method này đc sử dụng tại View Edit và Create
        // Dùng để xóa dữ liệu Session['fileUpload'], List fileUpload khi người dùng chuyển trang
        public void Load()
        {
            fileUpload = null;
            Session["fileUpload"] = null;
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
