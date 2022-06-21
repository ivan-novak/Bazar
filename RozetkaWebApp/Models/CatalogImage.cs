#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class CatalogImage
    {
        public int Id { get; set; }
        public int CatalogId { get; set; }
        public long ImageId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public byte[] Data { get; set; }

        public virtual Catalog Catalog { get; set; }
        public virtual Image Image { get; set; }
    }
}
