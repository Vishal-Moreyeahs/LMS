using LMS.Application.Contracts.Repositories;
using LMS.Application.Request;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeCourseController : ControllerBase
    {
        private readonly IEmployeeCourseServices _employeeCourseServices;

        public EmployeeCourseController(IEmployeeCourseServices employeeCourseServices)
        {
            _employeeCourseServices = employeeCourseServices;
        }

        [HttpPost]
        [Route("assignCourseToEmployees")]
        public async Task<IActionResult> AddQuestionToQuestionBank(List<EmployeeCourseRequest> employeeCourses)
        {
            return Ok(await _employeeCourseServices.AssignCourseToEmployee(employeeCourses));
        }

        [HttpDelete]
        [Route("unassignCourseToEmployee")]
        public async Task<IActionResult> DeleteCourseForEmployee(int employeeCourseId) //or bulk unassignment
        {
            return Ok(await _employeeCourseServices.DeleteCourseForEmployee(employeeCourseId));
        }

        [HttpGet]
        [Route("getCoursesForEmployee")]
        public async Task<IActionResult> GetCoursesForEmployee(int employeeId) 
        {
            return Ok(await _employeeCourseServices.GetCoursesByEmployeeId(employeeId));
        }
    }
}
