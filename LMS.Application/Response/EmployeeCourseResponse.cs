using LMS.Application.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Response
{
    public class EmployeeCourseResponse
    {
        public string Name { get; set; }

        public List<CourseRequest> Courses { get; set; }
    }
}
