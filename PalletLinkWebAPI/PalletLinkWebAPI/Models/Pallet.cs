using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PalletLinkWebAPI.Models
{
    public class Pallet
    {
        public int id { get; set; }
        public string status { get; set; }
        public int quantity { get; set; }
        public int washingCycles { get; set; }
        public int limitWashingCycles { get; set; }
        public int maintenanceCycles { get; set; }
        public int limitMaintenance { get; set; }
        public DateTime washingDate { get; set; }
        public bool maintenance { get; set; }
    }
}