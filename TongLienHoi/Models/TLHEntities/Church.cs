namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Church")]
    public partial class Church
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Church()
        {
            Church_Assignment_History = new HashSet<Church_Assignment_History>();
            Church_SoLuongTinHuu = new HashSet<Church_SoLuongTinHuu>();
            Church_TAISAN = new HashSet<Church_TAISAN>();
            Church_TinHuu = new HashSet<Church_TinHuu>();
            Clergy_AssignmentHistory = new HashSet<Clergy_AssignmentHistory>();
            Clergies = new HashSet<Clergy>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ChurchName { get; set; }

        public int? CapChiHoiID { get; set; }

        [StringLength(500)]
        public string AddressFull { get; set; }

        public int? CitiId { get; set; }

        [StringLength(50)]
        public string CellPhone { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(500)]
        public string AnhDaiDien { get; set; }

        [StringLength(4000)]
        public string GhiChu { get; set; }

        [StringLength(50)]
        public string NgayThanhLap { get; set; }

        [StringLength(50)]
        public string NgayCongNhanDiemNhom { get; set; }

        [StringLength(50)]
        public string NgayTLHCongNhanDiemNhom { get; set; }

        [StringLength(500)]
        public string VBTLHCongNhanDiemNhom { get; set; }

        [StringLength(50)]
        public string NgayCQCongNhanDiemNhom { get; set; }

        [StringLength(500)]
        public string VBCQCongNhanDiemNhom { get; set; }

        [StringLength(50)]
        public string NgayCongNhanHoiNhanh { get; set; }

        [StringLength(50)]
        public string NgayTLHCongNhanHoiNhanh { get; set; }

        [StringLength(500)]
        public string VBTLHCongNhanHoiNhanh { get; set; }

        [StringLength(50)]
        public string NgayCQCongNhanHoiNhanh { get; set; }

        [StringLength(500)]
        public string VBCQCongNhanHoiNhanh { get; set; }

        [StringLength(50)]
        public string NgayCongNhanChiHoiTuDuong { get; set; }

        [StringLength(50)]
        public string NgayTLHCongNhanHoiTuDuong { get; set; }

        [StringLength(500)]
        public string VBTLHCongNhanHoiTuDuong { get; set; }

        [StringLength(50)]
        public string NgayCQCongNhanHoiTuDuong { get; set; }

        [StringLength(500)]
        public string VBCQCongNhanHoiTuDuong { get; set; }

        [StringLength(50)]
        public string NgayCongNhanChiHoiTuLap { get; set; }

        [StringLength(50)]
        public string NgayTLHCongNhanChiHoiTuLap { get; set; }

        [StringLength(500)]
        public string VBTLHCongNhanChiHoiTuLap { get; set; }

        [StringLength(50)]
        public string NgayCQCongNhanChiHoiTuLap { get; set; }

        [StringLength(500)]
        public string VBCQCongNhanChiHoiTuLap { get; set; }

        [StringLength(50)]
        public string NgayTaiLap { get; set; }

        [StringLength(50)]
        public string NgayCungHienDenTho { get; set; }

        [StringLength(2000)]
        public string TaiSanHoiThanh { get; set; }

        [StringLength(2000)]
        public string DienTichDat { get; set; }

        [StringLength(2000)]
        public string QuyMoCoSo { get; set; }

        [StringLength(500)]
        public string GiayChungNhanQuyenSuDungDat { get; set; }

        [StringLength(100)]
        public string Longtitude { get; set; }

        [StringLength(100)]
        public string Latitude { get; set; }

        [StringLength(100)]
        public string TenKhongDau { get; set; }
        [StringLength(100)]
        public string SoChungNhan { get; set; }
        [StringLength(100)]
        public string NgayChungNhan { get; set; }

        public virtual CapChiHoi CapChiHoi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Church_Assignment_History> Church_Assignment_History { get; set; }

        public virtual City City { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Church_SoLuongTinHuu> Church_SoLuongTinHuu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Church_TAISAN> Church_TAISAN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Church_TinHuu> Church_TinHuu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clergy_AssignmentHistory> Clergy_AssignmentHistory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clergy> Clergies { get; set; }
    }
}
