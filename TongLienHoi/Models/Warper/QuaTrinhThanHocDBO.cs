using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTLVN.QLTLH.Models.Warper
{
    public class QuaTrinhThanHocDBO
    {
        public string Degree { get; set; }
        public int? CourseId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CollegeOrProgramme { get; set; }
        public int Id { get; set; }
        public int GiaoPhamId { get; set; }
        public string Paper { get; set; }

        public QuaTrinhThanHocDBO(){}
        public QuaTrinhThanHocDBO(Clergy_Education edu)
        {
            Degree = edu.Degree;
            CourseId = edu.CourseId;
            FromDate = edu.FromDate;
            ToDate = edu.ToDate;
            CollegeOrProgramme = edu.CollegeOrProgramme;
            Paper = edu.Paper;
            Id = edu.Id;
            GiaoPhamId = edu.Clergy.Id;
        }
    }
}