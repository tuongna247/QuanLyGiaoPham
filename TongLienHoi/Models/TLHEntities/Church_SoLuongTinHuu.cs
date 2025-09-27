namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Church_SoLuongTinHuu
    {
        public int Id { get; set; }

        public int? SoLuongTinHuu { get; set; }

        public int? NamThongKe { get; set; }

        public int? TinHuuBapTem { get; set; }

        public int? TinHuuChuaBapTem { get; set; }

        public int? LuaTuoiThieuNhi { get; set; }

        public int? LuaTuoiThieuNien { get; set; }

        public int? LuaTuoiThanhNien { get; set; }

        public int? LuaTuoiTrangNien { get; set; }

        public int? LuaTuoiTrungNien { get; set; }

        public int? LuaTuoiLaoNien { get; set; }

        public int? GioiTinhNam { get; set; }

        public int? GioiTinhNu { get; set; }

        public int? Church_Id { get; set; }

        public virtual Church Church { get; set; }
    }
}
