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
            ProductImages = new HashSet<ProductImage>();
            RootImages = new HashSet<RootImage>();
        }

        public long ImageId { get; set; }
        public string Title { get; set; }
        public byte[] Data { get; set; }

        public virtual ICollection<CatalogImage> CatalogImages { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<RootImage> RootImages { get; set; }

        public FileResult ToStream()
        {
            if (Data == null) return null;          
            System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(Data);
            return new FileStreamResult(oMemoryStream, "image/*");
        }
    }
}

