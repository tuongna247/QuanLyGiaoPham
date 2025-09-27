namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Clergy_Education
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string CollegeOrProgramme { get; set; }

        [StringLength(100)]
        public string Degree { get; set; }

        [StringLength(50)]
        public string FromDate { get; set; }

        [StringLength(50)]
        public string ToDate { get; set; }

        [StringLength(250)]
        public string Paper { get; set; }

        public int ClergyId { get; set; }

        public int? CourseId { get; set; }

        [StringLength(50)]
        public string mine_type { get; set; }

        public int? filesize { get; set; }

        [StringLength(100)]
        public string NoiChungNhan { get; set; }

        [StringLength(50)]
        public string NgayCapChungChi { get; set; }

        public int? OrderId { get; set; }

        public virtual Clergy Clergy { get; set; }
    }
}
