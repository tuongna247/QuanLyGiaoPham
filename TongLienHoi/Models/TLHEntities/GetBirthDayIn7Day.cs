namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GetBirthDayIn7Day
    {
        [Column(TypeName = "date")]
        public DateTime? BIRTHDAY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Tuoi { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(250)]
        public string Avatar { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Title { get; set; }
    }
}
