using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace LMS.Application.Request
{
    public class DomainRequest
    {
        [Required]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public List<SubDomainRequest> SubDomains { get; set; }
        [Required]
        public string Description { get; set; }

    }

    public class DomainDTO : DomainRequest
    {
        [Required]
        public int Id { get; set; }
    }

    public class SubDomainDTO : SubDomainRequest
    {
        [Required]
        public int Id { get; set; }
    }

    public class SubDomainRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }  = true;
        [Required]
        [JsonPropertyName("domainId")]
        public int Domain_Id { get; set; }
    }
}
