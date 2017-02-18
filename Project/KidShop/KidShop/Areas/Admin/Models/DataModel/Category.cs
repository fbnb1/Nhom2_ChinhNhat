﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kidshop.Areas.Admin.Models.DataModel
{
    [Table("Category")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Display(Name="Nhóm hàng")]
        [Column(TypeName="nvarchar")]
        [StringLength(150)]
        public string CategoryName { get; set; }

        [Column(TypeName="varchar")]
        [StringLength(150)]
        public string Alias { get; set; }

        [Display(Name = "Mô tả")]
        [Column(TypeName = "nvarchar")]
        [StringLength(300)]
        public string Description { get; set; }

        [Display(Name = "Thứ tự")]
        public int? Order { get; set; }

        [Display(Name = "Ẩn/Hiện")]
        public bool Status { get; set; }

        [Display(Name="Nhóm hàng cha")]
        public int? ParentId { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? CreateDate { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}