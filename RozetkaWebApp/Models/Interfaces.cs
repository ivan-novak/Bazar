using System;

namespace RozetkaWebApp.Models
{
    public interface iFilter
    {
        public int PropertyId { get; set; }    
        string Label { get; set; }
        string Value { get; set; }

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
        public string Label { get; }
        public string Value { get; set; }
        public string Dimension { get; set; }
    }

    public partial class Characteristic : iCharacteristic
    {
        public string Label { get { return Property.Label; } }
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
        public string Url { get { return "/images/" + ImageId.ToString(); } }
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
        public string Url { get { return "/images/" + ImageId.ToString(); } }
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
        public string Url { get { return "/images/" + ImageId.ToString(); } }

    }
    public partial class CatalogImage : iCatalogImage
    {
    }

    public interface iWallett
    {
        public long WalletId { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string Cardholder { get; set; }
        public string CardType { get; set; }
        public string VerificationCode { get; set; }
        public string UserId { get; set; }
    }
    public partial class Wallett : iWallett
    { 
    }


    public interface iAddress
    {
        public int AddressId { get; set; }
        public string AddressType { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string UserId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public int? ExtAddressId { get; set; }
    }

    public partial class Address : iAddress
    {
    }


    public interface iContact
    {
        public int ContactId { get; set; }
        public string ContactType { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string Title { get; set; }
        public string Salutation { get; set; }
        public string Attention { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string Fax { get; set; }
        public string FaxType { get; set; }
        public string Phone1 { get; set; }
        public string Phone1Type { get; set; }
        public string Phone2 { get; set; }
        public string Phone2Type { get; set; }
        public string Phone3 { get; set; }
        public string Phone3Type { get; set; }
        public int DefAddressId { get; set; }
        public string UserId { get; set; }
        public DateTime? AssignDate { get; set; }
        public int? ExtAddressId { get; set; }
    }

    public partial class Contact : iContact
    {
    }


    public interface iOrder
    {
        public long OrderId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string CardNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryContact { get; set; }
        public string DeliveryEmail { get; set; }
        public string DeliveryPhone { get; set; }
        public string ExtOrderNbr { get; set; }
    }
    public partial class Order : iOrder
    {
    }

    public interface iOrderDetail
    {
        public long OrderDatailId { get; set; }
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantities { get; set; }
        public decimal UnitCost { get; set; }
        public string Status { get; set; }
        public string ExtOrderDetailNbr { get; set; }      
    }

    public partial class OrderDetail : iOrderDetail
    {
    }

    public interface iAspNetUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
    }

    public partial class AspNetUser : iAspNetUser
    {
    }

    public interface iPromotion
    {
        public long PromotionId { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Attributes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long? ImageId { get; set; }
    }

    public partial class Promotion : iPromotion
    {
    }

    public interface iComment
    {

        public long CommentId { get; set; }
        public string UserId { get; set; }
        public long ProductId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public decimal? Score { get; set; }
        public long? ImageId { get; set; }
    }

    public partial class Comment : iComment
    {
    }

}



