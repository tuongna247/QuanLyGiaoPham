namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Clergy_TitleHistory
    {
        public int id { get; set; }

        public int? CleargyId { get; set; }

        public int TitleId { get; set; }

        [StringLength(50)]
        public string EffectiveDate { get; set; }

        [StringLength(250)]
        public string RequestPaper { get; set; }

        [StringLength(250)]
        public string ApprovalPaper { get; set; }

        public bool? ChucVuHienTai { get; set; }

        public string Content { get; set; }

        [StringLength(250)]
        public string DocumentNumber { get; set; }

        [StringLength(250)]
        public string DecisionNumber { get; set; }

        public int? OrderId { get; set; }
        public bool? IsCurrent { get; set; }

        public virtual Clergy Clergy { get; set; }

        public virtual ClergyTitle ClergyTitle { get; set; }
    }
}
