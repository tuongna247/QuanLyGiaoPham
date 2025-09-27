namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_UserRoles
    {
        [StringLength(255)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(250)]
        public string username { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int? RoleMappingId { get; set; }
    }
}
