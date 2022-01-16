using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Catalog
    {
        public Catalog()
        {
            CatalogImages = new HashSet<CatalogImage>();
            Products = new HashSet<Product>();
            Properties = new HashSet<Property>();
        }

        public int CatalogId { get; set; }
        public int PortalId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Attributes { get; set; }

        public virtual Portal Portal { get; set; }
        public virtual ICollection<CatalogImage> CatalogImages { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Property> Properties { get; set; }
    }
}
