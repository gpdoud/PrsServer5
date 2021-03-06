using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrsServer5.Models {

    public class Product {

        public int Id { get; set; }
        [StringLength(30), Required]
        public string PartNbr { get; set; }
        [StringLength(30), Required]
        public string Description { get; set; }
        [Column(TypeName = "decimal(9,2)")]
        public decimal Price { get; set; }
        [StringLength(15)]
        public string Unit { get; set; } = "Each";
        public string PhotoPath { get; set; }

        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }

        public Product() {
        }
    }
}
