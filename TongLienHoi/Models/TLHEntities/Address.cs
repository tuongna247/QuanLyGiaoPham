namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Address
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Address()
        {
            Clergies = new HashSet<Clergy>();
            Clergies1 = new HashSet<Clergy>();
            Clergies2 = new HashSet<Clergy>();
            Clergies3 = new HashSet<Clergy>();
        }

        public int Id { get; set; }

        [StringLength(250)]
        public string Street { get; set; }

        [StringLength(50)]
        public string Ward { get; set; }

        [StringLength(250)]
        public string District { get; set; }

        [StringLength(250)]
        public string City { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        public int? DistrictId { get; set; }

        public int? CityId { get; set; }

        [StringLength(20)]
        public string HomePhone { get; set; }

        [StringLength(20)]
        public string Fax { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clergy> Clergies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clergy> Clergies1 { get; set; }

        public virtual City City1 { get; set; }

        public virtual District District1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clergy> Clergies2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clergy> Clergies3 { get; set; }
    }
}
