using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LMS.Application.Request;

namespace LMS.Application.Response
{
    public class UserResponse
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string? PermanemtAddress { get; set; }
        public string? TemporaryAddress { get; set; }
        public string PhoneNo { get; set; }

        public int? ReportingManager { get; set; }

        [JsonPropertyName("CompanyId")]
        public int Company_Id { get; set; }

        [JsonPropertyName("RoleId")]
        public int Role_Id { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UserData : UserResponse
    { 
        public int Id { get; set; }
    }
}
