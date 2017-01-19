using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Areas.Admin.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public ActionResult List()
        {
            AdminDbContext db = new AdminDbContext();
            return View(db.DANHMUCSANPHAMs.ToList());
        }

        [HttpGet]
        public ActionResult Add()
        {
            AdminDbContext db = new AdminDbContext();
            ViewBag.ListCate = db.DANHMUCSANPHAMs.Select(g=>g.Ten).ToList();
            return View();
        }

        public string Alias(string str)
        {
            str = str.Trim();
            str = str.ToLower();
            str = str.Replace("  ", " ");
            str = str.Replace(" ", "-");
            str = str.Replace("â", "a");
            str = str.Replace("ă", "a");
            str = str.Replace("á", "a");
            str = str.Replace("ạ", "a");
            str = str.Replace("ã", "a");
            str = str.Replace("à", "a");
            str = str.Replace("ả", "a");
            str = str.Replace("ấ", "a");
            str = str.Replace("ầ", "a");
            str = str.Replace("ậ", "a");
            str = str.Replace("ẩ", "a");
            str = str.Replace("ẫ", "a");
            str = str.Replace("ằ", "a");
            str = str.Replace("ắ", "a");
            str = str.Replace("ặ", "a");
            str = str.Replace("ẳ", "a");
            str = str.Replace("ẵ", "a");
            str = str.Replace("è", "e");
            str = str.Replace("é", "e");
            str = str.Replace("ẽ", "e");
            str = str.Replace("ẻ", "e");
            str = str.Replace("ẹ", "e");
            str = str.Replace("ê", "e");
            str = str.Replace("ế", "e");
            str = str.Replace("ề", "e");
            str = str.Replace("ễ", "e");
            str = str.Replace("ể", "e");
            str = str.Replace("ệ", "e");
            str = str.Replace("ô", "o");
            str = str.Replace("ơ", "o");
            str = str.Replace("ó", "o");
            str = str.Replace("ò", "o");
            str = str.Replace("õ", "o");
            str = str.Replace("ỏ", "o");
            str = str.Replace("ọ", "o");
            str = str.Replace("ố", "o");
            str = str.Replace("ồ", "o");
            str = str.Replace("ộ", "o");
            str = str.Replace("ỗ", "o");
            str = str.Replace("ổ", "o");
            str = str.Replace("ớ", "o");
            str = str.Replace("ờ", "o");
            str = str.Replace("ở", "o");
            str = str.Replace("ỡ", "o");
            str = str.Replace("ợ", "o");
            str = str.Replace("đ", "d");
            str = str.Replace("ú", "u");
            str = str.Replace("ù", "u");
            str = str.Replace("ủ", "u");
            str = str.Replace("ũ", "u");
            str = str.Replace("ụ", "u");
            str = str.Replace("í", "i");
            str = str.Replace("ì", "i");
            str = str.Replace("ỉ", "i");
            str = str.Replace("ĩ", "i");
            str = str.Replace("ị", "i");
            str = str.Replace("!", "");
            str = str.Replace("@", "");
            str = str.Replace("#", "");
            str = str.Replace("$", "");
            str = str.Replace("%", "");
            str = str.Replace("^", "");
            str = str.Replace("&", "");
            str = str.Replace("*", "");
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            str = str.Replace("_", "");
            str = str.Replace("+", "");
            str = str.Replace("=", "");
            str = str.Replace("`", "");
            str = str.Replace("~", "");
            str = str.Replace("\\", "");
            str = str.Replace("|", "");
            str = str.Replace("/", "");
            str = str.Replace("'", "");
            str = str.Replace("\"", "");
            str = str.Replace(".", "");
            str = str.Replace(">", "");
            str = str.Replace(",", "");
            str = str.Replace("<", "");
            str = str.Replace("?", "");
            str = str.Replace(":", "");
            str = str.Replace(";", "");
            return str;
        }

        [HttpPost]
        public ActionResult Add(DANHMUCSANPHAM abc)
        {
            abc.Alias = Alias(abc.Ten);
            AdminDbContext db = new AdminDbContext();
            db.DANHMUCSANPHAMs.Add(abc);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult Delete(int Id)
        {
            AdminDbContext db = new AdminDbContext();
            DANHMUCSANPHAM a = db.DANHMUCSANPHAMs.Find(Id);
            db.DANHMUCSANPHAMs.Remove(a);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public ActionResult Edit(int Id)
        {
            AdminDbContext db = new AdminDbContext();
            DANHMUCSANPHAM Data = db.DANHMUCSANPHAMs.FirstOrDefault(r => r.Id == Id);
            ViewBag.ListCate = db.DANHMUCSANPHAMs.Select(g => g.Ten).ToList();
            return View(Data);
        }
        
        [HttpPost]
        public ActionResult Edit(DANHMUCSANPHAM abc)
        {
            AdminDbContext db = new AdminDbContext();
            db.Entry(abc).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("List");
        }
    }
}