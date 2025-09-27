namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Church_TinHuu_DanToc
    {
        public int Id { get; set; }

        public int? Church_TinHuu_Id { get; set; }

        public int? DanToc_Id { get; set; }

        public int? SoLuong { get; set; }

        public virtual Church_TinHuu Church_TinHuu { get; set; }

        public virtual DanToc DanToc { get; set; }
    }
}
