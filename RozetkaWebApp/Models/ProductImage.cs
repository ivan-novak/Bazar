using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class ProductImage
    {
        public long ProductImageId { get; set; }
        public long ProductId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Path { get; set; }

        public virtual Product Product { get; set; }
    }
}
