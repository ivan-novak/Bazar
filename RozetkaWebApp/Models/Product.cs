﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Product
    {
        public Product()
        {
            Characteristics = new HashSet<Characteristic>();
            Comments = new HashSet<Comment>();
            LineDetails = new HashSet<LineDetail>();
            ProductImages = new HashSet<ProductImage>();
            Views = new HashSet<View>();
        }

        public long ProductId { get; set; }
        public int CatalogId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Attributes { get; set; }

        [DisplayFormat(DataFormatString = "{0:F0}₴", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = false)]
        public decimal Quantity { get; set; }
        public long? PromotionId { get; set; }

        public DateTime? ChoiceDate { get; set; }
        public DateTime? ViewDate { get; set; }
        public string InventoryId { get; set; }
        public long? ViewCount { get; set; }
        public long? ChoiceCount { get; set; }

        public virtual Catalog Catalog { get; set; }
        public virtual Promotion Promotion { get; set; }
        public virtual ICollection<Characteristic> Characteristics { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<LineDetail> LineDetails { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<View> Views { get; set; }
    }
}
