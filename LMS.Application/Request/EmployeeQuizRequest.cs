using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Request
{
    public class EmployeeQuizRequest
    {
        public int Attempt { get; set; }
        public int Employee_Id { get; set; }
        public int Quiz_Id { get; set; }
        public int? Group_Id { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class EmployeeQuizDto
    { 
        public int Id { get; set; }
    }
}
