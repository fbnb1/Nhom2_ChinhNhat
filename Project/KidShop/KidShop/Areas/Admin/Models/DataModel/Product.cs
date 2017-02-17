using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kidshop.Areas.Admin.Models.DataModel
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Display(Name="Sản phẩm")]
        [Column(TypeName="nvarchar")]
        [StringLength(256)]
        [Required(ErrorMessage="Vui lòng nhập tên sản phẩm")]
        public string ProductName { get; set; }

        [Display(Name = "Thông tin")]
        [Column(TypeName = "nvarchar")]
        [StringLength(1000)]
        public string Description { get; set; }

        [Display(Name = "Nhóm hàng")]
        public int? CategoryId { get; set; }

        [Display(Name = "Giá")]
        public double Price { get; set; }

        [Display(Name = "Số lượng")]
        public int Qty { get; set; }

        [Display(Name="Hình ảnh")]
        public string Image { get; set; }

        [Display(Name="Ẩn/Hiện")]
        public bool Status { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}