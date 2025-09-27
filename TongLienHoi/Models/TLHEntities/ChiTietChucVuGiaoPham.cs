namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietChucVuGiaoPham")]
    public partial class ChiTietChucVuGiaoPham
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int ChucVuID { get; set; }

        public int GiaoPhamId { get; set; }

        [Column(TypeName = "date")]
        public DateTime TuNgay { get; set; }

        [Column(TypeName = "date")]
        public DateTime DenNgay { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GiaoVuLenh { get; set; }

        [StringLength(250)]
        public string UrlGiaoVuLenh { get; set; }

        public int? UpdateBy { get; set; }

        public int? CreatedBy { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public int? AddressId { get; set; }

        public virtual ChucVu ChucVu { get; set; }

        public virtual Clergy Clergy { get; set; }
    }
}
