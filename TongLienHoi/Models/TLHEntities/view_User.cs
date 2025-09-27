namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class view_User
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(250)]
        public string username { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(250)]
        public string password { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(250)]
        public string email { get; set; }

        public int? status { get; set; }

        [StringLength(255)]
        public string Name { get; set; }
    }
}
