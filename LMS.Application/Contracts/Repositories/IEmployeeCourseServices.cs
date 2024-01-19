using LMS.Application.Models;
using LMS.Application.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Contracts.Repositories
{
    public interface IEmployeeCourseServices
    {
        Task<Response<dynamic>> AssignCourseToEmployee(List<EmployeeCourseRequest> employeeCourses);
        Task<Response<dynamic>> GetCoursesByEmployeeId(int employeeId);
        Task<Response<dynamic>> DeleteCourseForEmployee(int employeeCourseId);
    }
}
