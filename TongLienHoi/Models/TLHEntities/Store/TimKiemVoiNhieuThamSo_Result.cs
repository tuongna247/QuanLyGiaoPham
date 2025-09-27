using System;

namespace HTTLVN.QLTLH.Models
{
    public class TimKiemVoiNhieuThamSo_Result
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string NickName { get; set; }
        public bool? Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Avatar { get; set; }
       
        public int? ContactAddressId { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public int? TitleId { get; set; }
        public int? ChucVuId { get; set; }
        public int? CapChiHoiId { get; set; }
        public string TenNhiemSo { get; set; }
        public string DiaChiNhiemSo { get; set; }
        public int? CurrentChurchId { get; set; }
        public string DienThoaiNhiemSo { get; set; }
        public string EmailNhiemSo { get; set; }
        public int? TinhThanhId { get; set; }
        public string TenDayDuKhongDau { get; set; }
        public int? DanToc { get; set; }
        public int? KetHon { get; set; }
        public int? BirthAddressId { get; set; }
        public int? BelieveYear { get; set; }
        public int? BaptismYear { get; set; }
        public int? CurrentAddressId { get; set; }
        public int? CurrentAssignment_Id { get; set; }
        public string DateOfDeath { get; set; }
        public string NoiSinh { get; set; }
        public bool? DinhCuNuocNgoai { get; set; }
        public bool? KyLuat { get; set; }
        public bool? DuHoc { get; set; }
        public bool? NghiChucVu { get; set; }
        public bool? HuuTri { get; set; }
        public int? SoNamChucVu { get; set; }
        public DateTime? ThoiGianChucVu { get; set; }
        public bool? QuaPhu { get; set; }
        public bool? IsVeVoiChua { get; set; }
        public int? SoThangChucVu { get; set; }
        public int? Seq { get; set; }
        public int? Sort { get; set; }
        public string TenChucDanh { get; set; }
    }
}