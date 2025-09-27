using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTLVN.QLTLH.Models.DBO
{
    public class AutoCodeDBO
    {
        public int Id { get; set; }
        public int CodeType { get; set; }
        public int Length { get; set; }
        public string FixChar { get; set; }
    }

    public class AutoCode_SubDBO
    {
        public int Id { get; set; }
        public int CodeType { get; set; }
        public string Type { get; set; }
        public int Length { get; set; }
        public string FixChar { get; set; }
        public int Position { get; set; }
    }
}