using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LMS.Domain.Models.Common;

namespace LMS.Domain.Models
{
    public class EmployeeCourse : BaseEntityClass
    {
        [ForeignKey("Employee_Id")]
        public virtual Employee Employee { get; set; }
        public int Employee_Id { get; set; }

        [ForeignKey("Courses_Id")]
        public virtual Course Courses { get; set; }
        public int Courses_Id { get; set; }

        [ForeignKey("Group_Id")]
        public virtual Group Group { get; set; }
        public int Group_Id { get; set; }

    }
}
