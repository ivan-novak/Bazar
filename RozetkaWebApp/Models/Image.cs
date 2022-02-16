using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Image
    {
        public Image()
        {
            CatalogImages = new HashSet<CatalogImage>();
            CommentImages = new HashSet<CommentImage>();
            PortalImages = new HashSet<PortalImage>();
            ProductImages = new HashSet<ProductImage>();
            PromotionImages = new HashSet<PromotionImage>();
            RootImages = new HashSet<RootImage>();
        }

        public long ImageId { get; set; }
        public string Title { get; set; }
        public byte[] Data { get; set; }

        public virtual ICollection<CatalogImage> CatalogImages { get; set; }
        public virtual ICollection<CommentImage> CommentImages { get; set; }
        public virtual ICollection<PortalImage> PortalImages { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<PromotionImage> PromotionImages { get; set; }
        public virtual ICollection<RootImage> RootImages { get; set; }
    }
}
