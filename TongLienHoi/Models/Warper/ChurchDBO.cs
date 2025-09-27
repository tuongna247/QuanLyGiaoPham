using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HTTLVN.QLTLH.Models.Warper
{
    public class ChurchDBO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nhập tên danh mục")]
        [DisplayName("Mô tả")]
        public string ChurchName { get; set; }
        
        public int CapChiHoiID { get; set; }
        public string AddressFull { get; set; }
        public int CitiId { get; set; }
        public string CellPhone { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}