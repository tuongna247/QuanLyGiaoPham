namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BangCapNguoiPhoiNgau")]
    public partial class BangCapNguoiPhoiNgau
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string ChuongTrinhThanHoc { get; set; }

        [StringLength(255)]
        public string NoiHoc { get; set; }

        [StringLength(50)]
        public string NgayTotNghiep { get; set; }

        [StringLength(255)]
        public string ChungChi { get; set; }

        public int? QuanHeVoiGiaoPham_Id { get; set; }

        [StringLength(50)]
        public string StartDate { get; set; }

        [StringLength(50)]
        public string EndDate { get; set; }

        public int? OrderId { get; set; }

        public virtual QuanHeVoiGiaoPham QuanHeVoiGiaoPham { get; set; }
    }
}
