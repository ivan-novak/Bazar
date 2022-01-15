using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class CatalogImage
    {
        public int CatalogImageId { get; set; }
        public int CatalogId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Path { get; set; }

        public virtual Catalog Catalog { get; set; }
    }
}
