using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrsServer5.Models {

    public class Request {

        public static string StatusNew = "NEW";
        public static string StatusReview = "REVIEW";
        public static string StatusApproved = "APPROVED";
        public static string StatusRejected = "REJECTED";
        public static string StatusEdit = "EDIT";

        public int Id { get; set; }
        [StringLength(80), Required]
        public string Description { get; set; }
        [StringLength(80)]
        public string Justification { get; set; }
        [StringLength(80)]
        public string RejectionReason { get; set; }
        [StringLength(80)]
        public string DeliveryMode { get; set; } = "Pickup";
        public string Status { get; set; } = StatusNew;
        [Column(TypeName = "decimal(11,2)")]
        public decimal Total { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public IEnumerable<Requestline> Requestlines { get; set; }

        public Request() {
        }
    }
}
