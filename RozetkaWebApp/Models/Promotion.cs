using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Promotion
    {
        public Promotion()
        {
            Products = new HashSet<Product>();
        }

        public long PromotionId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Attributes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long? ImageId { get; set; }

        public virtual Image Image { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
