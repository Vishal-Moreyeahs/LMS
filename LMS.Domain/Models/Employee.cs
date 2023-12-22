using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LMS.Domain.Models.Common;

namespace LMS.Domain.Models
{
    public class Employee : BaseEntityClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }
        public string AlternateNo { get; set; }
        public string PermanemtAddress { get; set; }
        public string TemporaryAddress { get; set; }

        [ForeignKey("Company")]
        public int Company_Id { get; set; }
        public int ReportingManager { get; set; }

        [ForeignKey("Role")]
        public int Role_Id { get; set; }

        public virtual Company Company { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<EmployeeQuiz> EmployeeQuizes { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}
