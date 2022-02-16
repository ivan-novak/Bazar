using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class CommentImage
    {
        public long CommentImageId { get; set; }
        public long CommentId { get; set; }
        public long ImageId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual Image Image { get; set; }
    }
}
