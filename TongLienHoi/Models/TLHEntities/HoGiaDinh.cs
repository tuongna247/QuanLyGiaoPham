namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoGiaDinh")]
    public partial class HoGiaDinh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HoGiaDinh()
        {
            HoGiaDinhTinHuu = new HashSet<HoGiaDinhTinHuu>();
            ToThamViengHoGiaDinh = new HashSet<ToThamVieng_HoGiaDinh>();
        }

        public int id { get; set; }

        public int? HomeOwner_Id { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(50)]
        public string ShortTitle { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public virtual TinHuu TinHuu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoGiaDinhTinHuu> HoGiaDinhTinHuu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ToThamVieng_HoGiaDinh> ToThamViengHoGiaDinh { get; set; }
    }
}
