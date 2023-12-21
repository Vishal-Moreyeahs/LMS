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
        [ForeignKey("Employee_Id")]
        public virtual Employee Employee { get; set; }
        public int Employee_Id { get; set; }

        [ForeignKey("SubDomain_Id")]
        public virtual SubDomain SubDomain { get; set; }
        public int SubDomain_Id { get; set; }
    }
}
