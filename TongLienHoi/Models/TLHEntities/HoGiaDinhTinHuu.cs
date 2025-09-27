namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HoGiaDinhTinHuu
    {
        public int id { get; set; }

        public int? HoGiaDinh_Id { get; set; }

        public int? TinHuu_Id { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public virtual HoGiaDinh HoGiaDinh { get; set; }

        public virtual TinHuu TinHuu { get; set; }
    }
}
