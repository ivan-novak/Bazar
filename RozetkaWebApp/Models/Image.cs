using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Image
    {
        public Image()
        {
            CatalogImages = new HashSet<CatalogImage>();
            Comments = new HashSet<Comment>();
            PortalImages = new HashSet<PortalImage>();
            ProductImages = new HashSet<ProductImage>();
            Promotions = new HashSet<Promotion>();
            RootImages = new HashSet<RootImage>();
        }

        public long ImageId { get; set; }
        public string Title { get; set; }
        public byte[] Data { get; set; }
        public string Url { get { return "/images/" + ImageId.ToString(); } }
        public virtual ICollection<CatalogImage> CatalogImages { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PortalImage> PortalImages { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<Promotion> Promotions { get; set; }
        public virtual ICollection<RootImage> RootImages { get; set; }
    }
}
