using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HTTLVN.QLTLH.Code;

namespace HTTLVN.QLTLH.Models
{
    public class VAssignComparable
    {
        public int id { get; set; }
        public string ClergyTitle { get; set; }
        public string ChucVu { get; set; }
        public string ChurchName { get; set; }
        public string Paper { get; set; }
        public string PaperNumber { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime DateStartDate { get; set; }
        public DateTime DateEndDate { get; set; }
        public string NhiemKy { get; set; }
        public int? TermNumber { get; set; }


        public VAssignComparable(v_assign cleryHistory)
        {
            id = cleryHistory.id;
            StartDate = cleryHistory.StartDate;
            EndDate = cleryHistory.EndDate;
            ClergyTitle = cleryHistory.Title;
            ChucVu = cleryHistory.TenChucVu;
            Paper = cleryHistory.Paper;
            ChurchName = cleryHistory.CapChiHoi +" "+ cleryHistory.ChurchName;
            PaperNumber = cleryHistory.PaperNumber;
            DateEndDate = CommonFunction.ConvertCustomDateTime(cleryHistory.EndDate);
            DateStartDate = CommonFunction.ConvertCustomDateTime(cleryHistory.StartDate);
            NhiemKy = cleryHistory.Term.HasValue ? cleryHistory.Term + " " + cleryHistory.TermType : "";
            TermNumber = cleryHistory.TermNumber;
        }
    }
}