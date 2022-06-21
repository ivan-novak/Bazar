#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Wallett
    {
        public long WalletId { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string Cardholder { get; set; }
        public string CardType { get; set; }
        public string VerificationCode { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUser User { get; set; }
    }
}
