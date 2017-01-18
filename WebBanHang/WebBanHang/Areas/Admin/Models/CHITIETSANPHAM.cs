namespace WebBanHang.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CHITIETSANPHAM")]
    public partial class CHITIETSANPHAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CHITIETSANPHAM()
        {
            CHITIETHOADONs = new HashSet<CHITIETHOADON>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? Size { get; set; }

        [StringLength(50)]
        public string Mau { get; set; }

        public int? Id_sanpham { get; set; }

        public int? Soluong { get; set; }

        public int? Gia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETHOADON> CHITIETHOADONs { get; set; }

        public virtual SANPHAM SANPHAM { get; set; }
    }
}
