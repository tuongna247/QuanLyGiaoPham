using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTLVN.QLTLH.Models.Warper
{
    public class QuanHeGiaDinhDBO
    {
        public int Id { get; set; }
        public string HoTen { get; set; }
        public int? NamSinh { get; set; }
        public string NgheNghiep { get; set; }
        public string Description { get; set; }
        public int? RelationShipId { get; set; }
        public string RelationShipName { get; set; }
        public int GiaoPhamId { get; set; }

        public QuanHeGiaDinhDBO(QuanHeVoiGiaoPham qhgd)
        {
            Id = qhgd.Id;
            HoTen = qhgd.HoTen;
            NamSinh = qhgd.YearOfBirth;
            NgheNghiep = qhgd.NgheNghiep;
            RelationShipId = qhgd.RelationShipId;
            Description = qhgd.Description;
            if (qhgd.QuanHeGiaDinh != null)
                RelationShipName = qhgd.QuanHeGiaDinh.Name;
            GiaoPhamId = qhgd.Clergy.Id;
        }

        public QuanHeGiaDinhDBO()
        {
            
        }
    }
}