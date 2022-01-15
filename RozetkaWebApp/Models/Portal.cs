using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Portal
    {
        public Portal()
        {
            Catalogs = new HashSet<Catalog>();
            PortalImages = new HashSet<PortalImage>();
        }

        public int PortalId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Discription { get; set; }
        public string Attributes { get; set; }

        public virtual ICollection<Catalog> Catalogs { get; set; }
        public virtual ICollection<PortalImage> PortalImages { get; set; }
    }
}
