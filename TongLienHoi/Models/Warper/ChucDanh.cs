using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTLVN.QLTLH.Models.Warper
{
    public class ChucDanh
    {
        public ChucDanh()
        {
        }
        public ChucDanh(Clergy_TitleHistory title)
        {
            TitleId = title.TitleId;
            ApprovalPaper = title.ApprovalPaper;
            EffectiveDate = title.EffectiveDate;
            if (title.ClergyTitle != null)
                Name = title.ClergyTitle.Title;
        }

        public string Name { get; set; }
        public string EffectiveDate { get; set; }
        public string ApprovalPaper { get; set; }
        public int TitleId { get; set; }
    }

    public  class NhanNhiemSo
    {
        public  NhanNhiemSo(){}
        public NhanNhiemSo(Clergy_AssignmentHistory assign)
        {
            StartDate = assign.StartDate;
            EndDate = assign.EndDate;
            ChurchId = assign.ChurchId;
            Role = assign.Role;
            OfficeId = assign.OfficeId;
            Term = assign.Term;
            TyleLuu = assign.TyLeLuu;
            Paper = assign.Paper;
        }

        public int? Term { get; set; }
        public int? TitleId { get; set; }
        public double? TyleLuu { get; set; }
        public string Role { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Paper { get; set; }
        public int? ChurchId { get; set; }
        public int? OfficeId { get; set; }
    }

    public  class KyLuat
    {
        public int? ConductId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Paper { get; set; }
    }
}