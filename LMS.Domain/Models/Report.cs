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
    public class Report : BaseEntityClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TotalMarks { get; set; }
        public int ObtainMarks { get; set; }
        public int TotalNoOfQuestion { get; set; }
        public bool ResultStatus { get; set; }
        public int Percentage { get; set; }

        [ForeignKey("EmployeeQuiz")]
        public int EmployeeQuiz_Id { get; set; }

        public virtual EmployeeQuiz EmployeeQuiz { get; set; } = null!;
    }
}
