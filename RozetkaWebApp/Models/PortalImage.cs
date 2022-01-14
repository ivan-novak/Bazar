using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class PortalImage
    {
        public int PortalImageId { get; set; }
        public int PortalId { get; set; }
        public string Caption { get; set; }
        public string Path { get; set; }

        public virtual Portal Portal { get; set; }
    }
}
