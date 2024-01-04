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

        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }


        [DataType(DataType.PhoneNumber)]
        public string? AlternateNo { get; set; }

        public string? PermanemtAddress { get; set; }
        public string? TemporaryAddress { get; set; }

        [Required]
        [JsonPropertyName("roleId")]
        public int Role_Id { get; set; }

        [Required]
        [JsonPropertyName("companyId")]
        public int Company_id { get; set; }

        public bool IsActive { get; set; } = true;

        public int? ReportingManager { get; set; }

        [Required]
        [MinLength(6)]
        [JsonPropertyName("password")]
        public string RealPassword { get; set; }
    }
}
