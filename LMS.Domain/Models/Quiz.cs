﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Domain.Models.Common;

namespace LMS.Domain.Models
{
    public class Quiz : BaseEntityClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; }
        public int Time { get; set; }
        public int RetakeCount { get; set; }
        public int PassingCriteria { get; set; }
        public bool IsMandatory { get; set; }



        [ForeignKey("SubDomain_Id")]
        public virtual SubDomain SubDomain { get; set; } = null!;
        public int SubDomain_Id { get; set; }

        [ForeignKey("Courses_Id")]
        public virtual Course Courses { get; set; } = null!;
        public int Courses_Id { get; set; }

        [ForeignKey("Company_Id")]
        public virtual Company Company { get; set; } = null!;
        public int Company_Id { get; set; }
        public virtual ICollection<EmployeeQuiz> EmployeeQuizes { get; set; }
        public virtual ICollection<QuizQuestion> QuizQuestions { get; set; }
    }
}
