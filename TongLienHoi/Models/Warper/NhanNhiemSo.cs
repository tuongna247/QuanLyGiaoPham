using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTLVN.QLTLH.Models.Warper
{
    public class NhanNhiemSoDBO
    {
        public int ChurchId { get; set; }
        public int ChucDanhId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NhiemKy { get; set; }
        public int OfficeId { get; set; }
        public string Role { get; set; }
        public int TyLeLuu { get; set; }
        public string Paper { get; set; }
        public string Infomation { get; set; }
        public DateTime ApplyDate { get; set; }

    }
}