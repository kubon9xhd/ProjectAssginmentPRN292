namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FollowU
    {
        public int ID { get; set; }

        [StringLength(150)]
        public string url { get; set; }

        [StringLength(50)]
        public string icon { get; set; }
    }
}
