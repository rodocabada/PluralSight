using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PalletLinkWebAPI.Models
{
    public class PCB
    {
        public int wipID { get; set; }
        public int customerID { get; set; }
        public string customerText { get; set; }
        public string assembly { get; set; }
        public string number { get; set; }
        public string revision { get; set; }
        public string assemblyID { get; set; }
        public string serialNumber { get; set; }
    }
}