using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class ProductImage
    {
        public long ProductImageId { get; set; }
        public long ProductId { get; set; }
        public string Caption { get; set; }
        public string Path { get; set; }

        public virtual Product Product { get; set; }
    }
}
