using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Domain.Models.Common;

namespace LMS.Domain.Models
{
    public class Course : BaseEntityClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        [ForeignKey("Company")]
        public int Company_Id { get; set; }

        [ForeignKey("SubDomain")]
        public int? SubDomain_Id { get; set; }
        public bool IsMandatory { get; set; }
        public DateTime? EndDate { get; set; }


        // Navigation properties
        public virtual Company Company { get; set; }
        
        public virtual SubDomain SubDomain { get; set; }

        public virtual ICollection<CourseContent> CourseContents { get; set; }
        public virtual ICollection<Quiz> Quizes { get; set; }
    }
    
}
