namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuanHeVoiGiaoPham")]
    public partial class QuanHeVoiGiaoPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QuanHeVoiGiaoPham()
        {
            BangCapNguoiPhoiNgaus = new HashSet<BangCapNguoiPhoiNgau>();
        }

        public int Id { get; set; }

        public int? ClergyId { get; set; }

        public int? RelationShipId { get; set; }

        [StringLength(250)]
        public string HoTen { get; set; }

        [StringLength(250)]
        public string NgheNghiep { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int? YearOfBirth { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BangCapNguoiPhoiNgau> BangCapNguoiPhoiNgaus { get; set; }

        public virtual Clergy Clergy { get; set; }

        public virtual Clergy Clergy1 { get; set; }

        public virtual QuanHeGiaDinh QuanHeGiaDinh { get; set; }
    }
}
