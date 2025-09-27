namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Clergy_Conduct
    {
        public int id { get; set; }

        public int? ConductTypeId { get; set; }

        [StringLength(50)]
        public string Fromdate { get; set; }

        [StringLength(50)]
        public string ToDate { get; set; }

        [StringLength(250)]
        public string Paper { get; set; }

        public int ClergyId { get; set; }

        [StringLength(250)]
        public string VanBan { get; set; }

        [StringLength(250)]
        public string SoVanBan { get; set; }

        public int? OrderId { get; set; }

        public virtual Clergy Clergy { get; set; }

        public virtual Conduct_Type Conduct_Type { get; set; }
    }
}
