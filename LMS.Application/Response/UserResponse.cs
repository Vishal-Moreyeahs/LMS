using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LMS.Application.Response
{
    public class UserResponse
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNo { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? AlternateNo { get; set; }
        public string? PermanemtAddress { get; set; }
        public string? TemporaryAddress { get; set; }

        public int? ReportingManager { get; set; }
        [Required]
        [JsonPropertyName("CompanyId")]
        public int Company_Id { get; set; }

        [Required]
        [JsonPropertyName("RoleId")]
        public int Role_Id { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UserData : UserResponse
    { 
        public int Id { get; set; }
    }
}
