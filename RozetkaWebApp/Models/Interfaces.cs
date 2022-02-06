namespace RozetkaWebApp.Models
{
    public interface iFilter
    {
        string Label { get; set; }
        string Value { get; set; }
        public int PropertyId { get; set; }
    }
    public partial class Filter : iFilter
    {

    }

    public  interface iPortal
    {     
        public int PortalId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Attributes { get; set; }
    }

    public partial class Portal : iPortal
    {

    }

    public interface iCatalog
    {

        public int CatalogId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Attributes { get; set; }

    }

    public partial class Catalog : iCatalog
    {

    }

    public interface iProduct
    {     
        public long ProductId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Attributes { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }

    public partial class Product : iProduct
    {

    }

    public interface iProperty
    {      
        public int PropertyId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Category { get; set; }
        public string Format { get; set; }
        public string Mask { get; set; }
        public bool? IsNumber { get; set; }
        public string Description { get; set; }  
    }

    public partial class Property : iProperty
    {

    }

    public interface iCharacteristic
    {
        public long CharacteristicId { get; set; }
        public long ProductId { get; set; }
        public int PropertyId { get; set; }
        public string Value { get; set; }
        public string Dimension { get; set; }
    }

    public partial class Characteristic : iCharacteristic
    {

    }

    public interface iImage
    {
      
        public long ImageId { get; set; }
        public string Title { get; set; }

   
    }

    public partial class Image : iImage
    {

    }

    public interface iRootImage
    {
        public int RootImageId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public long ImageId { get; set; }

    }

    public partial class RootImage : iRootImage
    {

    }

    public interface iPortalImage
    {
        public int PortalImageId { get; set; }
        public int PortalId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public long ImageId { get; set; }
    }
    public partial class PortalImage : iPortalImage
    {

    }

    public interface iProductImage
    {
        public long ProductImageId { get; set; }
        public long ProductId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public long ImageId { get; set; }
    }

    public partial class ProductImage : iProductImage
    {

    }

    public interface iCatalogImage
    {
        public int Id { get; set; }
        public int CatalogId { get; set; }
        public long ImageId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
   
    }
    public partial class CatalogImage : iCatalogImage
    {

    }

}


