namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TinHuu_Conduct
    {
        public int id { get; set; }

        public int? ConductTypeId { get; set; }

        public int TinHuu_Id { get; set; }

        [StringLength(50)]
        public string Fromdate { get; set; }

        [StringLength(50)]
        public string ToDate { get; set; }

        [StringLength(250)]
        public string VanBan { get; set; }

        [StringLength(250)]
        public string SoVanBan { get; set; }

        public virtual TinHuu TinHuu { get; set; }
    }
}
