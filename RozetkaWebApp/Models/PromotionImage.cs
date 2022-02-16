using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class PromotionImage
    {
        public long PromotionImageId { get; set; }
        public long PromotionId { get; set; }
        public long ImageId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }

        public virtual Image Image { get; set; }
        public virtual Promotion Promotion { get; set; }
    }
}
