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
    public class SubDomain : BaseEntityClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } 
        public string? Description { get; set; }
        public int Domain_Id { get; set; }

        public virtual Domains Domain { get; set; } = null!;
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<QuestionBank> QuestionBanks { get; set; }
        public virtual ICollection<Quiz> Quizes { get; set; }
    }
}
