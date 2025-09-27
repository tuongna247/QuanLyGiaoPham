using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTTLVN.QLTLH.Models.Warper
{
    public class VanThuDBO
    {
        public int id { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public string NoiDungPheDuyet { get; set; }
        public string LoaiVanDe { get; set; }
        public string NguoiGui { get; set; }
        public string NguoiNhan { get; set; }
        public string VanBan { get; set; }
        public string GhiChu { get; set; }
        public string TinhTrang { get; set; }
        public bool TrangThaiPheDuyet { get; set; }
        public DateTime NgayVanThu { get; set; }

        public IEnumerable<SelectListItem> LoaiVanThu
        {
            
            get
            {
                var items = new List<SelectListItem>
                {
                    new SelectListItem {Text = "Chọn loại văn thư", Value = "0"},
                    new SelectListItem {Text = "Văn thư đến", Value = "1"},
                    new SelectListItem {Text = "Văn thư đi", Value = "2"}
                };
                return items;
            }
        }
    }
}