namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_GetAllEndDay
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(250)]
        public string Avatar { get; set; }

        
        public string ChucDanh { get; set; }
        public string ChucVu { get; set; }
        public string TinhThanh { get; set; }
        public string NhiemSo { get; set; }
        public string GhiChu { get; set; }
        public Clergy Clergy { get; set; }

        public int Id { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? KetThucChucVu { get; set; }

        //public string EndDate { get; set; }
    }
}
