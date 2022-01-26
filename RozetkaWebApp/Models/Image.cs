using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Image
    {
        public Image()
        {
            CatalogImages = new HashSet<CatalogImage>();
            PortalImages = new HashSet<PortalImage>();
            ProductImages = new HashSet<ProductImage>();
        }

        public long ImageId { get; set; }
        public string Title { get; set; }
        public byte[] Data { get; set; }

        public virtual ICollection<CatalogImage> CatalogImages { get; set; }
        public virtual ICollection<PortalImage> PortalImages { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }

        public FileResult ToStreem()
        {
            if (Data == null) return null;
            System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(Data);
            return new FileStreamResult(oMemoryStream, "image/*");
        }
    }
}
