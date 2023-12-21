using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LMS.Domain.Models.Common;

namespace LMS.Domain.Models
{
    public class EmployeeQuiz : BaseEntityClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Attempt { get; set; }

        //[ForeignKey("Employee")]
        public int Employee_Id { get; set; }

        //[ForeignKey("Quiz")]
        public int Quiz_Id { get; set; }

        public DateTime EndDate { get; set; }

        //[ForeignKey("Group")]
        public int Group_Id { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Group Group { get; set; }
        public virtual Quiz Quiz { get; set; } 
        public virtual ICollection<Report> Reports { get; set; }
    }
}
