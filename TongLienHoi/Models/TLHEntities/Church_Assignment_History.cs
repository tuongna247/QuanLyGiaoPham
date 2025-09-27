namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Church_Assignment_History
    {
        public int Id { get; set; }

        public int? Church_Id { get; set; }

        public int? CapChiHoi_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Created_DT { get; set; }

        [StringLength(50)]
        public string Created_By { get; set; }

        [StringLength(500)]
        public string GiaoVuLenh { get; set; }

        public bool? IsCurrent { get; set; }

        public virtual CapChiHoi CapChiHoi { get; set; }

        public virtual Church Church { get; set; }
    }
}
