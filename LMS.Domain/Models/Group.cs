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
    public class Group : BaseEntityClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } //need to check
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int Employees_Id { get; set; }

        public virtual Employee Employees { get; set; } = null!;
        public virtual ICollection<EmployeeQuiz> EmployeeQuizes { get; set; }

    }
}
