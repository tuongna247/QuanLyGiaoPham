namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ToThamVieng")]
    public partial class ToThamVieng
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ToThamVieng()
        {
            ToThamVieng_HoGiaDinh = new HashSet<ToThamVieng_HoGiaDinh>();
        }

        public int id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(50)]
        public string ShortTitle { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ToThamVieng_HoGiaDinh> ToThamVieng_HoGiaDinh { get; set; }
    }
}
