using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Characteristic
    {
        public long CharacteristicId { get; set; }
        public long ProductId { get; set; }
        public int PropertyId { get; set; }
        public string TextValue { get; set; }
        public decimal? DigitValue { get; set; }

        public virtual Product Product { get; set; }
        public virtual Property Property { get; set; }
    }
}
