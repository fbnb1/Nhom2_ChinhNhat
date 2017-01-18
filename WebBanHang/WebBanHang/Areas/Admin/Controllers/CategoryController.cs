using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(DANHMUCSANPHAM abc)
        {
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