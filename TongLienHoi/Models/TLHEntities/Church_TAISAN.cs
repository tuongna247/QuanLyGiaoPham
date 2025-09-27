namespace HTTLVN.QLTLH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Church_TAISAN
    {
        public int Id { get; set; }

        public int? Church_ID { get; set; }

        [StringLength(250)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Content { get; set; }

        public DateTime? DateApproval { get; set; }

        [StringLength(250)]
        public string AttachFile { get; set; }

        public virtual Church Church { get; set; }
    }
}
