namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TinHuu")]
    public partial class TinHuu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TinHuu()
        {
            HoGiaDinhs = new HashSet<HoGiaDinh>();
            HoGiaDinh_TinHuu = new HashSet<HoGiaDinhTinHuu>();
            TinHuu_AssignmentHistory = new HashSet<TinHuu_AssignmentHistory>();
            TinHuu_Conduct = new HashSet<TinHuu_Conduct>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [StringLength(50)]
        public string NickName { get; set; }

        public bool Gender { get; set; }

        [StringLength(50)]
        public string DateOfBirth { get; set; }

        [StringLength(250)]
        public string Avatar { get; set; }

        [StringLength(250)]
        public string PlaceOfBirth { get; set; }

        [StringLength(250)]
        public string IdentityNumber { get; set; }

        [StringLength(50)]
        public string IDApprovedDate { get; set; }

        public int? IDApprovedPlace { get; set; }

        [StringLength(50)]
        public string BelieveDate { get; set; }

        [StringLength(50)]
        public string BaptismDate { get; set; }

        public int? PermanentAddressId { get; set; }

        public int? ContactAddressId { get; set; }

        public int? CurrentAddressId { get; set; }

        public int? BirthAddressId { get; set; }

        public int? ChucVuId { get; set; }

        public int? CurrentChurchId { get; set; }

        public int? TinhThanhId { get; set; }

        public int? HoGiaDinh_Id { get; set; }

        public int? BanNganh_Id { get; set; }

        public int? TitleId { get; set; }

        [StringLength(250)]
        public string Phone { get; set; }

        [StringLength(250)]
        public string MobilePhone { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [StringLength(250)]
        public string TenDayDuKhongDau { get; set; }

        public int? DanToc { get; set; }

        public int? KetHon { get; set; }

        public int? BelieveYear { get; set; }

        public int? BaptismYear { get; set; }

        [StringLength(50)]
        public string DateOfDeath { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        [StringLength(50)]
        public string NgheNghiep { get; set; }

        [StringLength(50)]
        public string QuanHeGiaDinh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoGiaDinh> HoGiaDinhs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoGiaDinhTinHuu> HoGiaDinh_TinHuu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TinHuu_AssignmentHistory> TinHuu_AssignmentHistory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TinHuu_Conduct> TinHuu_Conduct { get; set; }
    }
}
