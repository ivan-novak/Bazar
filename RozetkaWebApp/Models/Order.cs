using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Order
    {
        public Order()
        {
            LineDetails = new HashSet<LineDetail>();
        }

        public long OrderId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string CardNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryContact { get; set; }
        public string DeliveryEmail { get; set; }
        public string DeliveryPhone { get; set; }
        public string ExtOrderNbr { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual ICollection<LineDetail> LineDetails { get; set; }
    }
}
