using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Request
{
    public class CourseContentRequest
    {
        public string Title { get; set; }
        public int Sequence { get; set; }
        public int? Courses_Id { get; set; }
        [Required]
        public string Format { get; set; }
        public string Media { get; set; }
        public bool isActive { get; set; } = true;
    }
    public class CourseContentDto : CourseContentRequest
    { 
        public int Id { get; set; }
    }
}
