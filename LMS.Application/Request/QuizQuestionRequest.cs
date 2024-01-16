using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Request
{
    public class QuizQuestionRequest
    {
        public int SequenceNo { get; set; }
        public int Quiz_Id { get; set; }
        public int QuestionBank_Id { get; set; }
        public int Mark { get; set; }
    }

    public class QuizQuestionDto : QuizQuestionRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
