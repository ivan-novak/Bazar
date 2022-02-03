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
            ProductImages = new HashSet<ProductImage>();
            RootImages = new HashSet<RootImage>();
        }

        public long ImageId { get; set; }
        public string Title { get; set; }
        public byte[] Data { get; set; }

        public virtual ICollection<CatalogImage> CatalogImages { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<RootImage> RootImages { get; set; }
    }
}
