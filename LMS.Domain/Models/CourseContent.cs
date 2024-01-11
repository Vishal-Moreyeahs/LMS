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
    public class CourseContent : BaseEntityClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Sequence { get; set; }
        public int Courses_Id { get; set; }
        public string Format { get; set; }
        public string Media { get; set; }

        public virtual Course Courses { get; set; }

    }
}
