namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_church
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }

        [Key]
        [Column(Order = 1)]
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
        public string CityName { get; set; }

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
    }
}
