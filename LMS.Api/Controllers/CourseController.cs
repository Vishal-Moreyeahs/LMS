using LMS.Application.Contracts.Repositories;
using LMS.Application.Request;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseServices _courseServices;
        public CourseController(ICourseServices courseServices)
        {
            _courseServices = courseServices;
        }

        [HttpPost]
        [Route("addCourse")]
        public async Task<IActionResult> AddCourse(CourseRequest course)
        {
            return Ok(await _courseServices.AddCourse(course));
        }

        [HttpGet]
        [Route("getAllCourses")]
        public async Task<IActionResult> GetAllCourses()
        {
            return Ok(await _courseServices.GetAllCourse());
        }

        [HttpGet]
        [Route("getCourseById")]
        public async Task<IActionResult> GetCourse(int id)
        {
            return Ok(await _courseServices.GetCourseById(id));
        }

        [HttpGet]
        [Route("getAllContentType")]
        public async Task<IActionResult> GetContentType()
        {
            return Ok(await _courseServices.GetAllContentType());
        }
    }
}
