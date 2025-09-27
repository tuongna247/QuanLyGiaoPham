namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ToThamVieng_HoGiaDinh
    {
        public int id { get; set; }

        public int? ToThamVieng_Id { get; set; }

        public int? HoGiaDinh_Id { get; set; }

        public virtual HoGiaDinh HoGiaDinh { get; set; }

        public virtual ToThamVieng ToThamVieng { get; set; }
    }
}
