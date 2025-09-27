using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HTTLVN.QLTLH.Models.Warper
{
    public class MucSuDBO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Nhập tên ")]
        [DisplayName("Tên")]
        public string LastName { get; set; }
        public string NickName { get; set; }
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Avatar { get; set; }
        public string PlaceOfBirth { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime IDApprovedDate { get; set; }
        public int IDApprovedPlace { get; set; }
        public string NoicapNgayCap { get; set; }
        public DateTime BelieveDate { get; set; }
        public DateTime BaptismDate { get; set; }
        public int PermanentAddressId { get; set; }
        public int ContactAddressId { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public int TitleId { get; set; }
        public int ChucVuId { get; set; }
        public int CapChiHoiId { get; set; }
        public string TenNhiemSo { get; set; }
        public string DiaChiNhiemSo { get; set; }
        public int CurrentChurchId { get; set; }
        public string DienThoaiNhiemSo { get; set; }
        public string EmailNhiemSo { get; set; }
        public int TinhThanhId { get; set; }
        public string TenDayDuKhongDau { get; set; }
        public int DanToc { get; set; }
        public int KetHon { get; set; }
        public int BirthAddressId { get; set; }
        public int BelieveYear { get; set; }
        public int BaptismYear { get; set; }

    }
}