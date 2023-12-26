using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LMS.Application.Request
{
    public class RegistrationRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string UserName { get; set; }

        public string Address { get; set; }

        public string PhoneNo { get; set; }

        [Required]
        [JsonPropertyName("CompanyId")]
        public int Company_Id { get; set; }

        [Required]
        [JsonPropertyName("RoleId")]
        public int Role_Id { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        [MinLength(6)]
        [JsonPropertyName("Password")]
        public string RealPassword { get; set; }
    }
}
