using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class OrderDetailModel
    {
        public String ShipName { get; set; }
        public string ShipEmail { get; set; }
        public string ShipMobile { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCountry { get; set; }
        public string ShipCity { get; set; }
        public string ShipDistrict { get; set; }
        public string ShipVillage { get; set; }
    }
}