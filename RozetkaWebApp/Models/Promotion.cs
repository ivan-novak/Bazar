using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Promotion
    {
        public Promotion()
        {
            PromotionImages = new HashSet<PromotionImage>();
            PromotionProducts = new HashSet<PromotionProduct>();
        }

        public long PromotionId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Attributes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<PromotionImage> PromotionImages { get; set; }
        public virtual ICollection<PromotionProduct> PromotionProducts { get; set; }
    }
}
