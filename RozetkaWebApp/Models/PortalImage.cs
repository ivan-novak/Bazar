﻿using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class PortalImage
    {
        public int PortalImageId { get; set; }
        public int PortalId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public long ImageId { get; set; }

        public virtual Image Image { get; set; }
        public virtual Portal Portal { get; set; }
    }
}
