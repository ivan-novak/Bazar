using System;
using System.Collections.Generic;

#nullable disable

namespace RozetkaWebApp.Models
{
    public partial class Contact
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

        public virtual AspNetUser User { get; set; }
    }
}
