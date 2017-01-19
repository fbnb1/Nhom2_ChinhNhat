namespace WebBanHang.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SANPHAM")]
    public partial class SANPHAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SANPHAM()
        {
            CHITIETSANPHAMs = new HashSet<CHITIETSANPHAM>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        
        [Display(Name="Tên sản phẩm")]
        [StringLength(150)]
        public string Ten { get; set; }

        [StringLength(150)]
        public string Alias { get; set; }

        [Display(Name="Đơn vị tính")]
        [StringLength(50)]
        public string Donvitinh { get; set; }

        [Display(Name = "Hình ảnh")]
        [StringLength(150)]
        public string Image { get; set; }

        [Display(Name = "Thông tin sản phẩm")]
        public string Thongtin_sp { get; set; }

        [Display(Name = "Nhà cung cấp")]
        public int? Id_ncc { get; set; }

        [Display(Name = "Danh mục")]
        public int? Id_danhmuc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETSANPHAM> CHITIETSANPHAMs { get; set; }

        public virtual DANHMUCSANPHAM DANHMUCSANPHAM { get; set; }

        public virtual NHACUNGCAP NHACUNGCAP { get; set; }
    }
}
