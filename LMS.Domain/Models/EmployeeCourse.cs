using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LMS.Domain.Models.Common;

namespace LMS.Domain.Models
{
    public class EmployeeCourse : BaseEntityClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Employee_Id { get; set; }
        public int Courses_Id { get; set; }
        public int? Group_Id { get; set; }

        public virtual Course Courses { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
        public virtual Group Group { get; set; } = null!;
    }

}
