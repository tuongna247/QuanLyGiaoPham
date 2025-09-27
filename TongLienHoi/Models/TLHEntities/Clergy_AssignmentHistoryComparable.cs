using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Office2010.Excel;
using HTTLVN.QLTLH.Code;

namespace HTTLVN.QLTLH.Models
{
    public class Clergy_AssignmentHistoryComparable //: IComparable
    {
        public string ClergyTitle { get; set; }
        public string ChucVu { get; set; }
        public string ChurchName { get; set; }
        public int ChurchId { get; set; }
        public string Paper { get; set; }
        public string PaperNumber { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime DateStartDate { get; set; }
        public DateTime DateEndDate { get; set; }

        //public int CompareTo(object obj)
        //{
        //    if (obj == null) return 1;
        //    var otherTemplate = obj as Clergy_AssignmentHistoryComparable;
        //    if (otherTemplate != null)
        //        return DateStartDate.CompareTo(otherTemplate.DateStartDate);
        //    throw new ArgumentException("Object is null");
        //}

        public Clergy_AssignmentHistoryComparable(Clergy_AssignmentHistory cleryHistory)
        {
            StartDate = cleryHistory.StartDate;
            EndDate = cleryHistory.EndDate;
            if (cleryHistory.ChurchId != null)
                ChurchId = cleryHistory.ChucVuId.HasValue?cleryHistory.ChurchId.Value:0;
            ClergyTitle = cleryHistory.ClergyTitle != null ? cleryHistory.ClergyTitle.Title : "";
            ChucVu = cleryHistory.ChucVu != null ? cleryHistory.ChucVu.Name : "";
            Paper = cleryHistory.Paper;
            ChurchName = cleryHistory.Church != null ? cleryHistory.Church.ChurchName : "";
            PaperNumber = cleryHistory.PaperNumber;
            DateEndDate = CommonFunction.ConvertCustomDateTime(cleryHistory.EndDate);
            DateStartDate = CommonFunction.ConvertCustomDateTime(cleryHistory.StartDate);
        }

    }
}