using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTTLVN.QLTLH.Models.Warper
{
    public class VanThuReport
    {
        public int id { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public string NguoiGui { get; set; }
        public string VanBan { get; set; }
        public string GhiChu { get; set; }
        public string NgayVanThu { get; set; }
    }
}