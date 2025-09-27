namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("City")]
    public partial class City
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public City()
        {
            Addresses = new HashSet<Address>();
            Churches = new HashSet<Church>();
            Clergies = new HashSet<Clergy>();
            Districts = new HashSet<District>();
            TinHuu_Addresses = new HashSet<TinHuu_Addresses>();
        }

        public int id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public int? Status { get; set; }

        public int? SoChiHoi { get; set; }

        public int? SoChiHoiCT { get; set; }

        public int? SoChiHoiCCT { get; set; }

        public int? HoiNhanh { get; set; }

        public int? HoiNhanhCT { get; set; }

        public int? HoiNhanhCCT { get; set; }

        public int? DiemNhom { get; set; }

        public int? DiemNhomCT { get; set; }

        public int? DiemNhomCCT { get; set; }

        public int? TinHuu { get; set; }

        public int? TinHuuChuaBapTem { get; set; }

        public int? TinHuuDiNhom { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Church> Churches { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clergy> Clergies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<District> Districts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TinHuu_Addresses> TinHuu_Addresses { get; set; }
    }
}
