namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TinHuu_Addresses
    {
        public int Id { get; set; }

        [StringLength(1000)]
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

        public virtual City City1 { get; set; }

        public virtual District District1 { get; set; }
    }
}
