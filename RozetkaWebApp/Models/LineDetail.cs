using System;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class LineDetail
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

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
