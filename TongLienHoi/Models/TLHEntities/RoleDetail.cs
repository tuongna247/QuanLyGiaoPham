namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoleDetail")]
    public partial class RoleDetail
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int RoleMapping { get; set; }

        public virtual RoleMapping RoleMapping1 { get; set; }

        public virtual User User { get; set; }
    }
}
