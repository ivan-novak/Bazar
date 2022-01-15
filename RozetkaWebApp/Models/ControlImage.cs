using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class ControlImage
    {
        public int ControlImageId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Path { get; set; }
    }
}
