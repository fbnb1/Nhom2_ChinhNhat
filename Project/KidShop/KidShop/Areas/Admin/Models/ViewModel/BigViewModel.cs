using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kidshop.Areas.Admin.Models.DataModel;

namespace Kidshop.Areas.Admin.Models.ViewModel
{
    public class BigViewModel
    {
        public Product Product { get; set; }
        public ProductDetail ProductDetail { get; set; }
    }
}