﻿using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class PortalImage1
    {
        public int PortalImageId { get; set; }
        public int PortalId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public long ImageId { get; set; }
    }
}
