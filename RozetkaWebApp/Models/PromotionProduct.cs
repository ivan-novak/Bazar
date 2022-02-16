using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class PromotionProduct
    {
        public long PromotionProductId { get; set; }
        public long PromotionId { get; set; }
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Promotion Promotion { get; set; }
    }
}
