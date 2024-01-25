using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace LMS.Application.Request
{
    public class DomainRequest
    {
        private string _name;

        [Required]
        public string Name
        {
            get => _name;
            set => _name = value?.ToUpper();
        }

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

    }

    public class AddSubDomainModel
    {
        public int DomainId { get; set; }
        public List<SubDomainRequest> SubDomains { get; set; }

    }
}
