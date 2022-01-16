using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Property
    {
        public Property()
        {
            Characteristics = new HashSet<Characteristic>();
        }

        public int PropertyId { get; set; }
        public int CatalogId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Format { get; set; }
        public bool? IsNumber { get; set; }
        public string Description { get; set; }

        public virtual Catalog Catalog { get; set; }
        public virtual ICollection<Characteristic> Characteristics { get; set; }
    }
}
