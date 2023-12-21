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
    public class QuizQuestion : BaseEntityClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SequenceNo { get; set; }

        [ForeignKey("Quiz")]
        public int Quiz_Id { get; set; }

        [ForeignKey("QuestionBank")]
        public int QuestionBank_Id { get; set; }
        public int Mark { get; set; }

        public virtual QuestionBank QuestionBank { get; set; } = null!;
        public virtual Quiz Quiz { get; set; } = null!;
    }
}
