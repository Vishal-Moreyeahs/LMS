using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LMS.Application.Request
{
    public class CourseRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int SubDomain_Id { get; set; }
        public bool isActive { get; set; } = true;
        public List<CourseContentRequest> CourseContents {get; set;}

        public bool IsMandatory { get; set; } = false;
        public DateTime? EndDate { get; set; }
    }

    public class CourseDTO : CourseRequest
    { 
        public int Id { get; set; }
    }
}
