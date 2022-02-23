using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class CartDetail
    {
        public long OrderDatailId { get; set; }
        public long? OrderId { get; set; }
        public string UserId { get; set; }
        public string CartId { get; set; }
        public long ProductId { get; set; }
        public int Quantities { get; set; }
        public decimal UnitCost { get; set; }
        public string Status { get; set; }
        public string ExtOrderDetailNbr { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
