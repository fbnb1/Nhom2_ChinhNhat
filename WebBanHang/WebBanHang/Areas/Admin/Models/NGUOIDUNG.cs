namespace WebBanHang.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NGUOIDUNG")]
    public partial class NGUOIDUNG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NGUOIDUNG()
        {
            HOADONs = new HashSet<HOADON>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Tendangnhap { get; set; }

        [StringLength(50)]
        public string Matkhau { get; set; }

        [StringLength(150)]
        public string Ten { get; set; }

        [StringLength(300)]
        public string Diachi { get; set; }

        [StringLength(16)]
        public string Sodienthoai { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Ngaysinh { get; set; }

        public short? Gioitinh { get; set; }

        public int? Quyen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOADON> HOADONs { get; set; }
    }
}
