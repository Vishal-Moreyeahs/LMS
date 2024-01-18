using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Request
{
    public class EmployeeCourseRequest
    {
        [Required]
        public int Employee_Id { get; set; }

        [Required]
        public int Courses_Id { get; set; }
        public int? Group_Id { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
