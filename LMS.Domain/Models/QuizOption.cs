using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Domain.Models.Common;

namespace LMS.Domain.Models
{
    public class QuizOption : BaseEntityClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string OptionValue { get; set; }
        public bool IsImageAttached { get; set; }
        public string ImagePath { get; set; }
        public bool IsCorrect { get; set; }

        [ForeignKey("QuestionBank")]
        public int QuestionBank_Id { get; set; }

        public virtual QuestionBank QuestionBank { get; set; }

    }
}
