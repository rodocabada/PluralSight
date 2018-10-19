using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PalletLinkWebAPI.Models
{
    public class LinkingLog
    {
        public PCB pcb { get; set; }
        public string palletID { get; set; }
        public string palletSerial { get; set; }
        public string panelSizePL { get; set; }
        public string panelSizeMES { get; set; }
        public string linkObject { get; set; }
        public string linkMaterialID { get; set; }
        public string equipmentValue { get; set; }
        public string equipmentName { get; set; }
        public string routeStepID { get; set; }
        public string serialLoops { get; set; }
        public string loopsAllowed { get; set; }
        public string message { get; set; }
    }
}