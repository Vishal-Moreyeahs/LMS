using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LMS.Domain.Models.Common;

namespace LMS.Domain.Models
{

    public class Company : BaseEntityClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string OfficialMailId { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string PrimaryMailId { get; set; } = null!;

        //[Required]
        //public byte[] Password { get; set; } = null!;

        [Required]
        public string PrimaryContact { get; set; } = null!;

        [Required]
        public string AlternateContact { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;

        [Required]
        public string State { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        [Required]
        public string Pincode { get; set; } = null!;

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Domains> Domains { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<FileBank> FileBanks { get; set; }
        public virtual ICollection<Quiz> Quizes { get; set; }
    }

}
