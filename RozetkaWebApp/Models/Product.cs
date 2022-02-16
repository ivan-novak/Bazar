﻿using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Product
    {
        public Product()
        {
            Characteristics = new HashSet<Characteristic>();
            Comments = new HashSet<Comment>();
            OrderDetails = new HashSet<OrderDetail>();
            ProductImages = new HashSet<ProductImage>();
            PromotionProducts = new HashSet<PromotionProduct>();
            Views = new HashSet<View>();
        }

        public long ProductId { get; set; }
        public int CatalogId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Attributes { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }

        public virtual Catalog Catalog { get; set; }
        public virtual ICollection<Characteristic> Characteristics { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<PromotionProduct> PromotionProducts { get; set; }
        public virtual ICollection<View> Views { get; set; }
    }
}
