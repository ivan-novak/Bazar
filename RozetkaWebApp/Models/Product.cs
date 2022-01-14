using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Product
    {
        public Product()
        {
            Characteristics = new HashSet<Characteristic>();
            Orders = new HashSet<Order>();
            ProductImages = new HashSet<ProductImage>();
            Views = new HashSet<View>();
        }

        public long ProductId { get; set; }
        public int CatalogId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Attributes { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }

        public virtual Catalog Catalog { get; set; }
        public virtual ICollection<Characteristic> Characteristics { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<View> Views { get; set; }
    }
}
