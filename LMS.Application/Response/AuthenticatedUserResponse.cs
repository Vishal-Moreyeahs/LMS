using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Response
{
    public class AuthenticatedUserResponse
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        //public string RoleName { get; set; }
        public int CompanyId { get; set; }
        //public string CompanyName { get; set;}
    }
}
