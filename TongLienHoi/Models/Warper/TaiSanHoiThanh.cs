using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HTTLVN.QLTLH.Models
{
    public class TaiSanHoiThanh
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public string Name { get; set; }
        [ScaffoldColumn(false)]
        public string Description { get; set; }
        [UIHint("FileUpload")]
        [Required]
        public string ImageUrl { get; set; }
    }
}