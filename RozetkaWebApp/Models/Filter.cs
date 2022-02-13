using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Filter
    {
        public int CatalogId { get; set; }
        public int PropertyId { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
    }
}
