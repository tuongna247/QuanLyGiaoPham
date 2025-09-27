using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HTTLVN.QLTLH.Models;
using HTTLVN.QLTLH.Models.Warper;

namespace HTTLVN.QLTLH.Code
{

    public class ValidateFunction
    {
        /// <summary
        /// kiem tra tên giáo phẩm
        /// </summary>
        /// <param name="giaopham"></param>
        /// <returns></returns>
        public static Dictionary<string,string> ValidateDataGiaoPham(Clergy giaopham)
        {
            var listError = new Dictionary<string,string>();
            if(string.IsNullOrEmpty(giaopham.LastName))
                listError.Add("LastName", "Chưa nhập Tên giáo phẩm.");
            if (!string.IsNullOrEmpty(giaopham.Email) && !CommonFunction.IsValidEmail(giaopham.Email))
                listError.Add("Email", "Email không hợp lệ.");
            //kiem tra trung trong truong hop them moi
            //Trong trường hợp cập nhật kiểm tra data đã tồn tại 1 dòng rồi thì cũng báo lỗi luôn
            string tenGiaoPhamTrung;
            //if (giaopham.Id == 0 && CommonFunction.IsDublicateID(giaopham.IdentityNumber,out tenGiaoPhamTrung))
            //{
            //    listError.Add("IdentityNumber", "Số CMND đã tồn tại.");
            //}
            //else if (giaopham.Id > 0 && CommonFunction.IsContainTwoID(giaopham.IdentityNumber,giaopham.Id, out tenGiaoPhamTrung))
            //{
            //    listError.Add("IdentityNumber", "Số CMND đã tồn tại.");
            //}

            if (giaopham.TitleId==0)
                listError.Add("TitleId", "Chưa chọn chức danh.");
            //if (string.IsNullOrEmpty(giaopham.IdentityNumber))
            //    listError.Add("IdentityNumber", "Chưa nhập CMND.");
            if (CommonFunction.CompareDateTime(giaopham.DateOfBirth, giaopham.IDApprovedDate))
            {
                listError.Add("IDApprovedDate,DateOfBirth", "Ngày sinh phải nhỏ hơn Ngày cấp CMND.");
            }
          
            if (CommonFunction.CompareDateTime(giaopham.DateOfBirth, giaopham.BaptismDate))
            {
                listError.Add("DateOfBirth,BaptismDate", "Ngày sinh phải nhỏ hơn Năm chịu báp-têm.");
            }
            if (CommonFunction.IsValidateDateString(giaopham.BaptismDate))
            {
                listError.Add("BaptismDate", "Năm chịu báp-têm không hợp lệ.");
            }
            if (CommonFunction.IsValidateDateString(giaopham.DateOfBirth))
            {
                listError.Add("DateOfBirth", "Ngày sinh không hợp lệ.");
            }
            if (CommonFunction.IsValidateDateString(giaopham.BelieveDate))
            {
                listError.Add("BelieveDate", "Năm tin Chúa không hợp lệ.");
            }
            if (CommonFunction.CompareDateTime(giaopham.DateOfBirth, giaopham.BelieveDate))
            {
                listError.Add("DateOfBirth,BelieveDate", "Ngày sinh phải nhỏ hơn Năm tin Chúa.");
            }
            return listError;
        }

        public static Dictionary<string, string> ValidateGiaDinhGiaoPham(List<QuanHeGiaDinhDBO> quanhes)
        {
            var listError = new Dictionary<string, string>();
            
            foreach (var quanHeGiaDinhDbo in quanhes)
            {
                if ( string.IsNullOrEmpty(quanHeGiaDinhDbo.HoTen) && !listError.ContainsKey("HoTen"+quanHeGiaDinhDbo.RelationShipName))
                    listError.Add("HoTen"+quanHeGiaDinhDbo.RelationShipName, "Chưa nhập họ tên "+quanHeGiaDinhDbo.RelationShipName);    
            }
            return listError;
        }

        public static Dictionary<string, string> ValidateGiaDinhGiaoPham(QuanHeVoiGiaoPham quanhes)
        {
            var listError = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(quanhes.HoTen))
                listError.Add("HoTen", "Chưa nhập họ tên.");
            if (quanhes.YearOfBirth > 2200 || quanhes.YearOfBirth <1800)
                listError.Add("YearOfBirth", "Năm sinh không hợp lệ.");
            return listError;
        }

        public static Dictionary<string, string> ValidateDataTitleHistory(Clergy_TitleHistory title)
        {
            var listError = new Dictionary<string, string>();
            if (title.TitleId==0)
            {
                listError.Add("TitleId", "Chưa chọn chức danh.");
            }
            
            return listError;
        }

        public static Dictionary<string, string> ValidateVanThu(VanThu vanthu)
        {
            var listError = new Dictionary<string, string>();
            // Loại văn thư, Ngày gửi, Người gửi, Người nhận, Về việc
            if (string.IsNullOrWhiteSpace(vanthu.LoaiVanThu) || vanthu.LoaiVanThu=="0")
            {
                listError.Add("LoaiVanThu", "Chưa chọn loại văn thư");
            }
            if (vanthu.LoaiVanDe == "0")
            {
                listError.Add("LoaiVanDe", "Chưa chọn phân loại");
            }
            if (vanthu.NgayVanThu == null ||  vanthu.NgayVanThu.Value==DateTime.MinValue)
            {
                listError.Add("NgayVanThu", "Chưa nhập Ngày gửi.");
            }
            if (string.IsNullOrWhiteSpace(vanthu.NoiDung))
            {
                listError.Add("NoiDung", "Chưa nhập nội dung.");
            }
            if (string.IsNullOrWhiteSpace(vanthu.NguoiGui))
            {
                listError.Add("NguoiGui", "Chưa nhập người gửi.");
            }
            if (string.IsNullOrWhiteSpace(vanthu.NguoiNhan))
            {
                listError.Add("NguoiNhan", "Chưa nhập người nhận.");
            }
            if (string.IsNullOrWhiteSpace(vanthu.TieuDe))
            {
                listError.Add("TieuDe", "Chưa nhập tiêu đề.");
            }
            return listError;
        }

        public static Dictionary<string, string> ValidateDataClergyEducation(Clergy_Education title)
        {
            var listError = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(title.CollegeOrProgramme))
                listError.Add("CollegeOrProgramme", "Chưa nhập Chương trình Thần học.");
            
            return listError;
        }

        public static Dictionary<string, string> ValidateDataClergyConDuct(Clergy_Conduct title)
        {
            var listError = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(title.Paper))
                listError.Add("Paper", "Chưa nhập lý do.");
            return listError;
        }

        public static Dictionary<string, string> CheckValidationCity(City city)
        {
            var listError = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(city.Name))
            {
                listError.Add("Name", "Chưa nhập Tên Tỉnh Thành.");
            }
            return listError;
        }


        public static Dictionary<string, string> ValidateDataBangCapNguoiPhoiNgau(BangCapNguoiPhoiNgau title)
        {
            var listError = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(title.ChuongTrinhThanHoc))
                listError.Add("ChuongTrinhThanHoc", "Chưa nhập Chương trình Thần học.");

            return listError;
        }

        public static Dictionary<string, string> ValidateDataClergyAssign(Clergy_AssignmentHistory title,bool isSkipChucVu)
        {
            var listError = new Dictionary<string, string>();
            
            if (title.ChurchId==0 && !isSkipChucVu)
                listError.Add("ChurchId", "Chưa chọn nhiệm sở.");
            if (title.TitleId == 0)
                listError.Add("TitleId", "Chưa chọn chức danh.");
            if (title.ChucVuId == 0)
                listError.Add("ChucVuId", "Chưa chọn chức vụ.");
            //if (title.CapChiHoi == 0)
            //    listError.Add("CapChiHoi", "Chưa chọn cấp chi hội.");
            return listError;
        }
    }
}