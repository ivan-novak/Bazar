#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Address
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

        public virtual AspNetUser User { get; set; }
    }
}
