namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TinHuu_AssignmentHistory
    {
        public int id { get; set; }

        public int TinHuu_Id { get; set; }

        public int? ChucDanh_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }

        [StringLength(50)]
        public string StartDate { get; set; }

        [StringLength(50)]
        public string EndDate { get; set; }

        public int? TyLeLuu { get; set; }

        [StringLength(250)]
        public string Paper { get; set; }

        public int? NamChucVu { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ApplyDate { get; set; }

        public bool? IsCurrent { get; set; }

        public virtual TinHuu TinHuu { get; set; }
    }
}
