using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Image
    {
        public int ImageId { get; set; }
        public string Title { get; set; }
        public byte[] Data { get; set; }
    }
}
