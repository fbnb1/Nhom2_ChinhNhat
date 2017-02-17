using Kidshop.Models.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kidshop.Areas.Admin.Models.BusinessModel
{
    public class GetCategory
    {
        KidShopDbContext db = new KidShopDbContext();
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        //Lấy danh sách tất cả các Category
        public List<GetCategory> getCategory(int id, string level, List<GetCategory> Parent)
        {
            var rs = db.Category.Where(x=>x.ParentId == id).ToList();
            level += "__";
            foreach(var item in rs){
                Parent.Add(new GetCategory() { CategoryId = item.CategoryId, CategoryName = level + item.CategoryName });
                getCategory(item.CategoryId, level, Parent);
            }
            return Parent;
        }
    }
}