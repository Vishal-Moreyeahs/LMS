using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Domain.Models.Common;
using Microsoft.VisualBasic.FileIO;

namespace LMS.Domain.Models
{
    public class QuestionBank : BaseEntityClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsImageAttached { get; set; }
        public byte[] ImagePath { get; set; }

        [ForeignKey("SubDomain")]
        public int SubDomain_Id { get; set; }

        public virtual SubDomain SubDomain { get; set; }
        public virtual ICollection<QuizOption> QuizOptions { get; set; }
        public virtual ICollection<QuizQuestion> QuizQuestions { get; set; }
    }
}
