namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Clergy")]
    public partial class Clergy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Clergy()
        {
            ChiTietChucVuGiaoPhams = new HashSet<ChiTietChucVuGiaoPham>();
            Clergy_AssignmentHistory = new HashSet<Clergy_AssignmentHistory>();
            Clergy_Conduct = new HashSet<Clergy_Conduct>();
            Clergy_Education = new HashSet<Clergy_Education>();
            QuanHeVoiGiaoPhams = new HashSet<QuanHeVoiGiaoPham>();
            QuanHeVoiGiaoPhams1 = new HashSet<QuanHeVoiGiaoPham>();
            Clergy_TitleHistory = new HashSet<Clergy_TitleHistory>();
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

        [StringLength(250)]
        public string NoicapNgayCap { get; set; }

        [StringLength(50)]
        public string BelieveDate { get; set; }

        [StringLength(50)]
        public string BaptismDate { get; set; }

        public int? PermanentAddressId { get; set; }

        public int? ContactAddressId { get; set; }

        [StringLength(250)]
        public string Phone { get; set; }

        [StringLength(250)]
        public string MobilePhone { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        public int? TitleId { get; set; }

        public int? ChucVuId { get; set; }

        public int? CapChiHoiId { get; set; }

        [StringLength(250)]
        public string TenNhiemSo { get; set; }

        [StringLength(250)]
        public string DiaChiNhiemSo { get; set; }

        public int? CurrentChurchId { get; set; }

        [StringLength(250)]
        public string DienThoaiNhiemSo { get; set; }

        [StringLength(250)]
        public string EmailNhiemSo { get; set; }

        public int? TinhThanhId { get; set; }

        [StringLength(250)]
        public string TenDayDuKhongDau { get; set; }

        public int? DanToc { get; set; }

        public int? KetHon { get; set; }

        public int? BirthAddressId { get; set; }

        public int? BelieveYear { get; set; }

        public int? BaptismYear { get; set; }

        public int? CurrentAddressId { get; set; }

        public int? CurrentAssignment_Id { get; set; }

        [StringLength(50)]
        public string DateOfDeath { get; set; }
        [StringLength(50)]
        public string DienKhacNgay { get; set; }
        [StringLength(100)]
        public string DienKhacLyDo { get; set; }
        [StringLength(50)]
        public string NgayBoNhiem { get; set; }

        [StringLength(250)]
        public string NoiSinh { get; set; }

        public bool? DinhCuNuocNgoai { get; set; }
        public bool? IsVeVoiChua { get; set; }
        public bool? IsGianDoanCV { get; set; }
        [StringLength(500)]
        public string GianDoanCVChitiet { get; set; }

        public bool? KyLuat { get; set; }

        public bool? DuHoc { get; set; }

        public bool? HuuTri { get; set; }
        public bool? NghiChucVu { get; set; }

        public int? SoNamChucVu { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ThoiGianChucVu { get; set; }

        /// <summary>
        /// dùng để hiển thị năm hết chức vụ 
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime? KetThucChucVu { get; set; }

        public bool? QuaPhu { get; set; }

        public int? SoThangChucVu { get; set; }

        public int? Seq { get; set; }

        public virtual Address Address { get; set; }

        public virtual Address Address1 { get; set; }

        public virtual Address Address2 { get; set; }

        public virtual Address Address3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietChucVuGiaoPham> ChiTietChucVuGiaoPhams { get; set; }

        public virtual ChucVu ChucVu { get; set; }

        public virtual Church Church { get; set; }

        public virtual City City { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clergy_AssignmentHistory> Clergy_AssignmentHistory { get; set; }

        public virtual Clergy_AssignmentHistory Clergy_AssignmentHistory1 { get; set; }

        public virtual ClergyTitle ClergyTitle { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clergy_Conduct> Clergy_Conduct { get; set; }

        public virtual DanToc DanToc1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clergy_Education> Clergy_Education { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuanHeVoiGiaoPham> QuanHeVoiGiaoPhams { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuanHeVoiGiaoPham> QuanHeVoiGiaoPhams1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clergy_TitleHistory> Clergy_TitleHistory { get; set; }
    }
}
