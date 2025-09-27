namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_GetEndDay
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(250)]
        public string Avatar { get; set; }
        [StringLength(500)]
        public string GianDoanCVChitiet { get; set; }

        [Column(TypeName = "date")]
        public DateTime? KetThucChucVu { get; set; }
    }
}
