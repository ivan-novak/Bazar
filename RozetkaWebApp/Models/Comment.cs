using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Comment
    {
        public long CommentId { get; set; }
        public string UserId { get; set; }
        public long ProductId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public decimal? Score { get; set; }
        public long? ImageId { get; set; }

        public virtual Image Image { get; set; }
        public virtual Product Product { get; set; }
        public virtual AspNetUser User { get; set; }
    }
}
