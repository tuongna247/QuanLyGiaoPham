namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VanThu")]
    public partial class VanThu
    {
        public int id { get; set; }

        [StringLength(250)]
        public string TieuDe { get; set; }

        public string NoiDung { get; set; }

        public string NoiDungPheDuyet { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayVanThu { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayTao { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayPheDuyet { get; set; }

        public int? DuocDuyetBoi { get; set; }

        [StringLength(250)]
        public string LoaiVanDe { get; set; }

        [StringLength(250)]
        public string GhiChu { get; set; }

        public bool? TrangThaiPheDuyet { get; set; }

        [StringLength(50)]
        public string TinhTrang { get; set; }

        [StringLength(250)]
        public string NguoiGui { get; set; }

        [StringLength(250)]
        public string NguoiNhan { get; set; }

        [StringLength(500)]
        public string VanBan { get; set; }

        [StringLength(50)]
        public string LoaiVanThu { get; set; }

        public int? PhanLoaiVanThu_Id { get; set; }

        public virtual User User { get; set; }
    }
}
