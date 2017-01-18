namespace WebBanHang.Areas.Admin.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AdminDbContext : DbContext
    {
        public AdminDbContext()
            : base("name=AdminDbContext")
        {
        }

        public virtual DbSet<CHITIETHOADON> CHITIETHOADONs { get; set; }
        public virtual DbSet<CHITIETSANPHAM> CHITIETSANPHAMs { get; set; }
        public virtual DbSet<DANHMUCSANPHAM> DANHMUCSANPHAMs { get; set; }
        public virtual DbSet<HOADON> HOADONs { get; set; }
        public virtual DbSet<KHACHHANG> KHACHHANGs { get; set; }
        public virtual DbSet<NGUOIDUNG> NGUOIDUNGs { get; set; }
        public virtual DbSet<NHACUNGCAP> NHACUNGCAPs { get; set; }
        public virtual DbSet<SANPHAM> SANPHAMs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<THONGTINCUAHANG> THONGTINCUAHANGs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CHITIETSANPHAM>()
                .Property(e => e.Mau)
                .IsFixedLength();

            modelBuilder.Entity<CHITIETSANPHAM>()
                .HasMany(e => e.CHITIETHOADONs)
                .WithRequired(e => e.CHITIETSANPHAM)
                .HasForeignKey(e => e.Id_chitietsp)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DANHMUCSANPHAM>()
                .Property(e => e.Alias)
                .IsFixedLength();

            modelBuilder.Entity<DANHMUCSANPHAM>()
                .HasMany(e => e.SANPHAMs)
                .WithOptional(e => e.DANHMUCSANPHAM)
                .HasForeignKey(e => e.Id_danhmuc);

            modelBuilder.Entity<HOADON>()
                .HasMany(e => e.CHITIETHOADONs)
                .WithRequired(e => e.HOADON)
                .HasForeignKey(e => e.Id_hoadon)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.Sodienthoai)
                .IsFixedLength();

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.Email)
                .IsFixedLength();

            modelBuilder.Entity<KHACHHANG>()
                .HasMany(e => e.HOADONs)
                .WithOptional(e => e.KHACHHANG)
                .HasForeignKey(e => e.Id_khachhang);

            modelBuilder.Entity<NGUOIDUNG>()
                .Property(e => e.Tendangnhap)
                .IsFixedLength();

            modelBuilder.Entity<NGUOIDUNG>()
                .Property(e => e.Matkhau)
                .IsFixedLength();

            modelBuilder.Entity<NGUOIDUNG>()
                .Property(e => e.Sodienthoai)
                .IsFixedLength();

            modelBuilder.Entity<NGUOIDUNG>()
                .HasMany(e => e.HOADONs)
                .WithOptional(e => e.NGUOIDUNG)
                .HasForeignKey(e => e.Id_nguoidung);

            modelBuilder.Entity<NHACUNGCAP>()
                .Property(e => e.Sodienthoai)
                .IsFixedLength();

            modelBuilder.Entity<NHACUNGCAP>()
                .HasMany(e => e.SANPHAMs)
                .WithOptional(e => e.NHACUNGCAP)
                .HasForeignKey(e => e.Id_ncc);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.Alias)
                .IsFixedLength();

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.Image)
                .IsFixedLength();

            modelBuilder.Entity<SANPHAM>()
                .HasMany(e => e.CHITIETSANPHAMs)
                .WithOptional(e => e.SANPHAM)
                .HasForeignKey(e => e.Id_sanpham);

            modelBuilder.Entity<THONGTINCUAHANG>()
                .Property(e => e.Dienthoai)
                .IsFixedLength();
        }
    }
}
