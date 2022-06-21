#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class RootImage
    {
        public int RootImageId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public long ImageId { get; set; }

        public virtual Image Image { get; set; }
    }
}
