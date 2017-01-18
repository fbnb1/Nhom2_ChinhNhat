namespace WebBanHang.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DANHMUCSANPHAM")]
    public partial class DANHMUCSANPHAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DANHMUCSANPHAM()
        {
            SANPHAMs = new HashSet<SANPHAM>();
        }

        public int Id { get; set; }

        [StringLength(150)]
        public string Ten { get; set; }

        [StringLength(150)]
        public string Alias { get; set; }

        [StringLength(300)]
        public string Ghichu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SANPHAM> SANPHAMs { get; set; }
    }
}
