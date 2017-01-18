namespace WebBanHang.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("THONGTINCUAHANG")]
    public partial class THONGTINCUAHANG
    {
        [Key]
        [StringLength(150)]
        public string Ten { get; set; }

        [StringLength(16)]
        public string Dienthoai { get; set; }

        [StringLength(50)]
        public string Website { get; set; }

        [StringLength(300)]
        public string Diachi { get; set; }
    }
}
