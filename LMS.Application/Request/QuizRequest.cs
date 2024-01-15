using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Request
{
    public class QuizRequest
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Required]
        public int Time { get; set; }

        [Required]
        public int RetakeCount { get; set; }

        [Required]
        public int PassingCriteria { get; set; }

        [Required]
        public bool IsMandatory { get; set; }

        [Required]
        public int SubDomain_Id { get; set; }   //if we have subDomain Id then why we need to use company_id here

        [Required]
        public int Courses_Id { get; set; }

        public bool IsActive { get; set; } = true;

    }

    public class QuizDto : QuizRequest
    {
        [Required]
        public int Id { get; set; }
    }

}
