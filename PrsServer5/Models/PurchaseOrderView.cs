using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrsServer5.Models {

    public class PurchaseOrderView {
        public Vendor Vendor { get; set; }
        public List<Requestline> Polines { get; set; } = new List<Requestline>();
        public decimal Total { get; set; } = 0;
    }
}
