using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public long ProductId { get; set; }
        public string UserId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        public virtual Product Product { get; set; }
    }
}
