using System.ComponentModel.DataAnnotations;

namespace LMS.Application.Request
{
    public class CompanyRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [EmailAddress]
        public string OfficialMailId { get; set; }

        [Required]
        [EmailAddress]
        public string PrimaryMailId { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PrimaryContact { get; set; }

        [Required]
        [Phone]
        public string AlternateContact { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Pincode { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
