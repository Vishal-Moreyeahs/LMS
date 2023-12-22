using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Domain.Models.Common;

namespace LMS.Domain.Models
{
    public class EmployeeSubDomain : BaseEntityClass
    {
        public int Employee_Id { get; set; }
        public int SubDomain_Id { get; set; }

        public virtual Employee Employee { get; set; } = null!;
        public virtual SubDomain SubDomain { get; set; } = null!;
    }
}
