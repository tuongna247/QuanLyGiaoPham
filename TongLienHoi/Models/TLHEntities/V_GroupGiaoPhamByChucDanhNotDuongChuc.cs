namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_GroupGiaoPhamByChucDanhNotDuongChuc
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(10)]
        public string Short { get; set; }

        public bool? IsDuongChuc { get; set; }

        public int? Expr1 { get; set; }

        public int? Status { get; set; }
    }
}
