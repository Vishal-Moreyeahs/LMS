using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Domain.Models.Common;

namespace LMS.Domain.Models
{
    public class EmployeeQuizAnswer : BaseEntityClass
    {
        public bool IsCorrect { get; set; }

        [ForeignKey("EmployeeQuiz")]
        public int EmployeeQuiz_Id { get; set; }

        [ForeignKey("QuizQuestion")]
        public int QuizQuestion_Id { get; set; }

        public virtual EmployeeQuiz EmployeeQuiz { get; set; }
        public virtual QuizQuestion QuizQuestions { get; set; }
    }
}
