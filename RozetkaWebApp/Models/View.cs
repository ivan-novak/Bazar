using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class View
    {
        public long ViewId { get; set; }
        public long? ProductId { get; set; }
        public DateTime EventDate { get; set; }
        public string UserId { get; set; }

        public virtual Product Product { get; set; }
        public virtual AspNetUser User { get; set; }
    }
}
