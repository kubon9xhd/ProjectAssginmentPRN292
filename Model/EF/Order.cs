namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public long ID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? CustomerID { get; set; }

        [StringLength(100)]
        public string MetaTitle { get; set; }

        [StringLength(100)]
        public string ShipName { get; set; }

        [StringLength(50)]
        public string ShipMobile { get; set; }

        [StringLength(50)]
        public string ShipEmail { get; set; }

        [StringLength(50)]
        public string ShipAddress { get; set; }

        [StringLength(50)]
        public string ShipCountry { get; set; }

        [StringLength(50)]
        public string ShipCity { get; set; }

        [StringLength(50)]
        public string ShipDistrict { get; set; }

        [StringLength(50)]
        public string ShipVillage { get; set; }

        public bool? ShipCOD { get; set; }

        public int? Status { get; set; }
    }
}
