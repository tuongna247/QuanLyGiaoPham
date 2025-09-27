namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Clergy_AssignmentHistory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Clergy_AssignmentHistory()
        {
            Clergies = new HashSet<Clergy>();
        }

        public int id { get; set; }

        public int ClergyId { get; set; }

        public int? ChurchId { get; set; }

        public int? OfficeId { get; set; }

        [StringLength(50)]
        public string Role { get; set; }

        [StringLength(50)]
        public string StartDate { get; set; }

        [StringLength(50)]
        public string EndDate { get; set; }

        public int? Term { get; set; }

        public int? TitleId { get; set; }

        public int? TyLeLuu { get; set; }

        public bool? NeedAlert { get; set; }

        [StringLength(250)]
        public string Paper { get; set; }

        public string Information { get; set; }

        public int? NamChucVu { get; set; }

        public bool? NhanNhiemSo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Ngay_NhanNhiemSo { get; set; }

        public bool? ChucVu_HienTai { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ApplyDate { get; set; }

        [StringLength(50)]
        public string TermType { get; set; }

        [StringLength(250)]
        public string PaperNumber { get; set; }
        public string ChinhQuyenCN { get; set; }
        public string ChinhQuyenCNSo { get; set; }

        public int? ChucVuId { get; set; }

        public int? TermNumber { get; set; }

        public int? CapChiHoi { get; set; }

        public int? OrderId { get; set; }

        public bool? IsCurrent { get; set; }

        public virtual CapChiHoi CapChiHoi1 { get; set; }

        public virtual ChucVu ChucVu { get; set; }

        public virtual Church Church { get; set; }

        public virtual Clergy Clergy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clergy> Clergies { get; set; }

        public virtual ClergyTitle ClergyTitle { get; set; }

        public virtual Office Office { get; set; }
    }
}
