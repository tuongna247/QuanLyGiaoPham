namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("viewChurch")]
    public partial class viewChurch
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ChurchId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ChurchName { get; set; }

        [StringLength(50)]
        public string TinhThanh { get; set; }

        [StringLength(250)]
        public string CapChiHoi { get; set; }
    }
}
