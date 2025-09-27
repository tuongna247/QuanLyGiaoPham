namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_assign
    {
        [StringLength(50)]
        public string ChurchName { get; set; }

        [StringLength(50)]
        public string TinhThanh { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string TenChucVu { get; set; }

        public int? Term { get; set; }

        [StringLength(50)]
        public string TermType { get; set; }

        public int? NamChucVu { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Ngay_NhanNhiemSo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ApplyDate { get; set; }

        [StringLength(250)]
        public string PaperNumber { get; set; }

        public int? TermNumber { get; set; }

        public int? TyLeLuu { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClergyId { get; set; }

        [StringLength(50)]
        public string StartDate { get; set; }

        [StringLength(50)]
        public string EndDate { get; set; }

        [StringLength(250)]
        public string Paper { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(250)]
        public string CapChiHoi { get; set; }
        public string ChinhQuyenCN { get; set; }
        public string ChinhQuyenCNSo { get; set; }

        public int? ChurchId { get; set; }
    }
}
